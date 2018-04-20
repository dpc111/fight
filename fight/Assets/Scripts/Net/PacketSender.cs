namespace Net
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
	
	/*
		包发送模块(与服务端网络部分的名称对应)
		处理网络数据的发送
	*/
    public class PacketSender 
    {
    	public delegate void AsyncSendMethod();
		private byte[] buffer;
		int wpos = 0;				// 写入的数据位置
		int spos = 0;				// 发送完毕的数据位置
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
			Dbg.DEBUG_MSG("PacketSender::~PacketSender(), destroyed!");
		}

		void Init(NetworkInterface netInterface)
		{
            networkInterface = netInterface;
			
			buffer = new byte[NetApp.app.getInitArgs().SEND_BUFFER_MAX];
			asyncSendMethod = new AsyncSendMethod(this._asyncSend);
			asyncCallback = new AsyncCallback(_onSent);
			
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
			int dataLength = (int)stream.length();
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

			int t_spos = Interlocked.Add(ref spos, 0);
			int space = 0;
			int tt_wpos = wpos % buffer.Length;
			int tt_spos = t_spos % buffer.Length;
			
			if(tt_wpos >= tt_spos)
				space = buffer.Length - tt_wpos + tt_spos - 1;
			else
				space = tt_spos - tt_wpos - 1;

			if (dataLength > space)
			{
				Dbg.ERROR_MSG("PacketSender::send(): no space, Please adjust 'SEND_BUFFER_MAX'! data(" + dataLength 
					+ ") > space(" + space + "), wpos=" + wpos + ", spos=" + t_spos);
				
				return false;
			}

			int expect_total = tt_wpos + dataLength;
			if(expect_total <= buffer.Length)
			{
				Array.Copy(stream.data(), stream.rpos, buffer, tt_wpos, dataLength);
			}
			else
			{
				int remain = buffer.Length - tt_wpos;
				Array.Copy(stream.data(), stream.rpos, buffer, tt_wpos, remain);
				Array.Copy(stream.data(), stream.rpos + remain, buffer, 0, expect_total - buffer.Length);
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
			// 由于socket用的是非阻塞式，因此在这里不能直接使用socket.send()方法
			// 必须放到另一个线程中去做
			asyncSendMethod.BeginInvoke(asyncCallback, null);
		}

		void _asyncSend()
		{
			if (networkInterface == null || !networkInterface.Valid())
			{
				Dbg.WARNING_MSG("PacketSender::_asyncSend(): network interface invalid!");
				return;
			}

			var socket = networkInterface.Sock();

			while (true)
			{
				int sendSize = Interlocked.Add(ref wpos, 0) - spos;
				int t_spos = spos % buffer.Length;
				if (t_spos == 0)
					t_spos = sendSize;

				if (sendSize > buffer.Length - t_spos)
					sendSize = buffer.Length - t_spos;

				int bytesSent = 0;
				try
				{
					bytesSent = socket.Send(buffer, spos % buffer.Length, sendSize, 0);
				}
				catch (SocketException se)
				{
					Dbg.ERROR_MSG(string.Format("PacketSender::_asyncSend(): send data error, disconnect from '{0}'! error = '{1}'", socket.RemoteEndPoint, se));
					Event.FireIn("_closeNetwork", new object[] { networkInterface });
					return;
				}

                int spost = Interlocked.Add(ref spos, bytesSent);

				// 所有数据发送完毕了
                if (spost == Interlocked.Add(ref wpos, 0))
				{
					Interlocked.Exchange(ref sending, 0);
					return;
				}
			}
		}
		
		private static void _onSent(IAsyncResult ar)
		{
			AsyncResult result = (AsyncResult)ar;
			AsyncSendMethod caller = (AsyncSendMethod)result.AsyncDelegate;
			caller.EndInvoke(ar);
		}
	}
} 
