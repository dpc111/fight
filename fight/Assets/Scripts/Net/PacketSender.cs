namespace Net_
{
	using System; 
	using System.Net.Sockets; 
	using System.Net; 
	using System.Collections; 
	using System.Collections.Generic;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Runtime.Remoting.Messaging;

	using MessageID = System.UInt16;
	using MessageLength = System.UInt16;
	
    //包发送模块(与服务端网络部分的名称对应)
    //处理网络数据的发送
    public class PacketSender 
    {
    	public delegate void AsyncSendMethod();
		private byte[] buffer;
		int wpos = 0;				                            //写入的数据位置
		int spos = 0;				                            //发送完毕的数据位置
		int sending = 0;
		private NetworkInterface networkInterface = null;
		AsyncCallback asyncCallback = null;
		AsyncSendMethod asyncSendMethod;
		
        public PacketSender(NetworkInterface networkInterface)
        {
        	Init(networkInterface);
        }

		~PacketSender()
		{
			Dbg.DebugMsg("PacketSender::~PacketSender(), destroyed!");
		}

		void Init(NetworkInterface netInterface)
		{
            networkInterface = netInterface;
			buffer = new byte[NetApp.app.GetInitArgs().sendBufferMax];
			asyncSendMethod = new AsyncSendMethod(this.AsyncSend);
			asyncCallback = new AsyncCallback(OnSent);
			wpos = 0; 
			spos = 0;
			sending = 0;
		}

		public NetworkInterface NetworkInterface()
		{
			return networkInterface;
		}

		public bool Send(MemoryStream stream)
		{
			int dataLength = (int)stream.Length();
			if (dataLength <= 0)
				return true;

			if (0 == Interlocked.Add(ref sending, 0))
			{
				if (wpos == spos)
				{
					wpos = 0;
					spos = 0;
				}
			}

			int tspos = Interlocked.Add(ref spos, 0);
			int space = 0;
			int ttwpos = wpos % buffer.Length;
			int ttspos = tspos % buffer.Length;
			
			if(ttwpos >= ttspos)
				space = buffer.Length - ttwpos + ttspos - 1;
			else
				space = ttspos - ttwpos - 1;

			if (dataLength > space)
			{
				Dbg.ErrorMsg("");
				return false;
			}

			int expectTotal = ttwpos + dataLength;
			if(expectTotal <= buffer.Length)
			{
				Array.Copy(stream.Data(), stream.rpos, buffer, ttwpos, dataLength);
			}
			else
			{
				int remain = buffer.Length - ttwpos;
				Array.Copy(stream.Data(), stream.rpos, buffer, ttwpos, remain);
				Array.Copy(stream.Data(), stream.rpos + remain, buffer, 0, expectTotal - buffer.Length);
			}

			Interlocked.Add(ref wpos, dataLength);

			if (Interlocked.CompareExchange(ref sending, 1, 0) == 0)
			{
				StartSend();
			}

			return true;
		}

		void StartSend()
		{
			//由于socket用的是非阻塞式，因此在这里不能直接使用socket.send()方法
			//必须放到另一个线程中去做
			asyncSendMethod.BeginInvoke(asyncCallback, null);
		}

		void AsyncSend()
		{
			if (networkInterface == null || !networkInterface.Valid())
			{
				Dbg.WarningMsg("PacketSender::_asyncSend(): network interface invalid!");
				return;
			}

			var socket = networkInterface.Sock();

			while (true)
			{
				int sendSize = Interlocked.Add(ref wpos, 0) - spos;
				int tspos = spos % buffer.Length;
				if (tspos == 0)
					tspos = sendSize;

				if (sendSize > buffer.Length - tspos)
					sendSize = buffer.Length - tspos;

				int bytesSent = 0;
				try
				{
					bytesSent = socket.Send(buffer, spos % buffer.Length, sendSize, 0);
				}
				catch (SocketException se)
				{
					Dbg.ErrorMsg(string.Format("PacketSender::_asyncSend(): send data error, disconnect from '{0}'! error = '{1}'", socket.RemoteEndPoint, se));
					Event.FireIn("_closeNetwork", new object[] { networkInterface });
					return;
				}

                int spost = Interlocked.Add(ref spos, bytesSent);

				//所有数据发送完毕了
                if (spost == Interlocked.Add(ref wpos, 0))
				{
					Interlocked.Exchange(ref sending, 0);
					return;
				}
			}
		}
		
		private static void OnSent(IAsyncResult ar)
		{
			AsyncResult result = (AsyncResult)ar;
			AsyncSendMethod caller = (AsyncSendMethod)result.AsyncDelegate;
			caller.EndInvoke(ar);
		}
	}
} 
