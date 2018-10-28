namespace Net
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    public class Rudp {
	    public LinkedList<UdpChunk> recvQueue = new LinkedList<UdpChunk>();
	    public int recvMaxSeq;
	    public int recvMinSeq;
	    public int recvMaxAck;

	    public LinkedList<UdpChunk> sendQueue = new LinkedList<UdpChunk>();
	    public LinkedList<UdpChunk> sendHistory = new LinkedList<UdpChunk>();
	    public int sendSeq;

	    public long curTick;
	    public long recvTick;
	    public long reqTick;
	    public int reqTimes;

	    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
	    // PushQueueFront
	    // AddFirst
	
	    // PushQueueBack
	    // AddLast
	
	    // PushQueueAfter
	    // AddAfter
	
	    public void PushQueueBackSort(LinkedList<UdpChunk> q, UdpChunk c) {
		    LinkedListNode<UdpChunk> n = q.Last;
		    while (n != null) {
			    if (n.Value.seq <= c.seq) {
				    q.AddAfter(n, c);
				    return;
			    }
			    n = n.Previous;
		    }
		    q.AddLast(c);
	    }

	    public bool PushQueueBackSortCheckSame(LinkedList<UdpChunk> q, UdpChunk c) {
		    LinkedListNode<UdpChunk> n = q.Last;
		    while (n != null) {
			    if (n.Value.seq == c.seq) {
				    return false;
			    }
			    if (n.Value.seq < c.seq) {
				    q.AddAfter(n, c);
				    return true;
			    }
			    n = n.Previous;
		    }
		    q.AddLast(c);
		    return true;
	    }

	    // PopQueueFront
	    // First RemoveFirst
	
	    public UdpChunk PopQueueBySeq(LinkedList<UdpChunk> q, int seq) {
		    LinkedListNode<UdpChunk> n = q.Last;
		    while (n != null) {
			    if (n.Value.seq == seq) {
				    q.Remove(n);
				    return n.Value;
			    }
			    n = n.Previous;
		    }
		    return null;
	    }

	    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
	    public void RecvMinSeqUpdate() {
		    LinkedListNode<UdpChunk> n = recvQueue.First;
		    while (n != null) {
			    if (n.Value.seq > recvMinSeq) {
				    if (n.Value.seq == recvMinSeq + 1) {
					    recvMinSeq = n.Value.seq;
				        reqTimes = 0;
				    } else {
					    return;
				    }
			    }
			    n = n.Next;
		    }
	    }

	    public void RecvBuffIn(UdpChunk c) {
		    switch (c.type) {
			    case UdpConst.udpTypeReqSeq:
			        {
				        UdpChunk cAgain = PopQueueBySeq(sendHistory, c.ack);
				        if (cAgain == null) {
					        break;
				        }
				        sendQueue.AddFirst(cAgain);
			        }
                    break;
			    case UdpConst.udpTypeSynAck:
				    break;
			    case UdpConst.udpTypeData:
			    case UdpConst.udpTypeDataAgain:
				    if (recvMinSeq <= c.seq) {
					    break;
				    }
				    if (PushQueueBackSortCheckSame(recvQueue, c)) {
					    if (recvMinSeq < c.seq) {
						    RecvMinSeqUpdate();
					    }
					    if (recvMaxSeq < c.seq) {
						    recvMaxSeq = c.seq;
					    }
					    if (recvMaxAck < c.ack) {
						    recvMaxAck = c.ack;
						    SendHistoryClear();
					    }
					    recvTick = curTick;
				    }
                    break;
			    default :
				    break;
		    }
	    }
	
	    public UdpChunk RecvBuffOut() {
		    LinkedListNode<UdpChunk> n = recvQueue.First;
		    if (n == null) {
			    return null;
		    }
		    UdpChunk c = n.Value;
		    if (c.seq <= recvMinSeq) {
			    return c;
		    }
		    return null;
	    }


	    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
	    public void SendHistoryClear() {
		    LinkedListNode<UdpChunk> n = sendHistory.First;
		    while (n != null && n.Value.seq <= recvMaxAck) {
			    sendHistory.Remove(n);
			    n = sendHistory.First;
		    }
	    }

	    void SendReqAgain() {
		    if (recvMinSeq == recvMaxSeq) {
			    return;
		    }
		    UdpChunk c = new UdpChunk();
		    c.size = 0;
		    c.type = UdpConst.udpTypeReqSeq;
		    c.seq = 0;
		    c.ack = recvMinSeq + 1;
		    sendQueue.AddFirst(c);
		    ++reqTimes;
	    } 

	    public void SendBuffIn(byte[] buff, int size) {
		    if (size > UdpConst.udpDataMaxLen) {
			    return;
		    }
		    UdpChunk c = new UdpChunk();
		    c.size = (byte)size;
		    c.type = UdpConst.udpTypeData;
		    c.seq = ++sendSeq;
		    c.ack = recvMinSeq;
		    Array.Copy(buff, c.buff, size);
		    sendQueue.AddLast(c);
	    }

	   public UdpChunk SendBuffOut() {
		    LinkedListNode<UdpChunk> n = sendQueue.First;
		    if (n == null) {
			    return null;
		    }
            sendQueue.RemoveFirst();
		    UdpChunk c = n.Value;
		    return c;
	    }
	    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
	    public void Init(long tick) {
		    recvMaxSeq = 0;
		    recvMinSeq = 0;
		    recvMaxAck = 0;
		    sendSeq = 0;
		    curTick = tick;
		    recvTick = tick;
		    reqTick = 0;
		    reqTimes = 0;
	    }

	    public int HandleProcess(long tick) {
		    curTick = tick;
		    if (curTick - recvTick > UdpConst.udpTimeOutTicks) {
			    return -1;
		    }
		    if (recvMaxSeq - recvMinSeq > UdpConst.udpRecvSeqMaxDev) {
			    return -1;
		    }
		    if (curTick - reqTick > UdpConst.udpReqAgainTicks) {
			    SendReqAgain();
			    reqTick = curTick;
		    }
		    return 0;
	    }
    }
}