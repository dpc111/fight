namespace Net_
{
  	using UnityEngine; 
	using System; 
	using System.Collections;
	using System.Collections.Generic;
	
    //这个模块将多个数据包打捆在一起
    //由于每个数据包都有最大上限， 向Bundle中写入大量数据将会在内部产生多个MemoryStream
    //在send时会全部发送出去
	public class Bundle : ObjectPool<Bundle>
    {
		public MemoryStream stream = new MemoryStream();
		public List<MemoryStream> streamList = new List<MemoryStream>();
		public int numMessage = 0;
		public int messageLength = 0;
		public Message msgtype = null;
		private int curMsgStreamIndex = 0;
		
		public Bundle()
		{
		}

		public void Clear()
		{
			stream = MemoryStream.CreateObject();
			streamList = new List<MemoryStream>();
			numMessage = 0;
			messageLength = 0;
			msgtype = null;
			curMsgStreamIndex = 0;
		}

		//把自己放回缓冲池
		public void ReclaimObject()
		{
			Clear();
			ReclaimObject(this);
		}
		
		public void NewMessage(Message mt)
		{
			Fini(false);
			msgtype = mt;
			numMessage += 1;
			WriteUint16(msgtype.id);
			if(msgtype.msglen == -1)
			{
				WriteUint16(0);
				messageLength = 0;
			}
			curMsgStreamIndex = 0;
		}
		
		public void WriteMsgLength()
		{
			if(msgtype.msglen != -1)
				return;
			MemoryStream stream = this.stream;
			if(curMsgStreamIndex > 0)
			{
				stream = streamList[streamList.Count - curMsgStreamIndex];
			}
			stream.Data()[2] = (Byte)(messageLength & 0xff);
			stream.Data()[3] = (Byte)(messageLength >> 8 & 0xff);
		}
		
		public void Fini(bool issend)
		{
			if(numMessage > 0)
			{
				WriteMsgLength();
				streamList.Add(stream);
				stream = MemoryStream.CreateObject();
			}
			if(issend)
			{
				numMessage = 0;
				msgtype = null;
			}
			curMsgStreamIndex = 0;
		}
		
		public void send(NetworkInterface networkInterface)
		{
			Fini(true);
			
			if(networkInterface.Valid())
			{
				for(int i=0; i<streamList.Count; i++)
				{
					MemoryStream tempStream = streamList[i];
					networkInterface.Send(tempStream);
				}
			}
			else
			{
				Dbg.ErrorMsg("Bundle::send: networkInterface invalid!");  
			}

			// 把不用的MemoryStream放回缓冲池，以减少垃圾回收的消耗
			for (int i = 0; i < streamList.Count; ++i)
			{
				streamList[i].ReclaimObject();
			}

			streamList.Clear();
			stream.Clear();

			// 我们认为，发送完成，就视为这个bundle不再使用了，
			// 所以我们会把它放回对象池，以减少垃圾回收带来的消耗，
			// 如果需要继续使用，应该重新Bundle.createObject()，
			// 如果外面不重新createObject()而直接使用，就可能会出现莫名的问题，
			// 仅以此备注，警示使用者。
			Bundle.ReclaimObject(this);
		}
		
		public void checkStream(int v)
		{
			if(v > stream.Space())
			{
				streamList.Add(stream);
				stream = MemoryStream.CreateObject();
				++ curMsgStreamIndex;
			}
	
			messageLength += v;
		}
		
		//---------------------------------------------------------------------------------
		public void WriteInt8(SByte v)
		{
			checkStream(1);
			stream.WriteInt8(v);
		}
	
		public void WriteInt16(Int16 v)
		{
			checkStream(2);
			stream.WriteInt16(v);
		}
			
		public void WriteInt32(Int32 v)
		{
			checkStream(4);
			stream.WriteInt32(v);
		}
	
		public void WriteInt64(Int64 v)
		{
			checkStream(8);
			stream.WriteInt64(v);
		}
		
		public void WriteUint8(Byte v)
		{
			checkStream(1);
			stream.WriteUint8(v);
		}
	
		public void WriteUint16(UInt16 v)
		{
			checkStream(2);
			stream.WriteUint16(v);
		}
			
		public void WriteUint32(UInt32 v)
		{
			checkStream(4);
			stream.WriteUint32(v);
		}
	
		public void WriteUint64(UInt64 v)
		{
			checkStream(8);
			stream.WriteUint64(v);
		}
		
		public void WriteFloat(float v)
		{
			checkStream(4);
			stream.WriteFloat(v);
		}
	
		public void WriteDouble(double v)
		{
			checkStream(8);
			stream.WriteDouble(v);
		}
		
		public void WriteString(string v)
		{
			checkStream(v.Length + 1);
			stream.WriteString(v);
		}
		
		public void WriteBlob(byte[] v)
		{
			checkStream(v.Length + 4);
			stream.WriteBlob(v);
		}
    }
} 
