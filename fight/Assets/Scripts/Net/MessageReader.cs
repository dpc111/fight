namespace Net
{
  	using UnityEngine; 
	using System; 
	
	using MsgID = System.UInt16;
	using MsgLength = System.UInt16;
	using MsgLengthEx = System.UInt32;
	
    //消息阅读模块
    //从数据包流中分析出所有的消息包并将其交给对应的消息处理函数
    public class MessageReader
    {
		enum READ_STATE
		{
			//消息ID
			READ_STATE_MSGID = 0,
			//消息的长度65535以内
			READ_STATE_MSGLEN = 1,
			//当上面的消息长度都无法到达要求时使用扩展长度
			//uint32
			READ_STATE_MSGLEN_EX = 2,
			// 消息的内容
			READ_STATE_BODY = 3
		}
		
		private MsgID msgid = 0;
		private MsgLength msglen = 0;
		private MsgLengthEx expectSize = 2;
		private READ_STATE state = READ_STATE.READ_STATE_MSGID;
		private MemoryStream stream = new MemoryStream();
		
		public MessageReader()
		{
		}
		
		public void Process(byte[] datas, MsgLengthEx offset, MsgLengthEx length)
		{
			MsgLengthEx totallen = offset;
			while(length > 0 && expectSize > 0)
			{
				if(state == READ_STATE.READ_STATE_MSGID)
				{
					if(length >= expectSize)
					{
						Array.Copy(datas, totallen, stream.Data(), stream.wpos, expectSize);
						totallen += expectSize;
						stream.wpos += (int)expectSize;
						length -= expectSize;
						msgid = stream.ReadUint16();
						stream.Clear();

						Message msg = Message.clientMessages[msgid];

						if(msg.msglen == -1)
						{
							state = READ_STATE.READ_STATE_MSGLEN;
							expectSize = 2;
						}
						else if(msg.msglen == 0)
						{
							// 如果是0个参数的消息，那么没有后续内容可读了，处理本条消息并且直接跳到下一条消息
							#if UNITY_EDITOR
							Dbg.ProfileStart(msg.name);
							#endif

							msg.HandleMessage(stream);

							#if UNITY_EDITOR
							Dbg.ProfileEnd(msg.name);
							#endif

							state = READ_STATE.READ_STATE_MSGID;
							expectSize = 2;
						}
						else
						{
							expectSize = (MsgLengthEx)msg.msglen;
							state = READ_STATE.READ_STATE_BODY;
						}
					}
					else
					{
						Array.Copy(datas, totallen, stream.Data(), stream.wpos, length);
						stream.wpos += (int)length;
						expectSize -= length;
						break;
					}
				}
				else if(state == READ_STATE.READ_STATE_MSGLEN)
				{
					if(length >= expectSize)
					{
						Array.Copy(datas, totallen, stream.Data(), stream.wpos, expectSize);
						totallen += expectSize;
						stream.wpos += (int)expectSize;
						length -= expectSize;
						
						msglen = stream.ReadUint16();
						stream.Clear();
						
						// 长度扩展
						if(msglen >= 65535)
						{
							state = READ_STATE.READ_STATE_MSGLEN_EX;
							expectSize = 4;
						}
						else
						{
							state = READ_STATE.READ_STATE_BODY;
							expectSize = msglen;
						}
					}
					else
					{
						Array.Copy(datas, totallen, stream.Data(), stream.wpos, length);
						stream.wpos += (int)length;
						expectSize -= length;
						break;
					}
				}
				else if(state == READ_STATE.READ_STATE_MSGLEN_EX)
				{
					if(length >= expectSize)
					{
						Array.Copy(datas, totallen, stream.Data(), stream.wpos, expectSize);
						totallen += expectSize;
						stream.wpos += (int)expectSize;
						length -= expectSize;
						
						expectSize = stream.ReadUint32();
						stream.Clear();
						
						state = READ_STATE.READ_STATE_BODY;
					}
					else
					{
						Array.Copy(datas, totallen, stream.Data(), stream.wpos, length);
						stream.wpos += (int)length;
						expectSize -= length;
						break;
					}
				}
				else if(state == READ_STATE.READ_STATE_BODY)
				{
					if(length >= expectSize)
					{
						stream.Append (datas, totallen, expectSize);
						totallen += expectSize;
						length -= expectSize;

						Message msg = Message.clientMessages[msgid];
						
#if UNITY_EDITOR
						Dbg.ProfileStart(msg.name);
#endif

						msg.HandleMessage(stream);
#if UNITY_EDITOR
						Dbg.ProfileEnd(msg.name);
#endif
						
						stream.Clear();
						
						state = READ_STATE.READ_STATE_MSGID;
						expectSize = 2;
					}
					else
					{
						stream.Append (datas, totallen, length);
						expectSize -= length;
						break;
					}
				}
			}
		}
    }
} 
