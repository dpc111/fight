using UnityEngine;
using System;
using System.Collections;
using Net;

//可以理解为插件的入口模块
//在这个入口中安装了需要监听的事件(installEvents)，同时初始化KBEngine(initKBEngine)
public class NetMain : MonoBehaviour 
{
	public NetApp gamApp = null;
	//在unity3d界面中可见选项
	public DebugLevel debugLevel = DebugLevel.debug;
	public bool isMultiThreads = true;
	public string ip = "127.0.0.1";
	public int port = 20013;
	public NetApp.ClientType clientType = NetApp.ClientType.ClientTypeMini;
	public string persistentDataPath = "Application.persistentDataPath";
	public bool syncPlayer = true;
	public int threadUpdateHZ = 10;
	public int serverHeartbeatTick = 15;
	public int sendBufferMax = (int)Net.NetworkInterface.tcpPacketMax;
	public int recvBufferMax = (int)Net.NetworkInterface.tcpPacketMax;
	public bool useAliasEntityID = true;
	public bool isOnInitCallPropertysSetMethods = true;

	protected virtual void Awake() 
	 {
		DontDestroyOnLoad(transform.gameObject);
	 }
 
	protected virtual void Start () 
	{
		MonoBehaviour.print("clientapp::start()");
		InstallEvents();
		InitNet();
	}
	
	public virtual void InstallEvents()
	{
	}
	
	public virtual void InitNet()
	{
		Dbg.debugLevel = debugLevel;
		NetArgs args = new NetArgs();
		
		args.ip = ip;
		args.port = port;
		args.clientType = clientType;
		
		if(persistentDataPath == "Application.persistentDataPath")
			args.persistentDataPath = Application.persistentDataPath;
		else
			args.persistentDataPath = persistentDataPath;
		
		args.syncPlayer = syncPlayer;
		args.threadUpdateHZ = threadUpdateHZ;
		args.serverHeartbeatTick = serverHeartbeatTick;
		args.useAliasEntityID = useAliasEntityID;
		args.isOnInitCallPropertysSetMethods = isOnInitCallPropertysSetMethods;

		args.sendBufferMax = (UInt32)sendBufferMax;
		args.recvBufferMax = (UInt32)recvBufferMax;
		
		args.isMultiThreads = isMultiThreads;
		
		if(isMultiThreads)
			gamApp = new KBEngineAppThread(args);
		else
			gamApp = new NetApp(args);
	}
	
	protected virtual void OnDestroy()
	{
		MonoBehaviour.print("clientapp::OnDestroy(): begin");
        if (NetApp.app != null)
        {
            NetApp.app.Destroy();
            NetApp.app = null;
        }
		MonoBehaviour.print("clientapp::OnDestroy(): end");
	}
	
	protected virtual void FixedUpdate () 
	{
		KBEUpdate();
	}

	public virtual void KBEUpdate()
	{
		// 单线程模式必须自己调用
		if(!isMultiThreads)
			gamApp.Process();
		
		Net.Event.ProcessOutEvents();
	}
}
