namespace Net
{
  	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;
	using System.Text;
    using System.Threading;
	using System.Text.RegularExpressions;
	
	using MessageID = System.UInt16;
	using MessageLength = System.UInt16;
	
    //这是Net插件的核心模块
    //包括网络创建、持久化协议、entities的管理、以及引起对外可调用接口。
	public class NetApp
	{
		public static NetApp app = null;
		private NetworkInterface networkInterface = null;
        NetArgs args = null;
    	//客户端的类别
		public enum ClientType
		{
			// Mobile(Phone, Pad)
			ClientTypeMobile				= 1,
			// Windows Application program
			ClientTypeWin					= 2,
			// Linux Application program
			ClientTypeLinux				= 3,
			// Mac Application program
			ClientTypeMac					= 4,
			// Web，HTML5，Flash
			ClientTypeBrowser				= 5,
			// bots
			ClientTypeBots				= 6,
			// Mini-Client
			ClientTypeMini				= 7,
		};
		
        public string userName = "kbengine";
        public string password = "123456";
        // 是否正在加载本地消息协议
        private static bool loadingLocalMessages_ = false;
        // 消息协议是否已经导入了
		private static bool loginappMessageImported_ = false;
		private static bool baseappMessageImported_ = false;
		private static bool entitydefImported_ = false;
		private static bool isImportServerErrorsDescr_ = false;
		// 服务端分配的baseapp地址
		public string baseappIP = "";
		public UInt16 baseappPort = 0;
		// 当前状态
		public string currserver = "";
		public string currstate = "";
		// 服务端下行以及客户端上行用于登录时处理的账号绑定的二进制信息
		// 该信息由用户自己进行扩展
		private byte[] serverDatas = new byte[0];
		private byte[] clientDatas = new byte[0];
		// 通信协议加密，blowfish协议
		private byte[] encryptedKey = new byte[0];
		// 服务端与客户端的版本号以及协议MD5
		public string serverVersion = "";
		public string clientVersion = "1.1.6";
		public string serverScriptVersion = "";
		public string clientScriptVersion = "0.1.0";
		public string serverProtocolMD5 = "";
		public string serverEntitydefMD5 = "";
		// 持久化插件信息， 例如：从服务端导入的协议可以持久化到本地，下次登录版本不发生改变
		// 可以直接从本地加载来提供登录速度
		private PersistentInfos persistentInfos = null;
		// 当前玩家的实体id与实体类别
		public UInt64 entity_uuid = 0;
		public Int32 entity_id = 0;
		public string entity_type = "";
		private List<Entity> _controlledEntities = new List<Entity>();
		// 当前服务端最后一次同步过来的玩家位置
		private Vector3 _entityServerPos = new Vector3(0f, 0f, 0f);
		// space的数据，具体看API手册关于spaceData
		// https://github.com/kbengine/kbengine/tree/master/docs/api
		private Dictionary<string, string> _spacedatas = new Dictionary<string, string>();
		// 所有实体都保存于这里， 请参看API手册关于entities部分
		// https://github.com/kbengine/kbengine/tree/master/docs/api
		public Dictionary<Int32, Entity> entities = new Dictionary<Int32, Entity>();
		// 在玩家View范围小于256个实体时我们可以通过一字节索引来找到entity
		private List<Int32> _entityIDAliasIDList = new List<Int32>();
		private Dictionary<Int32, MemoryStream> _bufferedCreateEntityMessage = new Dictionary<Int32, MemoryStream>(); 
		// 描述服务端返回的错误信息
		public struct ServerErr
		{
			public string name;
			public string descr;
			public UInt16 id;
		}
		
		// 所有服务端错误码对应的错误描述
		public static Dictionary<UInt16, ServerErr> serverErrs = new Dictionary<UInt16, ServerErr>(); 
		
		private System.DateTime _lastTickTime = System.DateTime.Now;
		private System.DateTime _lastTickCBTime = System.DateTime.Now;
		private System.DateTime _lastUpdateToServerTime = System.DateTime.Now;
		
		// 玩家当前所在空间的id， 以及空间对应的资源
		public UInt32 spaceID = 0;
		public string spaceResPath = "";
		public bool isLoadedGeometry = false;
		
		// entityDef管理模块
		public static EntityDef entityDef = new EntityDef();
		
		// 按照标准，每个客户端部分都应该包含这个属性
		public const string component = "client"; 
		
        public NetApp(NetArgs args)
        {
			if (app != null)
				throw new Exception("Only one instance of KBEngineApp!");
			app = this;
			Initialize(args);
        }

		public virtual bool Initialize(NetArgs a)
		{
            args = a;
        	InitNetwork();
            //注册事件
            InstallEvents();
            //允许持久化
            if(args.persistentDataPath != "")
         	   persistentInfos = new PersistentInfos(args.persistentDataPath);
         	return true;
		}
		
		void InitNetwork()
		{
			Message.BindFixedMessage();
        	networkInterface = new NetworkInterface();
		}
		
		void InstallEvents()
		{
			Event.RegisterIn("createAccount", this, "createAccount");
			Event.RegisterIn("login", this, "login");
			Event.RegisterIn("reloginBaseapp", this, "reloginBaseapp");
			Event.RegisterIn("resetPassword", this, "resetPassword");
			Event.RegisterIn("bindAccountEmail", this, "bindAccountEmail");
			Event.RegisterIn("newPassword", this, "newPassword");
			// 内部事件
			Event.RegisterIn("_closeNetwork", this, "_closeNetwork");
		}

		public NetArgs GetInitArgs()
		{
			return args;
		}
		
        public virtual void Destroy()
        {
        	Dbg.WarningMsg("KBEngine::destroy()");
        	Reset();
        	Net.Event.DeregisterIn(this);
        	ResetMessages();
        	NetApp.app = null;
        }
        
        public NetworkInterface NetworkInterface()
        {
        	return networkInterface;
        }
        
        public byte[] ServerDatas()
        {
        	return serverDatas;
        }
        
        public void EntityServerPos(Vector3 pos)
        {
        	_entityServerPos = pos;
        }
        
        public void ResetMessages()
        {
	        loadingLocalMessages_ = false;
			loginappMessageImported_ = false;
			baseappMessageImported_ = false;
			entitydefImported_ = false;
			isImportServerErrorsDescr_ = false;
			serverErrs.Clear();
			Message.Clear();
			EntityDef.clear();
			Entity.clear();
			Dbg.DebugMsg("KBEngine::resetMessages()");
        }
        
		public virtual void Reset()
		{
			Net.Event.ClearFiredEvents();
			clearEntities(true);
			currserver = "";
			currstate = "";
			serverDatas = new byte[0];
			serverVersion = "";
			serverScriptVersion = "";
			entity_uuid = 0;
			entity_id = 0;
			entity_type = "";
			_entityIDAliasIDList.Clear();
			_bufferedCreateEntityMessage.Clear();
			_lastTickTime = System.DateTime.Now;
			_lastTickCBTime = System.DateTime.Now;
			_lastUpdateToServerTime = System.DateTime.Now;
			spaceID = 0;
			spaceResPath = "";
			isLoadedGeometry = false;
			if (networkInterface != null)
				networkInterface.Reset();
			networkInterface = new NetworkInterface();
			_spacedatas.Clear();
		}
		
		public static bool ValidEmail(string strEmail) 
		{ 
			return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"); 
		}  
		
		//插件的主循环处理函数
		public virtual void Process()
		{
			//处理网络
			if (networkInterface != null)
				networkInterface.Process();
			//处理外层抛入的事件
			Event.ProcessInEvents();
			//向服务端发送心跳以及同步角色信息到服务端
			SendTick();
		}
		
		//当前玩家entity
		public Entity Player()
		{
			Entity e;
			if(entities.TryGetValue(entity_id, out e))
				return e;
			
			return null;
		}

		public void _closeNetwork(NetworkInterface networkInterface)
		{
			networkInterface.Close();
		}
		
		//向服务端发送心跳以及同步角色信息到服务端
		public void SendTick()
		{
			if(networkInterface == null || networkInterface.connected == false)
				return;

			if(!loginappMessageImported_ && !baseappMessageImported_)
				return;
			
			TimeSpan span = DateTime.Now - _lastTickTime;
			
			// 更新玩家的位置与朝向到服务端
			updatePlayerToServer();
			
			if(span.Seconds > args.serverHeartbeatTick)
			{
				span = _lastTickCBTime - _lastTickTime;
				
				// 如果心跳回调接收时间小于心跳发送时间，说明没有收到回调
				// 此时应该通知客户端掉线了
				if(span.Seconds < 0)
				{
					Dbg.ErrorMsg("sendTick: Receive appTick timeout!");
					networkInterface.Close();
					return;
				}

				Message Loginapp_onClientActiveTickMsg = null;
				Message Baseapp_onClientActiveTickMsg = null;
				
				Message.messages.TryGetValue("Loginapp_onClientActiveTick", out Loginapp_onClientActiveTickMsg);
				Message.messages.TryGetValue("Baseapp_onClientActiveTick", out Baseapp_onClientActiveTickMsg);
				
				if(currserver == "loginapp")
				{
					if(Loginapp_onClientActiveTickMsg != null)
					{
						Bundle bundle = Bundle.CreateObject();
						bundle.NewMessage(Message.messages["Loginapp_onClientActiveTick"]);
						bundle.send(networkInterface);
					}
				}
				else
				{
					if(Baseapp_onClientActiveTickMsg != null)
					{
						Bundle bundle = Bundle.CreateObject();
						bundle.NewMessage(Message.messages["Baseapp_onClientActiveTick"]);
						bundle.send(networkInterface);
					}
				}
				
				_lastTickTime = System.DateTime.Now;
			}
		}

		/*
			服务器心跳回调
		*/
		public void Client_onAppActiveTickCB()
		{
			_lastTickCBTime = System.DateTime.Now;
		}

		/*
			与服务端握手，与任何一个进程连接之后应该第一时间进行握手
		*/
		public void hello()
		{
			Bundle bundle = Bundle.CreateObject();
			if(currserver == "loginapp")
				bundle.NewMessage(Message.messages["Loginapp_hello"]);
			else
				bundle.NewMessage(Message.messages["Baseapp_hello"]);
			
			bundle.WriteString(clientVersion);
			bundle.WriteString(clientScriptVersion);
			bundle.WriteBlob(encryptedKey);
			bundle.send(networkInterface);
		}

		/*
			握手之后服务端的回调
		*/
		public void Client_onHelloCB(MemoryStream stream)
		{
			serverVersion = stream.ReadString();
			serverScriptVersion = stream.ReadString();
			serverProtocolMD5 = stream.ReadString();
			serverEntitydefMD5 = stream.ReadString();
			Int32 ctype = stream.ReadInt32();
			
			Dbg.DebugMsg("KBEngine::Client_onHelloCB: verInfo(" + serverVersion 
				+ "), scriptVersion("+ serverScriptVersion + "), srvProtocolMD5("+ serverProtocolMD5 
				+ "), srvEntitydefMD5("+ serverEntitydefMD5 + "), + ctype(" + ctype + ")!");
			
			onServerDigest();
			
			if(currserver == "baseapp")
			{
				onLogin_baseapp();
			}
			else
			{
				onLogin_loginapp();
			}
		}
		
		/*
			引擎版本不匹配
		*/
		public void Client_onVersionNotMatch(MemoryStream stream)
		{
			serverVersion = stream.ReadString();
			
			Dbg.ErrorMsg("Client_onVersionNotMatch: verInfo=" + clientVersion + "(server: " + serverVersion + ")");
			Event.FireAll("onVersionNotMatch", new object[]{clientVersion, serverVersion});
			
			if(persistentInfos != null)
				persistentInfos.onVersionNotMatch(clientVersion, serverVersion);
		}

		/*
			脚本版本不匹配
		*/
		public void Client_onScriptVersionNotMatch(MemoryStream stream)
		{
			serverScriptVersion = stream.ReadString();
			
			Dbg.ErrorMsg("Client_onScriptVersionNotMatch: verInfo=" + clientScriptVersion + "(server: " + serverScriptVersion + ")");
			Event.FireAll("onScriptVersionNotMatch", new object[]{clientScriptVersion, serverScriptVersion});
			
			if(persistentInfos != null)
				persistentInfos.onScriptVersionNotMatch(clientScriptVersion, serverScriptVersion);
		}
		
		/*
			被服务端踢出
		*/
		public void Client_onKicked(UInt16 failedcode)
		{
			Dbg.DebugMsg("Client_onKicked: failedcode=" + failedcode);
			Event.FireAll("onKicked", new object[]{failedcode});
		}
		
		/*
			服务端错误描述导入了
		*/
		public void Client_onImportServerErrorsDescr(MemoryStream stream)
		{
			byte[] datas = new byte[stream.wpos - stream.rpos];
			Array.Copy(stream.Data(), stream.rpos, datas, 0, stream.wpos - stream.rpos);
			
			onImportServerErrorsDescr (stream);
			
			if(persistentInfos != null)
				persistentInfos.onImportServerErrorsDescr(datas);
		}

		/*
			服务端错误描述导入了
		*/
		public void onImportServerErrorsDescr(MemoryStream stream)
		{
			UInt16 size = stream.ReadUint16();
			while(size > 0)
			{
				size -= 1;
				
				ServerErr e;
				e.id = stream.ReadUint16();
				e.name = System.Text.Encoding.UTF8.GetString(stream.ReadBlob());
				e.descr = System.Text.Encoding.UTF8.GetString(stream.ReadBlob());
				
				serverErrs.Add(e.id, e);
					
				//Dbg.DEBUG_MSG("Client_onImportServerErrorsDescr: id=" + e.id + ", name=" + e.name + ", descr=" + e.descr);
			}
		}
		
		/*
			登录到服务端，必须登录完成loginapp与网关(baseapp)，登录流程才算完毕
		*/
		public void login(string username, string password, byte[] datas)
		{
			NetApp.app.userName = username;
			NetApp.app.password = password;
			NetApp.app.clientDatas = datas;
			
			NetApp.app.login_loginapp(true);
		}
		
		/*
			登录到服务端(loginapp), 登录成功后还必须登录到网关(baseapp)登录流程才算完毕
		*/
		public void login_loginapp(bool noconnect)
		{
			if(noconnect)
			{
				Reset();
				networkInterface.ConnectTo(args.ip, args.port, onConnectTo_loginapp_callback, null);
			}
			else
			{
				Dbg.DebugMsg("KBEngine::login_loginapp(): send login! username=" + userName);
				Bundle bundle = Bundle.CreateObject();
				bundle.NewMessage(Message.messages["Loginapp_login"]);
				bundle.WriteInt8((sbyte)args.clientType);
				bundle.WriteBlob(NetApp.app.clientDatas);
				bundle.WriteString(userName);
				bundle.WriteString(password);
				bundle.send(networkInterface);
			}
		}
		
		private void onConnectTo_loginapp_callback(string ip, int port, bool success, object userData)
		{
			_lastTickCBTime = System.DateTime.Now;
			
			if(!success)
			{
				Dbg.ErrorMsg(string.Format("KBEngine::login_loginapp(): connect {0}:{1} is error!", ip, port));  
				return;
			}
			
			currserver = "loginapp";
			currstate = "login";
			
			Dbg.DebugMsg(string.Format("KBEngine::login_loginapp(): connect {0}:{1} is success!", ip, port));

			hello();
		}
		
		private void onLogin_loginapp()
		{
			_lastTickCBTime = System.DateTime.Now;
			
			if(!loginappMessageImported_)
			{
				var bundle = Bundle.CreateObject();
				bundle.NewMessage(Message.messages["Loginapp_importClientMessages"]);
				bundle.send(networkInterface);
				Dbg.DebugMsg("KBEngine::onLogin_loginapp: send importClientMessages ...");
				Event.FireOut("Loginapp_importClientMessages", new object[]{});
			}
			else
			{
				onImportClientMessagesCompleted();
			}
		}
		
		/*
			登录到服务端，登录到网关(baseapp)
		*/
		public void login_baseapp(bool noconnect)
		{  
			if(noconnect)
			{
				Event.FireOut("onLoginBaseapp", new object[]{});
				
				networkInterface.Reset();
				networkInterface = new NetworkInterface();
				networkInterface.ConnectTo(baseappIP, baseappPort, onConnectTo_baseapp_callback, null);
			}
			else
			{
				Bundle bundle = Bundle.CreateObject();
				bundle.NewMessage(Message.messages["Baseapp_loginBaseapp"]);
				bundle.WriteString(userName);
				bundle.WriteString(password);
				bundle.send(networkInterface);
			}
		}

		private void onConnectTo_baseapp_callback(string ip, int port, bool success, object userData)
		{
			_lastTickCBTime = System.DateTime.Now;
			
			if(!success)
			{
				Dbg.ErrorMsg(string.Format("KBEngine::login_baseapp(): connect {0}:{1} is error!", ip, port));
				return;
			}
			
			currserver = "baseapp";
			currstate = "";
			
			Dbg.DebugMsg(string.Format("KBEngine::login_baseapp(): connect {0}:{1} is successfully!", ip, port));

			hello();
		}
		
		private void onLogin_baseapp()
		{
			_lastTickCBTime = System.DateTime.Now;
			
			if(!baseappMessageImported_)
			{
				var bundle = Bundle.CreateObject();
				bundle.NewMessage(Message.messages["Baseapp_importClientMessages"]);
				bundle.send(networkInterface);
				Dbg.DebugMsg("KBEngine::onLogin_baseapp: send importClientMessages ...");
				Event.FireOut("Baseapp_importClientMessages", new object[]{});
			}
			else
			{
				onImportClientMessagesCompleted();
			}
		}
		
		/*
			重登录到网关(baseapp)
			一些移动类应用容易掉线，可以使用该功能快速的重新与服务端建立通信
		*/
		public void reloginBaseapp()
		{  
			if(networkInterface.Valid())
				return;

			Event.FireAll("onReloginBaseapp", new object[]{});
			networkInterface.ConnectTo(baseappIP, baseappPort, onReConnectTo_baseapp_callback, null);
		}

		private void onReConnectTo_baseapp_callback(string ip, int port, bool success, object userData)
		{
			if(!success)
			{
				Dbg.ErrorMsg(string.Format("KBEngine::reloginBaseapp(): connect {0}:{1} is error!", ip, port));
				return;
			}
			
			
			Dbg.DebugMsg(string.Format("KBEngine::relogin_baseapp(): connect {0}:{1} is successfully!", ip, port));

			Bundle bundle = Bundle.CreateObject();
			bundle.NewMessage(Message.messages["Baseapp_reloginBaseapp"]);
			bundle.WriteString(userName);
			bundle.WriteString(password);
			bundle.WriteUint64(entity_uuid);
			bundle.WriteInt32(entity_id);
			bundle.send(networkInterface);
			
			_lastTickCBTime = System.DateTime.Now;
		}
		
		/*
			从二进制流导入消息协议
		*/
		public bool importMessagesFromMemoryStream(byte[] loginapp_clientMessages, byte[] baseapp_clientMessages, byte[] entitydefMessages, byte[] serverErrorsDescr)
		{
			ResetMessages();
			
			loadingLocalMessages_ = true;
			MemoryStream stream = MemoryStream.CreateObject();
			stream.Append(loginapp_clientMessages, (UInt32)0, (UInt32)loginapp_clientMessages.Length);
			currserver = "loginapp";
			onImportClientMessages(stream);
			stream.ReclaimObject();

			stream = MemoryStream.CreateObject();
			stream.Append(baseapp_clientMessages, (UInt32)0, (UInt32)baseapp_clientMessages.Length);
			currserver = "baseapp";
			onImportClientMessages(stream);
			currserver = "loginapp";
			stream.ReclaimObject();

			stream = MemoryStream.CreateObject();
			stream.Append(serverErrorsDescr, (UInt32)0, (UInt32)serverErrorsDescr.Length);
			onImportServerErrorsDescr(stream);
			stream.ReclaimObject();

			stream = MemoryStream.CreateObject();
			stream.Append(entitydefMessages, (UInt32)0, (UInt32)entitydefMessages.Length);
			onImportClientEntityDef(stream);
			stream.ReclaimObject();

			loadingLocalMessages_ = false;
			loginappMessageImported_ = true;
			baseappMessageImported_ = true;
			entitydefImported_ = true;
			isImportServerErrorsDescr_ = true;
		
			currserver = "";
			Dbg.DebugMsg("KBEngine::importMessagesFromMemoryStream(): is successfully!");
			return true;
		}

		/*
			从二进制流导入消息协议完毕了
		*/
		private void onImportClientMessagesCompleted()
		{
			Dbg.DebugMsg("KBEngine::onImportClientMessagesCompleted: successfully! currserver=" + 
				currserver + ", currstate=" + currstate);

			if(currserver == "loginapp")
			{
				if(!isImportServerErrorsDescr_ && !loadingLocalMessages_)
				{
					Dbg.DebugMsg("KBEngine::onImportClientMessagesCompleted(): send importServerErrorsDescr!");
					isImportServerErrorsDescr_ = true;
					Bundle bundle = Bundle.CreateObject();
					bundle.NewMessage(Message.messages["Loginapp_importServerErrorsDescr"]);
					bundle.send(networkInterface);
				}
				
				if(currstate == "login")
				{
					login_loginapp(false);
				}
				else if(currstate == "autoimport")
				{
				}
				else if(currstate == "resetpassword")
				{
					resetpassword_loginapp(false);
				}
				else if(currstate == "createAccount")
				{
					createAccount_loginapp(false);
				}
				else{
				}

				loginappMessageImported_ = true;
			}
			else
			{
				baseappMessageImported_ = true;
				
				if(!entitydefImported_ && !loadingLocalMessages_)
				{
					Dbg.DebugMsg("KBEngine::onImportClientMessagesCompleted: send importEntityDef(" + entitydefImported_ + ") ...");
					Bundle bundle = Bundle.CreateObject();
					bundle.NewMessage(Message.messages["Baseapp_importClientEntityDef"]);
					bundle.send(networkInterface);
					Event.FireOut("Baseapp_importClientEntityDef", new object[]{});
				}
				else
				{
					onImportEntityDefCompleted();
				}
			}
		}
		
		/*
			从二进制流创建entitydef支持的数据类型
		*/
		public void createDataTypeFromStreams(MemoryStream stream, bool canprint)
		{
			UInt16 aliassize = stream.ReadUint16();
			Dbg.DebugMsg("KBEngine::createDataTypeFromStreams: importAlias(size=" + aliassize + ")!");
			
			while(aliassize > 0)
			{
				aliassize--;
				createDataTypeFromStream(stream, canprint);
			};
		
			foreach(string datatype in EntityDef.datatypes.Keys)
			{
				if(EntityDef.datatypes[datatype] != null)
				{
					EntityDef.datatypes[datatype].bind();
				}
			}			
		}
			
		public void createDataTypeFromStream(MemoryStream stream, bool canprint)
		{
			UInt16 utype = stream.ReadUint16();
			string name = stream.ReadString();
			string valname = stream.ReadString();

			/* 有一些匿名类型，我们需要提供一个唯一名称放到datatypes中
				如：
				<onRemoveAvatar>
					<Arg>	ARRAY <of> INT8 </of>		</Arg>
				</onRemoveAvatar>				
			*/
			if(valname.Length == 0)
				valname = "Null_" + utype;
			
			if(canprint)
				Dbg.DebugMsg("KBEngine::Client_onImportClientEntityDef: importAlias(" + name + ":" + valname + ":" + utype + ")!");
			
			if(name == "FIXED_DICT")
			{
				KBEDATATYPE_FIXED_DICT datatype = new KBEDATATYPE_FIXED_DICT();
				Byte keysize = stream.ReadUint8();
				datatype.implementedBy = stream.ReadString();
					
				while(keysize > 0)
				{
					keysize--;
					
					string keyname = stream.ReadString();
					UInt16 keyutype = stream.ReadUint16();
					datatype.dicttype[keyname] = keyutype;
				};
				
				EntityDef.datatypes[valname] = datatype;
			}
			else if(name == "ARRAY")
			{
				UInt16 uitemtype = stream.ReadUint16();
				KBEDATATYPE_ARRAY datatype = new KBEDATATYPE_ARRAY();
				datatype.vtype = uitemtype;
				EntityDef.datatypes[valname] = datatype;
			}
			else
			{
				KBEDATATYPE_BASE val = null;
				EntityDef.datatypes.TryGetValue(name, out val);
				EntityDef.datatypes[valname] = val;
			}
	
			EntityDef.id2datatypes[utype] = EntityDef.datatypes[valname];
			
			// 将用户自定义的类型补充到映射表中
			EntityDef.datatype2id[valname] = utype;
		}

		public void Client_onImportClientEntityDef(MemoryStream stream)
		{
			byte[] datas = new byte[stream.wpos - stream.rpos];
			Array.Copy (stream.Data (), stream.rpos, datas, 0, stream.wpos - stream.rpos);

			onImportClientEntityDef (stream);
			
			if(persistentInfos != null)
				persistentInfos.onImportClientEntityDef(datas);
		}

		public void onImportClientEntityDef(MemoryStream stream)
		{
			createDataTypeFromStreams(stream, true);

			
			while(stream.Length() > 0)
			{
				string scriptmodule_name = stream.ReadString();
				UInt16 scriptUtype = stream.ReadUint16();
				UInt16 propertysize = stream.ReadUint16();
				UInt16 methodsize = stream.ReadUint16();
				UInt16 base_methodsize = stream.ReadUint16();
				UInt16 cell_methodsize = stream.ReadUint16();
				
				Dbg.DebugMsg("KBEngine::Client_onImportClientEntityDef: import(" + scriptmodule_name + "), propertys(" + propertysize + "), " +
						"clientMethods(" + methodsize + "), baseMethods(" + base_methodsize + "), cellMethods(" + cell_methodsize + ")!");
				
				
				ScriptModule module = new ScriptModule(scriptmodule_name);
				EntityDef.moduledefs[scriptmodule_name] = module;
				EntityDef.idmoduledefs[scriptUtype] = module;

				Type Class = module.script;
				
				while(propertysize > 0)
				{
					propertysize--;
					
					UInt16 properUtype = stream.ReadUint16();
					UInt32 properFlags = stream.ReadUint32();
					Int16 ialiasID = stream.ReadInt16();
					string name = stream.ReadString();
					string defaultValStr = stream.ReadString();
					KBEDATATYPE_BASE utype = EntityDef.id2datatypes[stream.ReadUint16()];
					
					System.Reflection.MethodInfo setmethod = null;
					
					if(Class != null)
					{
						try{
							setmethod = Class.GetMethod("set_" + name);
						}
						catch (Exception e)
						{
							string err = "KBEngine::Client_onImportClientEntityDef: " + 
								scriptmodule_name + ".set_" + name + ", error=" + e.ToString();
							
							throw new Exception(err);
						}
					}
					
					Property savedata = new Property();
					savedata.name = name;
					savedata.utype = utype;
					savedata.properUtype = properUtype;
					savedata.properFlags = properFlags;
					savedata.aliasID = ialiasID;
					savedata.defaultValStr = defaultValStr;
					savedata.setmethod = setmethod;
					savedata.val = savedata.utype.parseDefaultValStr(savedata.defaultValStr);
					
					module.propertys[name] = savedata;
					
					if(ialiasID != -1)
					{
						module.usePropertyDescrAlias = true;
						module.idpropertys[(UInt16)ialiasID] = savedata;
					}
					else
					{
						module.usePropertyDescrAlias = false;
						module.idpropertys[properUtype] = savedata;
					}

					//Dbg.DEBUG_MSG("KBEngine::Client_onImportClientEntityDef: add(" + scriptmodule_name + "), property(" + name + "/" + properUtype + ").");
				};
				
				while(methodsize > 0)
				{
					methodsize--;
					
					UInt16 methodUtype = stream.ReadUint16();
					Int16 ialiasID = stream.ReadInt16();
					string name = stream.ReadString();
					Byte argssize = stream.ReadUint8();
					List<KBEDATATYPE_BASE> args = new List<KBEDATATYPE_BASE>();
					
					while(argssize > 0)
					{
						argssize--;
						args.Add(EntityDef.id2datatypes[stream.ReadUint16()]);
					};
					
					Method savedata = new Method();
					savedata.name = name;
					savedata.methodUtype = methodUtype;
					savedata.aliasID = ialiasID;
					savedata.args = args;
					
					if(Class != null)
					{
						try{
							savedata.handler = Class.GetMethod(name);
						}
						catch (Exception e)
						{
							string err = "KBEngine::Client_onImportClientEntityDef: " + scriptmodule_name + "." + name + ", error=" + e.ToString();
							throw new Exception(err);
						}
					}
							
					module.methods[name] = savedata;
					
					if(ialiasID != -1)
					{
						module.useMethodDescrAlias = true;
						module.idmethods[(UInt16)ialiasID] = savedata;
					}
					else
					{
						module.useMethodDescrAlias = false;
						module.idmethods[methodUtype] = savedata;
					}
					
					//Dbg.DEBUG_MSG("KBEngine::Client_onImportClientEntityDef: add(" + scriptmodule_name + "), method(" + name + ").");
				};
	
				while(base_methodsize > 0)
				{
					base_methodsize--;
					
					UInt16 methodUtype = stream.ReadUint16();
					Int16 ialiasID = stream.ReadInt16();
					string name = stream.ReadString();
					Byte argssize = stream.ReadUint8();
					List<KBEDATATYPE_BASE> args = new List<KBEDATATYPE_BASE>();
					
					while(argssize > 0)
					{
						argssize--;
						args.Add(EntityDef.id2datatypes[stream.ReadUint16()]);
					};
					
					Method savedata = new Method();
					savedata.name = name;
					savedata.methodUtype = methodUtype;
					savedata.aliasID = ialiasID;
					savedata.args = args;
					
					module.base_methods[name] = savedata;
					module.idbase_methods[methodUtype] = savedata;
					
					//Dbg.DEBUG_MSG("KBEngine::Client_onImportClientEntityDef: add(" + scriptmodule_name + "), base_method(" + name + ").");
				};
				
				while(cell_methodsize > 0)
				{
					cell_methodsize--;
					
					UInt16 methodUtype = stream.ReadUint16();
					Int16 ialiasID = stream.ReadInt16();
					string name = stream.ReadString();
					Byte argssize = stream.ReadUint8();
					List<KBEDATATYPE_BASE> args = new List<KBEDATATYPE_BASE>();
					
					while(argssize > 0)
					{
						argssize--;
						args.Add(EntityDef.id2datatypes[stream.ReadUint16()]);
					};
					
					Method savedata = new Method();
					savedata.name = name;
					savedata.methodUtype = methodUtype;
					savedata.aliasID = ialiasID;
					savedata.args = args;
				
					module.cell_methods[name] = savedata;
					module.idcell_methods[methodUtype] = savedata;
					//Dbg.DEBUG_MSG("KBEngine::Client_onImportClientEntityDef: add(" + scriptmodule_name + "), cell_method(" + name + ").");
				};
				
				if(module.script == null)
				{
					Dbg.ErrorMsg("KBEngine::Client_onImportClientEntityDef: module(" + scriptmodule_name + ") not found!");
				}

				foreach(string name in module.methods.Keys)
				{
					// Method infos = module.methods[name];

					if(module.script != null && module.script.GetMethod(name) == null)
					{
						Dbg.WarningMsg(scriptmodule_name + "(" + module.script + "):: method(" + name + ") no implement!");
					}
				};
			}
			
			onImportEntityDefCompleted();
		}
		
		private void onImportEntityDefCompleted()
		{
			Dbg.DebugMsg("KBEngine::onImportEntityDefCompleted: successfully!");
			entitydefImported_ = true;
			
			if(!loadingLocalMessages_)
				login_baseapp(false);
		}

		/*
			通过错误id得到错误描述
		*/
		public string serverErr(UInt16 id)
		{
			ServerErr e;
			
			if(!serverErrs.TryGetValue(id, out e))
			{
				return "";
			}

			return e.name + " [" + e.descr + "]";
		}
	
		/*
			从服务端返回的二进制流导入客户端消息协议
		*/
		public void Client_onImportClientMessages(MemoryStream stream)
		{
			byte[] datas = new byte[stream.wpos - stream.rpos];
			Array.Copy (stream.Data (), stream.rpos, datas, 0, stream.wpos - stream.rpos);

			onImportClientMessages (stream);
			
			if(persistentInfos != null)
				persistentInfos.onImportClientMessages(currserver, datas);
		}

		public void onImportClientMessages(MemoryStream stream)
		{
			UInt16 msgcount = stream.ReadUint16();
			
			Dbg.DebugMsg(string.Format("KBEngine::Client_onImportClientMessages: start currserver=" + currserver + "(msgsize={0})...", msgcount));
			
			while(msgcount > 0)
			{
				msgcount--;
				
				MessageID msgid = stream.ReadUint16();
				Int16 msglen = stream.ReadInt16();
				
				string msgname = stream.ReadString();
				sbyte argstype = stream.ReadInt8();
				Byte argsize = stream.ReadUint8();
				List<Byte> argstypes = new List<Byte>();
				
				for(Byte i=0; i<argsize; i++)
				{
					argstypes.Add(stream.ReadUint8());
				}
				
				System.Reflection.MethodInfo handler = null;
				bool isClientMethod = msgname.Contains("Client_");
				
				if(isClientMethod)
				{
					handler = typeof(NetApp).GetMethod(msgname);
					if(handler == null)
					{
						Dbg.WarningMsg(string.Format("KBEngine::onImportClientMessages[{0}]: interface({1}/{2}/{3}) no implement!", 
							currserver, msgname, msgid, msglen));
						
						handler = null;
					}
					else
					{
						//Dbg.DEBUG_MSG(string.Format("KBEngine::onImportClientMessages: imported({0}/{1}/{2}) successfully!", 
						//	msgname, msgid, msglen));
					}
				}
				
				if(msgname.Length > 0)
				{
					Message.messages[msgname] = new Message(msgid, msgname, msglen, argstype, argstypes, handler);
					
					//if(!isClientMethod)
					//	Dbg.DEBUG_MSG(string.Format("KBEngine::onImportClientMessages[{0}]: imported({1}/{2}/{3}) successfully!", 
					//		currserver, msgname, msgid, msglen));
					
					if(isClientMethod)
					{
						Message.clientMessages[msgid] = Message.messages[msgname];
					}
					else
					{
						if(currserver == "loginapp")
							Message.loginappMessages[msgid] = Message.messages[msgname];
						else
							Message.baseappMessages[msgid] = Message.messages[msgname];
					}
				}
				else
				{
					Message msg = new Message(msgid, msgname, msglen, argstype, argstypes, handler);
					
					//if(!isClientMethod)
					//	Dbg.DEBUG_MSG(string.Format("KBEngine::onImportClientMessages[{0}]: imported({1}/{2}/{3}) successfully!", 
					//		currserver, msgname, msgid, msglen));
					
					if(currserver == "loginapp")
						Message.loginappMessages[msgid] = msg;
					else
						Message.baseappMessages[msgid] = msg;
				}
			};

			onImportClientMessagesCompleted();
		}
		
		public void onOpenLoginapp_resetpassword()
		{  
			Dbg.DebugMsg("KBEngine::onOpenLoginapp_resetpassword: successfully!");
			currserver = "loginapp";
			currstate = "resetpassword";
			_lastTickCBTime = System.DateTime.Now;

			if(!loginappMessageImported_)
			{
				Bundle bundle = Bundle.CreateObject();
				bundle.NewMessage(Message.messages["Loginapp_importClientMessages"]);
				bundle.send(networkInterface);
				Dbg.DebugMsg("KBEngine::onOpenLoginapp_resetpassword: send importClientMessages ...");
			}
			else
			{
				onImportClientMessagesCompleted();
			}
		}

		/*
			重置密码, 通过loginapp
		*/
		public void resetPassword(string username)
		{
			NetApp.app.userName = username;
			resetpassword_loginapp(true);
		}
		
		/*
			重置密码, 通过loginapp
		*/
		public void resetpassword_loginapp(bool noconnect)
		{
			if(noconnect)
			{
				Reset();
				networkInterface.ConnectTo(args.ip, args.port, onConnectTo_resetpassword_callback, null);
			}
			else
			{
				Bundle bundle = Bundle.CreateObject();
				bundle.NewMessage(Message.messages["Loginapp_reqAccountResetPassword"]);
				bundle.WriteString(userName);
				bundle.send(networkInterface);
			}
		}

		private void onConnectTo_resetpassword_callback(string ip, int port, bool success, object userData)
		{
			_lastTickCBTime = System.DateTime.Now;
			
			if(!success)
			{
				Dbg.ErrorMsg(string.Format("KBEngine::resetpassword_loginapp(): connect {0}:{1} is error!", ip, port));
				return;
			}
			
			Dbg.DebugMsg(string.Format("KBEngine::resetpassword_loginapp(): connect {0}:{1} is success!", ip, port)); 
			onOpenLoginapp_resetpassword();
		}
		
		public void Client_onReqAccountResetPasswordCB(UInt16 failcode)
		{
			if(failcode != 0)
			{
				Dbg.ErrorMsg("KBEngine::Client_onReqAccountResetPasswordCB: " + userName + " is failed! code=" + failcode + "!");
				return;
			}
	
			Dbg.DebugMsg("KBEngine::Client_onReqAccountResetPasswordCB: " + userName + " is successfully!");
		}
		
		/*
			绑定Email，通过baseapp
		*/
		public void bindAccountEmail(string emailAddress)
		{
			Bundle bundle = Bundle.CreateObject();
			bundle.NewMessage(Message.messages["Baseapp_reqAccountBindEmail"]);
			bundle.WriteInt32(entity_id);
			bundle.WriteString(password);
			bundle.WriteString(emailAddress);
			bundle.send(networkInterface);
		}

		public void Client_onReqAccountBindEmailCB(UInt16 failcode)
		{
			if(failcode != 0)
			{
				Dbg.ErrorMsg("KBEngine::Client_onReqAccountBindEmailCB: " + userName + " is failed! code=" + failcode + "!");
				return;
			}

			Dbg.DebugMsg("KBEngine::Client_onReqAccountBindEmailCB: " + userName + " is successfully!");
		}
		
		/*
			设置新密码，通过baseapp， 必须玩家登录在线操作所以是baseapp。
		*/
		public void newPassword(string old_password, string new_password)
		{
			Bundle bundle = Bundle.CreateObject();
			bundle.NewMessage(Message.messages["Baseapp_reqAccountNewPassword"]);
			bundle.WriteInt32(entity_id);
			bundle.WriteString(old_password);
			bundle.WriteString(new_password);
			bundle.send(networkInterface);
		}

		public void Client_onReqAccountNewPasswordCB(UInt16 failcode)
		{
			if(failcode != 0)
			{
				Dbg.ErrorMsg("KBEngine::Client_onReqAccountNewPasswordCB: " + userName + " is failed! code=" + failcode + "!");
				return;
			}
	
			Dbg.DebugMsg("KBEngine::Client_onReqAccountNewPasswordCB: " + userName + " is successfully!");
		}

		public void createAccount(string username, string password, byte[] datas)
		{
			NetApp.app.userName = username;
			NetApp.app.password = password;
			NetApp.app.clientDatas = datas;
			
			NetApp.app.createAccount_loginapp(true);
		}

		/*
			创建账号，通过loginapp
		*/
		public void createAccount_loginapp(bool noconnect)
		{
			if(noconnect)
			{
				Reset();
				networkInterface.ConnectTo(args.ip, args.port, onConnectTo_createAccount_callback, null);
			}
			else
			{
				Bundle bundle = Bundle.CreateObject();
				bundle.NewMessage(Message.messages["Loginapp_reqCreateAccount"]);
				bundle.WriteString(userName);
				bundle.WriteString(password);
				bundle.WriteBlob(NetApp.app.clientDatas);
				bundle.send(networkInterface);
			}
		}

		public void onOpenLoginapp_createAccount()
		{  
			Dbg.DebugMsg("KBEngine::onOpenLoginapp_createAccount: successfully!");
			currserver = "loginapp";
			currstate = "createAccount";
			_lastTickCBTime = System.DateTime.Now;
			
			if(!loginappMessageImported_)
			{
				Bundle bundle = Bundle.CreateObject();
				bundle.NewMessage(Message.messages["Loginapp_importClientMessages"]);
				bundle.send(networkInterface);
				Dbg.DebugMsg("KBEngine::onOpenLoginapp_createAccount: send importClientMessages ...");
			}
			else
			{
				onImportClientMessagesCompleted();
			}
		}
		
		private void onConnectTo_createAccount_callback(string ip, int port, bool success, object userData)
		{
			_lastTickCBTime = System.DateTime.Now;
			
			if(!success)
			{
				Dbg.ErrorMsg(string.Format("KBEngine::createAccount_loginapp(): connect {0}:{1} is error!", ip, port));
				return;
			}
			
			Dbg.DebugMsg(string.Format("KBEngine::createAccount_loginapp(): connect {0}:{1} is success!", ip, port)); 
			onOpenLoginapp_createAccount();
		}
		
		/*
			获得了服务端摘要信息， 摘要包括协议MD5， entitydefMD5
		*/
		public void onServerDigest()
		{
			if(persistentInfos != null)
				persistentInfos.onServerDigest(currserver, serverProtocolMD5, serverEntitydefMD5);
		}

		/*
			登录loginapp失败了
		*/
		public void Client_onLoginFailed(MemoryStream stream)
		{
			UInt16 failedcode = stream.ReadUint16();
			serverDatas = stream.ReadBlob();
			Dbg.ErrorMsg("KBEngine::Client_onLoginFailed: failedcode(" + failedcode + "), datas(" + serverDatas.Length + ")!");
			Event.FireAll("onLoginFailed", new object[]{failedcode});
		}
		
		/*
			登录loginapp成功了
		*/
		public void Client_onLoginSuccessfully(MemoryStream stream)
		{
			var accountName = stream.ReadString();
			userName = accountName;
			baseappIP = stream.ReadString();
			baseappPort = stream.ReadUint16();
			
			Dbg.DebugMsg("KBEngine::Client_onLoginSuccessfully: accountName(" + accountName + "), addr(" + 
					baseappIP + ":" + baseappPort + "), datas(" + serverDatas.Length + ")!");
			
			serverDatas = stream.ReadBlob();
			login_baseapp(true);
		}
		
		/*
			登录baseapp失败了
		*/
		public void Client_onLoginBaseappFailed(UInt16 failedcode)
		{
			Dbg.ErrorMsg("KBEngine::Client_onLoginBaseappFailed: failedcode(" + failedcode + ")!");
			Event.FireAll("onLoginBaseappFailed", new object[]{failedcode});
		}

		/*
			重登录baseapp失败了
		*/
		public void Client_onReloginBaseappFailed(UInt16 failedcode)
		{
			Dbg.ErrorMsg("KBEngine::Client_onReloginBaseappFailed: failedcode(" + failedcode + ")!");
			Event.FireAll("onReloginBaseappFailed", new object[]{failedcode});
		}
		
		/*
			登录baseapp成功了
		*/
		public void Client_onReloginBaseappSuccessfully(MemoryStream stream)
		{
			entity_uuid = stream.ReadUint64();
			Dbg.DebugMsg("KBEngine::Client_onReloginBaseappSuccessfully: name(" + userName + ")!");
			Event.FireAll("onReloginBaseappSuccessfully", new object[]{});
		}

		/*
			服务端通知创建一个角色
		*/
		public void Client_onCreatedProxies(UInt64 rndUUID, Int32 eid, string entityType)
		{
			Dbg.DebugMsg("KBEngine::Client_onCreatedProxies: eid(" + eid + "), entityType(" + entityType + ")!");
			
			entity_uuid = rndUUID;
			entity_id = eid;
			entity_type = entityType;
			
			if(!this.entities.ContainsKey(eid))
			{
				ScriptModule module = null;
				if(!EntityDef.moduledefs.TryGetValue(entityType, out module))
				{
					Dbg.ErrorMsg("KBEngine::Client_onCreatedProxies: not found module(" + entityType + ")!");
					return;
				}
				
				Type runclass = module.script;
				if(runclass == null)
					return;
				
				Entity entity = (Entity)Activator.CreateInstance(runclass);
				entity.id = eid;
				entity.className = entityType;
				
				entity.baseEntityCall = new EntityCall();
				entity.baseEntityCall.id = eid;
				entity.baseEntityCall.className = entityType;
				entity.baseEntityCall.type = EntityCall.ENTITYCALL_TYPE.ENTITYCALL_TYPE_BASE;

				entities[eid] = entity;
				
				MemoryStream entityMessage = null;
				_bufferedCreateEntityMessage.TryGetValue(eid, out entityMessage);
				
				if(entityMessage != null)
				{
					Client_onUpdatePropertys(entityMessage);
					_bufferedCreateEntityMessage.Remove(eid);
					entityMessage.ReclaimObject();
				}
				
				entity.__init__();
				entity.inited = true;
				
				if(args.isOnInitCallPropertysSetMethods)
					entity.callPropertysSetMethods();
			}
			else
			{
				MemoryStream entityMessage = null;
				_bufferedCreateEntityMessage.TryGetValue(eid, out entityMessage);
				
				if(entityMessage != null)
				{
					Client_onUpdatePropertys(entityMessage);
					_bufferedCreateEntityMessage.Remove(eid);
					entityMessage.ReclaimObject();
				}
			}
		}
		
		public Entity findEntity(Int32 entityID)
		{
			Entity entity = null;
			
			if(!entities.TryGetValue(entityID, out entity))
			{
				return null;
			}
			
			return entity;
		}

		/*
			通过流数据获得View实体的ID
		*/
		public Int32 getViewEntityIDFromStream(MemoryStream stream)
		{
			if (!args.useAliasEntityID)
				return stream.ReadInt32();

			Int32 id = 0;
			if(_entityIDAliasIDList.Count > 255)
			{
				id = stream.ReadInt32();
			}
			else
			{
				byte aliasID = stream.ReadUint8();
				
				// 如果为0且客户端上一步是重登陆或者重连操作并且服务端entity在断线期间一直处于在线状态
				// 则可以忽略这个错误, 因为cellapp可能一直在向baseapp发送同步消息， 当客户端重连上时未等
				// 服务端初始化步骤开始则收到同步信息, 此时这里就会出错。
				if(_entityIDAliasIDList.Count <= aliasID)
					return 0;
				
				id = _entityIDAliasIDList[aliasID];
			}
			
			return id;
		}
		
		/*
			服务端使用优化的方式更新实体属性数据
		*/
		public void Client_onUpdatePropertysOptimized(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			onUpdatePropertys_(eid, stream);
		}
		
		/*
			服务端更新实体属性数据
		*/
		public void Client_onUpdatePropertys(MemoryStream stream)
		{
			Int32 eid = stream.ReadInt32();
			onUpdatePropertys_(eid, stream);
		}
		
		public void onUpdatePropertys_(Int32 eid, MemoryStream stream)
		{
			Entity entity = null;
			
			if(!entities.TryGetValue(eid, out entity))
			{
				MemoryStream entityMessage = null;
				if(_bufferedCreateEntityMessage.TryGetValue(eid, out entityMessage))
				{
					Dbg.ErrorMsg("KBEngine::Client_onUpdatePropertys: entity(" + eid + ") not found!");
					return;
				}

				MemoryStream stream1 = MemoryStream.CreateObject();
				stream1.wpos = stream.wpos;
				stream1.rpos = stream.rpos - 4;
				Array.Copy(stream.Data(), stream1.Data(), stream.Data().Length);
				_bufferedCreateEntityMessage[eid] = stream1;
				return;
			}
			
			ScriptModule sm = EntityDef.moduledefs[entity.className];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			while(stream.Length() > 0)
			{
				UInt16 utype = 0;
				
				if(sm.usePropertyDescrAlias)
				{
					utype = stream.ReadUint8();
				}
				else
				{
					utype = stream.ReadUint16();
				}
			
				Property propertydata = pdatas[utype];
				utype = propertydata.properUtype;
				System.Reflection.MethodInfo setmethod = propertydata.setmethod;

				object val = propertydata.utype.createFromStream(stream);
				object oldval = entity.getDefinedPropertyByUType(utype);

				 // Dbg.DEBUG_MSG("KBEngine::Client_onUpdatePropertys: " + entity.className + "(id=" + eid  + " " + 
				 // propertydata.name + "=" + val + "), hasSetMethod=" + setmethod + "!");
			
				entity.setDefinedPropertyByUType(utype, val);
				if(setmethod != null)
				{
					if(propertydata.isBase())
					{
						if(entity.inited)
							setmethod.Invoke(entity, new object[]{oldval});
					}
					else
					{
						if(entity.inWorld)
							setmethod.Invoke(entity, new object[]{oldval});
					}
				}
			}
		}

		/*
			服务端使用优化的方式调用实体方法
		*/
		public void Client_onRemoteMethodCallOptimized(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			onRemoteMethodCall_(eid, stream);
		}
		
		/*
			服务端调用实体方法
		*/
		public void Client_onRemoteMethodCall(MemoryStream stream)
		{
			Int32 eid = stream.ReadInt32();
			onRemoteMethodCall_(eid, stream);
		}
	
		public void onRemoteMethodCall_(Int32 eid, MemoryStream stream)
		{
			Entity entity = null;
			
			if(!entities.TryGetValue(eid, out entity))
			{
				Dbg.ErrorMsg("KBEngine::Client_onRemoteMethodCall: entity(" + eid + ") not found!");
				return;
			}
			
			UInt16 methodUtype = 0;

			if(EntityDef.moduledefs[entity.className].useMethodDescrAlias)
				methodUtype = stream.ReadUint8();
			else
				methodUtype = stream.ReadUint16();
			
			Method methoddata = EntityDef.moduledefs[entity.className].idmethods[methodUtype];
			
			// Dbg.DEBUG_MSG("KBEngine::Client_onRemoteMethodCall: " + entity.className + "." + methoddata.name);
			
			object[] args = new object[methoddata.args.Count];
	
			for(int i=0; i<methoddata.args.Count; i++)
			{
				args[i] = methoddata.args[i].createFromStream(stream);
			}
			
			try
			{
				methoddata.handler.Invoke(entity, args);
			}
            catch (Exception e)
            {
            	if(methoddata.handler != null)
            	{
					Dbg.ErrorMsg("KBEngine::Client_onRemoteMethodCall: " + entity.className + "(" + eid + "), callMethod(" + entity.className + "." + methoddata.name + ") is error!\nerror=" + e.ToString());
				}
				else
				{
					Dbg.ErrorMsg("KBEngine::Client_onRemoteMethodCall: " + entity.className + "(" + eid + "), not found method(" + entity.className + "." + methoddata.name + ")!\nerror=" + e.ToString());
				}
            }
		}

		/*
			服务端通知一个实体进入了世界(如果实体是当前玩家则玩家第一次在一个space中创建了， 如果是其他实体则是其他实体进入了玩家的View)
		*/
		public void Client_onEntityEnterWorld(MemoryStream stream)
		{
			Int32 eid = stream.ReadInt32();
			if(entity_id > 0 && entity_id != eid)
				_entityIDAliasIDList.Add(eid);
			
			UInt16 uentityType;
			if(EntityDef.idmoduledefs.Count > 255)
				uentityType = stream.ReadUint16();
			else
				uentityType = stream.ReadUint8();
			
			sbyte isOnGround = 1;
			
			if(stream.Length() > 0)
				isOnGround = stream.ReadInt8();
			
			string entityType = EntityDef.idmoduledefs[uentityType].name;
			// Dbg.DEBUG_MSG("KBEngine::Client_onEntityEnterWorld: " + entityType + "(" + eid + "), spaceID(" + KBEngineApp.app.spaceID + ")!");
			
			Entity entity = null;
			
			if(!entities.TryGetValue(eid, out entity))
			{
				MemoryStream entityMessage = null;
				if(!_bufferedCreateEntityMessage.TryGetValue(eid, out entityMessage))
				{
					Dbg.ErrorMsg("KBEngine::Client_onEntityEnterWorld: entity(" + eid + ") not found!");
					return;
				}
				
				ScriptModule module = null;
				if(!EntityDef.moduledefs.TryGetValue(entityType, out module))
				{
					Dbg.ErrorMsg("KBEngine::Client_onEntityEnterWorld: not found module(" + entityType + ")!");
				}
				
				Type runclass = module.script;
				if(runclass == null)
					return;
				
				entity = (Entity)Activator.CreateInstance(runclass);
				entity.id = eid;
				entity.className = entityType;
				
				entity.cellEntityCall = new EntityCall();
				entity.cellEntityCall.id = eid;
				entity.cellEntityCall.className = entityType;
				entity.cellEntityCall.type = EntityCall.ENTITYCALL_TYPE.ENTITYCALL_TYPE_CELL;
				
				entities[eid] = entity;
				
				Client_onUpdatePropertys(entityMessage);
				_bufferedCreateEntityMessage.Remove(eid);
				entityMessage.ReclaimObject();
				
				entity.isOnGround = isOnGround > 0;
				entity.set_direction(entity.getDefinedProperty("direction"));
				entity.set_position(entity.getDefinedProperty("position"));
								
				entity.__init__();
				entity.inited = true;
				entity.inWorld = true;
				entity.enterWorld();
				
				if(args.isOnInitCallPropertysSetMethods)
					entity.callPropertysSetMethods();
			}
			else
			{
				if(!entity.inWorld)
				{
					// 安全起见， 这里清空一下
					// 如果服务端上使用giveClientTo切换控制权
					// 之前的实体已经进入世界， 切换后的实体也进入世界， 这里可能会残留之前那个实体进入世界的信息
					_entityIDAliasIDList.Clear();
					clearEntities(false);
					entities[entity.id] = entity;
				
					entity.cellEntityCall = new EntityCall();
					entity.cellEntityCall.id = eid;
					entity.cellEntityCall.className = entityType;
					entity.cellEntityCall.type = EntityCall.ENTITYCALL_TYPE.ENTITYCALL_TYPE_CELL;
					
					entity.set_direction(entity.getDefinedProperty("direction"));
					entity.set_position(entity.getDefinedProperty("position"));					

					_entityServerPos = entity.position;
					entity.isOnGround = isOnGround > 0;
					entity.inWorld = true;
					entity.enterWorld();

					if(args.isOnInitCallPropertysSetMethods)
						entity.callPropertysSetMethods();
				}
			}
		}

		/*
			服务端使用优化的方式通知一个实体离开了世界(如果实体是当前玩家则玩家离开了space， 如果是其他实体则是其他实体离开了玩家的View)
		*/
		public void Client_onEntityLeaveWorldOptimized(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			NetApp.app.Client_onEntityLeaveWorld(eid);
		}

		/*
			服务端通知一个实体离开了世界(如果实体是当前玩家则玩家离开了space， 如果是其他实体则是其他实体离开了玩家的View)
		*/
		public void Client_onEntityLeaveWorld(Int32 eid)
		{
			Entity entity = null;
			
			if(!entities.TryGetValue(eid, out entity))
			{
				Dbg.ErrorMsg("KBEngine::Client_onEntityLeaveWorld: entity(" + eid + ") not found!");
				return;
			}
			
			if(entity.inWorld)
				entity.leaveWorld();
			
			if(entity_id == eid)
			{
				clearSpace(false);
				entity.cellEntityCall = null;
			}
			else
			{
				if(_controlledEntities.Remove(entity))
					Event.FireOut("onLoseControlledEntity", new object[]{entity});

				entities.Remove(eid);
				entity.onDestroy();
				_entityIDAliasIDList.Remove(eid);
			}
		}

		/*
			服务端通知当前玩家进入了一个新的space
		*/
		public void Client_onEntityEnterSpace(MemoryStream stream)
		{
			Int32 eid = stream.ReadInt32();
			spaceID = stream.ReadUint32();
			
			sbyte isOnGround = 1;
			
			if(stream.Length() > 0)
				isOnGround = stream.ReadInt8();
			
			Entity entity = null;
			
			if(!entities.TryGetValue(eid, out entity))
			{
				Dbg.ErrorMsg("KBEngine::Client_onEntityEnterSpace: entity(" + eid + ") not found!");
				return;
			}
			
			entity.isOnGround = isOnGround > 0;
			_entityServerPos = entity.position;
			entity.enterSpace();
		}
		
		/*
			服务端通知当前玩家离开了space
		*/
		public void Client_onEntityLeaveSpace(Int32 eid)
		{
			Entity entity = null;
			
			if(!entities.TryGetValue(eid, out entity))
			{
				Dbg.ErrorMsg("KBEngine::Client_onEntityLeaveSpace: entity(" + eid + ") not found!");
				return;
			}
			
			entity.leaveSpace();
			clearSpace(false);
		}
	
		/*
			账号创建返回结果
		*/
		public void Client_onCreateAccountResult(MemoryStream stream)
		{
			UInt16 retcode = stream.ReadUint16();
			byte[] datas = stream.ReadBlob();
			
			Event.FireOut("onCreateAccountResult", new object[]{retcode, datas});
			
			if(retcode != 0)
			{
				Dbg.WarningMsg("KBEngine::Client_onCreateAccountResult: " + userName + " create is failed! code=" + retcode + "!");
				return;
			}
	
			Dbg.DebugMsg("KBEngine::Client_onCreateAccountResult: " + userName + " create is successfully!");
		}

		/*
			告诉客户端：你当前负责（或取消）控制谁的位移同步
		*/
		public void Client_onControlEntity(Int32 eid, sbyte isControlled)
		{
			Entity entity = null;

			if (!entities.TryGetValue(eid, out entity))
			{
				Dbg.ErrorMsg("KBEngine::Client_onControlEntity: entity(" + eid + ") not found!");
				return;
			}

			var isCont = isControlled != 0;
			if (isCont)
			{
				// 如果被控制者是玩家自己，那表示玩家自己被其它人控制了
				// 所以玩家自己不应该进入这个被控制列表
				if (Player().id != entity.id)
				{
					_controlledEntities.Add(entity);
				}
			}
			else
			{
				_controlledEntities.Remove(entity);
			}
			
			entity.isControlled = isCont;
			
			try
			{
				entity.onControlled(isCont);
				Event.FireOut("onControlled", new object[]{entity, isCont});
			}
			catch (Exception e)
			{
				Dbg.ErrorMsg(string.Format("KBEngine::Client_onControlEntity: entity id = '{0}', is controlled = '{1}', error = '{1}'", eid, isCont, e));
			}
		}

		/*
			更新当前玩家的位置与朝向到服务端， 可以通过开关_syncPlayer关闭这个机制
		*/
		public void updatePlayerToServer()
		{
			if(!args.syncPlayer || spaceID == 0)
			{
				return;
			}

			var now = DateTime.Now;
			TimeSpan span = now - _lastUpdateToServerTime;

			if (span.Ticks < 1000000)
				return;
			
			Entity playerEntity = Player();
			if (playerEntity == null || playerEntity.inWorld == false || playerEntity.isControlled)
				return;

			_lastUpdateToServerTime = now - (span - TimeSpan.FromTicks(1000000));
			
			Vector3 position = playerEntity.position;
			Vector3 direction = playerEntity.direction;
			
			bool posHasChanged = Vector3.Distance(playerEntity._entityLastLocalPos, position) > 0.001f;
			bool dirHasChanged = Vector3.Distance(playerEntity._entityLastLocalDir, direction) > 0.001f;
			
			if(posHasChanged || dirHasChanged)
			{
				playerEntity._entityLastLocalPos = position;
				playerEntity._entityLastLocalDir = direction;

				Bundle bundle = Bundle.CreateObject();
				bundle.NewMessage(Message.messages["Baseapp_onUpdateDataFromClient"]);
				bundle.WriteFloat(position.x);
				bundle.WriteFloat(position.y);
				bundle.WriteFloat(position.z);
				
				double x = ((double)direction.x / 360 * (System.Math.PI * 2));
				double y = ((double)direction.y / 360 * (System.Math.PI * 2));
				double z = ((double)direction.z / 360 * (System.Math.PI * 2));
				
				// 根据弧度转角度公式会出现负数
				// unity会自动转化到0~360度之间，这里需要做一个还原
				if(x - System.Math.PI > 0.0)
					x -= System.Math.PI * 2;

				if(y - System.Math.PI > 0.0)
					y -= System.Math.PI * 2;
				
				if(z - System.Math.PI > 0.0)
					z -= System.Math.PI * 2;
				
				bundle.WriteFloat((float)x);
				bundle.WriteFloat((float)y);
				bundle.WriteFloat((float)z);
				bundle.WriteUint8((Byte)(playerEntity.isOnGround == true ? 1 : 0));
				bundle.WriteUint32(spaceID);
				bundle.send(networkInterface);
			}

			// 开始同步所有被控制了的entity的位置
			for (int i = 0; i < _controlledEntities.Count; ++i)
			{
				var entity = _controlledEntities[i];
				position = entity.position;
				direction = entity.direction;

				posHasChanged = Vector3.Distance(entity._entityLastLocalPos, position) > 0.001f;
				dirHasChanged = Vector3.Distance(entity._entityLastLocalDir, direction) > 0.001f;

				if (posHasChanged || dirHasChanged)
				{
					entity._entityLastLocalPos = position;
					entity._entityLastLocalDir = direction;

					Bundle bundle = Bundle.CreateObject();
					bundle.NewMessage(Message.messages["Baseapp_onUpdateDataFromClientForControlledEntity"]);
					bundle.WriteInt32(entity.id);
					bundle.WriteFloat(position.x);
					bundle.WriteFloat(position.y);
					bundle.WriteFloat(position.z);

					double x = ((double)direction.x / 360 * (System.Math.PI * 2));
					double y = ((double)direction.y / 360 * (System.Math.PI * 2));
					double z = ((double)direction.z / 360 * (System.Math.PI * 2));
				
					// 根据弧度转角度公式会出现负数
					// unity会自动转化到0~360度之间，这里需要做一个还原
					if(x - System.Math.PI > 0.0)
						x -= System.Math.PI * 2;

					if(y - System.Math.PI > 0.0)
						y -= System.Math.PI * 2;
					
					if(z - System.Math.PI > 0.0)
						z -= System.Math.PI * 2;
					
					bundle.WriteFloat((float)x);
					bundle.WriteFloat((float)y);
					bundle.WriteFloat((float)z);
					bundle.WriteUint8((Byte)(entity.isOnGround == true ? 1 : 0));
					bundle.WriteUint32(spaceID);
					bundle.send(networkInterface);
				}
			}
		}

		/*
			当前space添加了关于几何等信息的映射资源
			客户端可以通过这个资源信息来加载对应的场景
		*/
		public void addSpaceGeometryMapping(UInt32 uspaceID, string respath)
		{
			Dbg.DebugMsg("KBEngine::addSpaceGeometryMapping: spaceID(" + uspaceID + "), respath(" + respath + ")!");
			
			isLoadedGeometry = true;
			spaceID = uspaceID;
			spaceResPath = respath;
			Event.FireOut("addSpaceGeometryMapping", new object[]{spaceResPath});
		}

		public void clearSpace(bool isall)
		{
			_entityIDAliasIDList.Clear();
			_spacedatas.Clear();
			clearEntities(isall);
			isLoadedGeometry = false;
			spaceID = 0;
		}
		
		public void clearEntities(bool isall)
		{
			_controlledEntities.Clear();

			if (!isall)
			{
				Entity entity = Player();
				
				foreach (KeyValuePair<Int32, Entity> dic in entities)  
				{ 
					if(dic.Key == entity.id)
						continue;
					
					if(dic.Value.inWorld)
						dic.Value.leaveWorld();
					
				    dic.Value.onDestroy();
				}  
		
				entities.Clear();
				entities[entity.id] = entity;
			}
			else
			{
				foreach (KeyValuePair<Int32, Entity> dic in entities)  
				{ 
					if(dic.Value.inWorld)
						dic.Value.leaveWorld();

				    dic.Value.onDestroy();
				}  
		
				entities.Clear();
			}
		}
		
		/*
			服务端初始化客户端的spacedata， spacedata请参考API
		*/
		public void Client_initSpaceData(MemoryStream stream)
		{
			clearSpace(false);
			spaceID = stream.ReadUint32();
			
			while(stream.Length() > 0)
			{
				string key = stream.ReadString();
				string val = stream.ReadString();
				Client_setSpaceData(spaceID, key, val);
			}
			
			Dbg.DebugMsg("KBEngine::Client_initSpaceData: spaceID(" + spaceID + "), size(" + _spacedatas.Count + ")!");
		}

		/*
			服务端设置客户端的spacedata， spacedata请参考API
		*/
		public void Client_setSpaceData(UInt32 spaceID, string key, string value)
		{
			Dbg.DebugMsg("KBEngine::Client_setSpaceData: spaceID(" + spaceID + "), key(" + key + "), value(" + value + ")!");
			_spacedatas[key] = value;
			
			if(key == "_mapping")
				addSpaceGeometryMapping(spaceID, value);
			
			Event.FireOut("onSetSpaceData", new object[]{spaceID, key, value});
		}

		/*
			服务端删除客户端的spacedata， spacedata请参考API
		*/
		public void Client_delSpaceData(UInt32 spaceID, string key)
		{
			Dbg.DebugMsg("KBEngine::Client_delSpaceData: spaceID(" + spaceID + "), key(" + key + ")");
			_spacedatas.Remove(key);
			Event.FireOut("onDelSpaceData", new object[]{spaceID, key});
		}
		
		public string getSpaceData(string key)
		{
			string val = "";
			
			if(!_spacedatas.TryGetValue(key, out val))
			{
				return "";
			}
			
			return val;
		}

		/*
			服务端通知强制销毁一个实体
		*/
		public void Client_onEntityDestroyed(Int32 eid)
		{
			Dbg.DebugMsg("KBEngine::Client_onEntityDestroyed: entity(" + eid + ")");
			
			Entity entity = null;
			
			if(!entities.TryGetValue(eid, out entity))
			{
				Dbg.ErrorMsg("KBEngine::Client_onEntityDestroyed: entity(" + eid + ") not found!");
				return;
			}
			
			if(entity.inWorld)
			{
				if(entity_id == eid)
					clearSpace(false);
				
				entity.leaveWorld();
			}

			if(_controlledEntities.Remove(entity))
				Event.FireOut("onLoseControlledEntity", new object[]{entity});

			entities.Remove(eid);
			entity.onDestroy();
		}
		
		/*
			服务端更新玩家的基础位置， 客户端以这个基础位置加上便宜值计算出玩家周围实体的坐标
		*/
		public void Client_onUpdateBasePos(float x, float y, float z)
		{
			_entityServerPos.x = x;
			_entityServerPos.y = y;
			_entityServerPos.z = z;

			var entity = Player();
			if (entity != null && entity.isControlled)
			{
				entity.position.Set(_entityServerPos.x, _entityServerPos.y, _entityServerPos.z);
				Event.FireOut("updatePosition", new object[]{entity});
				entity.onUpdateVolatileData();
			}
		}
		
		public void Client_onUpdateBasePosXZ(float x, float z)
		{
			_entityServerPos.x = x;
			_entityServerPos.z = z;

			var entity = Player();
			if (entity != null && entity.isControlled)
			{
				entity.position.x = _entityServerPos.x;
				entity.position.z = _entityServerPos.z;
				Event.FireOut("updatePosition", new object[]{entity});
				entity.onUpdateVolatileData();
			}
		}

		public void Client_onUpdateBaseDir(MemoryStream stream)
		{
			float yaw, pitch, roll;
			yaw = stream.ReadFloat() * 360 / ((float)System.Math.PI * 2);
			pitch = stream.ReadFloat() * 360 / ((float)System.Math.PI * 2);
			roll = stream.ReadFloat() * 360 / ((float)System.Math.PI * 2);

			var entity = Player();
			if (entity != null && entity.isControlled)
			{
				entity.direction.Set(roll, pitch, yaw);
				Event.FireOut("set_direction", new object[]{entity});
				entity.onUpdateVolatileData();
			}
		}

		public void Client_onUpdateData(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			Entity entity = null;
			
			if(!entities.TryGetValue(eid, out entity))
			{
				Dbg.ErrorMsg("KBEngine::Client_onUpdateData: entity(" + eid + ") not found!");
				return;
			}
		}

		/*
			服务端强制设置了玩家的坐标 
			例如：在服务端使用avatar.position=(0,0,0), 或者玩家位置与速度异常时会强制拉回到一个位置
		*/
		public void Client_onSetEntityPosAndDir(MemoryStream stream)
		{
			Int32 eid = stream.ReadInt32();
			Entity entity = null;
			
			if(!entities.TryGetValue(eid, out entity))
			{
				Dbg.ErrorMsg("KBEngine::Client_onSetEntityPosAndDir: entity(" + eid + ") not found!");
				return;
			}
			
			entity.position.x = stream.ReadFloat();
			entity.position.y = stream.ReadFloat();
			entity.position.z = stream.ReadFloat();
			
			entity.direction.x = stream.ReadFloat();
			entity.direction.y = stream.ReadFloat();
			entity.direction.z = stream.ReadFloat();
			
			Vector3 position = (Vector3)entity.getDefinedProperty("position");
			Vector3 direction = (Vector3)entity.getDefinedProperty("direction");
			Vector3 old_position = new Vector3(position.x, position.y, position.z);
			Vector3 old_direction = new Vector3(direction.x, direction.y, direction.z);
			
			position.x = entity.position.x;
			position.y = entity.position.y;
			position.z = entity.position.z;
			
			direction.x = entity.direction.x;
			direction.y = entity.direction.y;
			direction.z = entity.direction.z;
			
			entity.setDefinedProperty("position", position);
			entity.setDefinedProperty("direction", direction);
			
			entity._entityLastLocalPos = entity.position;
			entity._entityLastLocalDir = entity.direction;
			
			entity.set_direction(old_direction);
			entity.set_position(old_position);
		}
		
		public void Client_onUpdateData_ypr(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			SByte y = stream.ReadInt8();
			SByte p = stream.ReadInt8();
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, y, p, r, -1);
		}
		
		public void Client_onUpdateData_yp(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			SByte y = stream.ReadInt8();
			SByte p = stream.ReadInt8();
			
			_updateVolatileData(eid, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, y, p, KBEDATATYPE_BASE.KBE_FLT_MAX, -1);
		}
		
		public void Client_onUpdateData_yr(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			SByte y = stream.ReadInt8();
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, y, KBEDATATYPE_BASE.KBE_FLT_MAX, r, -1);
		}
		
		public void Client_onUpdateData_pr(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			SByte p = stream.ReadInt8();
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, p, r, -1);
		}
		
		public void Client_onUpdateData_y(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			SByte y = stream.ReadInt8();
			
			_updateVolatileData(eid, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, y, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, -1);
		}
		
		public void Client_onUpdateData_p(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			SByte p = stream.ReadInt8();
			
			_updateVolatileData(eid, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, p, KBEDATATYPE_BASE.KBE_FLT_MAX, -1);
		}
		
		public void Client_onUpdateData_r(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, r, -1);
		}
		
		public void Client_onUpdateData_xz(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
			
			_updateVolatileData(eid, xz[0], KBEDATATYPE_BASE.KBE_FLT_MAX, xz[1], KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, 1);
		}
		
		public void Client_onUpdateData_xz_ypr(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
	
			SByte y = stream.ReadInt8();
			SByte p = stream.ReadInt8();
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, xz[0], KBEDATATYPE_BASE.KBE_FLT_MAX, xz[1], y, p, r, 1);
		}
		
		public void Client_onUpdateData_xz_yp(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
	
			SByte y = stream.ReadInt8();
			SByte p = stream.ReadInt8();
			
			_updateVolatileData(eid, xz[0], KBEDATATYPE_BASE.KBE_FLT_MAX, xz[1], y, p, KBEDATATYPE_BASE.KBE_FLT_MAX, 1);
		}
		
		public void Client_onUpdateData_xz_yr(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
	
			SByte y = stream.ReadInt8();
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, xz[0], KBEDATATYPE_BASE.KBE_FLT_MAX, xz[1], y, KBEDATATYPE_BASE.KBE_FLT_MAX, r, 1);
		}
		
		public void Client_onUpdateData_xz_pr(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
	
			SByte p = stream.ReadInt8();
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, xz[0], KBEDATATYPE_BASE.KBE_FLT_MAX, xz[1], KBEDATATYPE_BASE.KBE_FLT_MAX, p, r, 1);
		}
		
		public void Client_onUpdateData_xz_y(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			Vector2 xz = stream.ReadPackXZ();
			SByte yaw = stream.ReadInt8();
			_updateVolatileData(eid, xz[0], KBEDATATYPE_BASE.KBE_FLT_MAX, xz[1], yaw, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, 1);
		}
		
		public void Client_onUpdateData_xz_p(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
	
			SByte p = stream.ReadInt8();
			
			_updateVolatileData(eid, xz[0], KBEDATATYPE_BASE.KBE_FLT_MAX, xz[1], KBEDATATYPE_BASE.KBE_FLT_MAX, p, KBEDATATYPE_BASE.KBE_FLT_MAX, 1);
		}
		
		public void Client_onUpdateData_xz_r(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
	
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, xz[0], KBEDATATYPE_BASE.KBE_FLT_MAX, xz[1], KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, r, 1);
		}
		
		public void Client_onUpdateData_xyz(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
			float y = stream.ReadPackY();
			
			_updateVolatileData(eid, xz[0], y, xz[1], KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, 0);
		}
		
		public void Client_onUpdateData_xyz_ypr(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
			float y = stream.ReadPackY();
			
			SByte yaw = stream.ReadInt8();
			SByte p = stream.ReadInt8();
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, xz[0], y, xz[1], yaw, p, r, 0);
		}
		
		public void Client_onUpdateData_xyz_yp(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
			float y = stream.ReadPackY();
			
			SByte yaw = stream.ReadInt8();
			SByte p = stream.ReadInt8();

			_updateVolatileData(eid, xz[0], y, xz[1], yaw, p, KBEDATATYPE_BASE.KBE_FLT_MAX, 0);
		}
		
		public void Client_onUpdateData_xyz_yr(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
			float y = stream.ReadPackY();
			
			SByte yaw = stream.ReadInt8();
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, xz[0], y, xz[1], yaw, KBEDATATYPE_BASE.KBE_FLT_MAX, r, 0);
		}
		
		public void Client_onUpdateData_xyz_pr(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
			float y = stream.ReadPackY();
			
			SByte p = stream.ReadInt8();
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, xz[0], y, xz[1], KBEDATATYPE_BASE.KBE_FLT_MAX, p, r, 0);
		}
		
		public void Client_onUpdateData_xyz_y(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
			float y = stream.ReadPackY();
			
			SByte yaw = stream.ReadInt8();
			_updateVolatileData(eid, xz[0], y, xz[1], yaw, KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, 0);
		}
		
		public void Client_onUpdateData_xyz_p(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
			float y = stream.ReadPackY();
			
			SByte p = stream.ReadInt8();
			
			_updateVolatileData(eid, xz[0], y, xz[1], KBEDATATYPE_BASE.KBE_FLT_MAX, p, KBEDATATYPE_BASE.KBE_FLT_MAX, 0);
		}
		
		public void Client_onUpdateData_xyz_r(MemoryStream stream)
		{
			Int32 eid = getViewEntityIDFromStream(stream);
			
			Vector2 xz = stream.ReadPackXZ();
			float y = stream.ReadPackY();
			
			SByte r = stream.ReadInt8();
			
			_updateVolatileData(eid, xz[0], y, xz[1], KBEDATATYPE_BASE.KBE_FLT_MAX, KBEDATATYPE_BASE.KBE_FLT_MAX, r, 0);
		}
		
		private void _updateVolatileData(Int32 entityID, float x, float y, float z, float yaw, float pitch, float roll, sbyte isOnGround)
		{
			Entity entity = null;

			if(!entities.TryGetValue(entityID, out entity))
			{
				// 如果为0且客户端上一步是重登陆或者重连操作并且服务端entity在断线期间一直处于在线状态
				// 则可以忽略这个错误, 因为cellapp可能一直在向baseapp发送同步消息， 当客户端重连上时未等
				// 服务端初始化步骤开始则收到同步信息, 此时这里就会出错。
				Dbg.ErrorMsg("KBEngine::_updateVolatileData: entity(" + entityID + ") not found!");
				return;
			}
			
			// 小于0不设置
			if(isOnGround >= 0)
			{
				entity.isOnGround = (isOnGround > 0);
			}
		
			bool changeDirection = false;
			
			if(roll != KBEDATATYPE_BASE.KBE_FLT_MAX)
			{
				changeDirection = true;
				entity.direction.x = NetMath.int82angle((SByte)roll, false) * 360 / ((float)System.Math.PI * 2);
			}

			if(pitch != KBEDATATYPE_BASE.KBE_FLT_MAX)
			{
				changeDirection = true;
				entity.direction.y = NetMath.int82angle((SByte)pitch, false) * 360 / ((float)System.Math.PI * 2);
			}
			
			if(yaw != KBEDATATYPE_BASE.KBE_FLT_MAX)
			{
				changeDirection = true;
				entity.direction.z = NetMath.int82angle((SByte)yaw, false) * 360 / ((float)System.Math.PI * 2);
			}
			
			bool done = false;
			if(changeDirection == true)
			{
				Event.FireOut("set_direction", new object[]{entity});
				done = true;
			}
			
	            bool positionChanged = x != KBEDATATYPE_BASE.KBE_FLT_MAX || y != KBEDATATYPE_BASE.KBE_FLT_MAX || z != KBEDATATYPE_BASE.KBE_FLT_MAX;
	            if (x == KBEDATATYPE_BASE.KBE_FLT_MAX) x = 0.0f;
	            if (y == KBEDATATYPE_BASE.KBE_FLT_MAX) y = 0.0f;
	            if (z == KBEDATATYPE_BASE.KBE_FLT_MAX) z = 0.0f;
            
			if(positionChanged)
			{
				Vector3 pos = new Vector3(x + _entityServerPos.x, y + _entityServerPos.y, z + _entityServerPos.z);
				
				entity.position = pos;
				done = true;
				Event.FireOut("updatePosition", new object[]{entity});
			}
			
			if(done)
				entity.onUpdateVolatileData();
		}
		
		/*
			服务端通知流数据下载开始
			请参考API手册关于onStreamDataStarted
		*/
		public void Client_onStreamDataStarted(Int16 id, UInt32 datasize, string descr)
		{
			Event.FireOut("onStreamDataStarted", new object[]{id, datasize, descr});
		}
		
		public void Client_onStreamDataRecv(MemoryStream stream)
		{
			Int16 resID = stream.ReadInt16();
			byte[] datas = stream.ReadBlob();
			Event.FireOut("onStreamDataRecv", new object[]{resID, datas});
		}
		
		public void Client_onStreamDataCompleted(Int16 id)
		{
			Event.FireOut("onStreamDataCompleted", new object[]{id});
		}
	}
	

	public class KBEngineAppThread : NetApp
	{
		/*
			KBEngine处理线程
		*/
	    public class KBEThread
	    {

	        NetApp app_;
			public bool over = false;
			
	        public KBEThread(NetApp app)
	        {
	            this.app_ = app;
	        }

	        public void run()
	        {
				Dbg.InfoMsg("KBEThread::run()");
				over = false;

	            try
	            {
	                this.app_.Process();
	            }
	            catch (Exception e)
	            {
	                Dbg.ErrorMsg(e.ToString());
	            }
				
				over = true;
				Dbg.InfoMsg("KBEThread::end()");
	        }
	    }
    
		private Thread _t = null;
		public KBEThread kbethread = null;

		// 主循环频率
		public static int threadUpdateHZ = 10;

		// 主循环周期ms 优化去掉循环中做除法
		private static float threadUpdatePeriod = 1000f / threadUpdateHZ;
		
		// 插件是否退出
		private bool _isbreak = false;
		
		private System.DateTime _lasttime = System.DateTime.Now;

		public KBEngineAppThread(NetArgs args) : 
			base(args)
		{
		}

		public override bool Initialize(NetArgs args)
		{
			base.Initialize(args);
			
			KBEngineAppThread.threadUpdateHZ = args.threadUpdateHZ;
			threadUpdatePeriod = 1000f / threadUpdateHZ;
			
			kbethread = new KBEThread(this);
			_t = new Thread(new ThreadStart(kbethread.run));
			_t.Start();
			
			return true;
		}

		public override void Reset()
		{
			_isbreak = false;
			_lasttime = System.DateTime.Now;
			
			base.Reset();
		}

		/*
			插件退出处理
		*/
		public void breakProcess()
		{
			_isbreak = true;
		}
		
		public bool isbreak()
		{
			return _isbreak;
		}
		
		public override void Process()
		{
			while(!isbreak())
			{
				base.Process();
				_thread_wait();
			}
			
			Dbg.WarningMsg("KBEngineAppThread::process(): break!");
		}
	
		/*
			防止占满CPU, 需要让线程等待一会
		*/
		void _thread_wait()
		{
			TimeSpan span = DateTime.Now - _lasttime; 
			
			int diff = (int)(threadUpdatePeriod - span.Milliseconds);

			if(diff < 0)
				diff = 0;
			
			System.Threading.Thread.Sleep(diff);
			_lasttime = DateTime.Now;
		}
		
		public override void Destroy()
		{
			Dbg.WarningMsg("KBEngineAppThread::destroy()");
			breakProcess();
			
			int i = 0;
			while(!kbethread.over && i < 50)
			{
				Thread.Sleep(100);
				i += 1;
			}
			
			if(_t != null)
				_t.Abort();
			
			_t = null;

			base.Destroy();
		}
	}
} 
