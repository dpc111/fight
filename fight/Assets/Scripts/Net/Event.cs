namespace Net
{
  	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;
	using System.Threading;
	
    //事件模块
    //Net插件层与Unity3D表现层通过事件来交互
    public class Event
    {
		public struct Pair
		{
			public object obj;
			public string funcName;
			public System.Reflection.MethodInfo method;
		};
		
		public struct EventObj
		{
			public Pair info;
			public object[] args;
		};
		
    	static Dictionary<string, List<Pair>> eventsOut = new Dictionary<string, List<Pair>>();
		static LinkedList<EventObj> firedEventsOut = new LinkedList<EventObj>();
		static LinkedList<EventObj> doingEventsOut = new LinkedList<EventObj>();
		
    	static Dictionary<string, List<Pair>> eventsIn = new Dictionary<string, List<Pair>>();
		static LinkedList<EventObj> firedEventsIn = new LinkedList<EventObj>();
		static LinkedList<EventObj> doingEventsIn = new LinkedList<EventObj>();

		static bool isPause = false;
	
		public Event()
		{
		}
		
		public static void clear()
		{
			eventsOut.Clear();
			eventsIn.Clear();
			clearFiredEvents();
		}

		public static void clearFiredEvents()
		{
			MonitorEnter(eventsOut);
			firedEventsOut.Clear();
			MonitorExit(eventsOut);
			
			doingEventsOut.Clear();
			
			MonitorEnter(eventsIn);
			firedEventsIn.Clear();
			MonitorExit(eventsIn);
			
			doingEventsIn.Clear();
			
			isPause = false;
		}
		
		public static void Pause()
		{
			isPause = true;
		}

		public static void Resume()
		{
			isPause = false;
		}

		public static bool IsPause()
		{
			return isPause;
		}

		public static void MonitorEnter(object obj)
		{
			if(NetApp.app == null)
				return;
			
			Monitor.Enter(obj);
		}

		public static void MonitorExit(object obj)
		{
			if(NetApp.app == null)
				return;
			
			Monitor.Exit(obj);
		}
		
		public static bool HasRegisterOut(string eventName)
		{
			return HasRegister(eventsOut, eventName);
		}

		public static bool hasRegisterIn(string eventName)
		{
			return HasRegister(eventsIn, eventName);
		}
		
		private static bool HasRegister(Dictionary<string, List<Pair>> events, string eventName)
		{
			bool has = false;
			MonitorEnter(events);
			has = events.ContainsKey(eventName);
			MonitorExit(events);
			return has;
		}
		
        //注册监听由Net插件抛出的事件。(out = Net->render)
        //通常由渲染表现层来注册, 例如：监听角色血量属性的变化， 如果UI层注册这个事件，
        //事件触发后就可以根据事件所附带的当前血量值来改变角色头顶的血条值。
		public static bool RegisterOut(string eventName, object obj, string funcName)
		{
			return Register(eventsOut, eventName, obj, funcName);
		}

        //注册监听由渲染表现层抛出的事件(in = render->Net)
        //通常由kbe插件层来注册， 例如：UI层点击登录， 此时需要触发一个事件给kbe插件层进行与服务端交互的处理。
		public static bool RegisterIn(string eventName, object obj, string funcName)
		{
			return Register(eventsIn, eventName, obj, funcName);
		}
		
		private static bool Register(Dictionary<string, List<Pair>> events, string eventName, object obj, string funcName)
		{
			Deregister(events, eventName, obj, funcName);
			List<Pair> lst = null;
			
			Pair pair = new Pair();
			pair.obj = obj;
			pair.funcName = funcName;
			pair.method = obj.GetType().GetMethod(funcName);
			if(pair.method == null)
			{
				Dbg.ERROR_MSG("Event::register/" + obj + "/not found method/" + funcName);
				return false;
			}
			
			MonitorEnter(events);
			if(!events.TryGetValue(eventName, out lst))
			{
				lst = new List<Pair>();
				lst.Add(pair);
				events.Add(eventName, lst);
				MonitorExit(events);
				return true;
			}
			
			lst.Add(pair);
			MonitorExit(events);
			return true;
		}

		public static bool DeregisterOut(string eventName, object obj, string funcName)
		{
			return Deregister(eventsOut, eventName, obj, funcName);
		}

		public static bool DeregisterIn(string eventName, object obj, string funcName)
		{
			return Deregister(eventsIn, eventName, obj, funcName);
		}
		
		private static bool Deregister(Dictionary<string, List<Pair>> events, string eventName, object obj, string funcName)
		{
			MonitorEnter(events);
			List<Pair> lst = null;
			
			if(!events.TryGetValue(eventName, out lst))
			{
				MonitorExit(events);
				return false;
			}
			
			for(int i=0; i<lst.Count; i++)
			{
				if(obj == lst[i].obj && lst[i].funcName == funcName)
				{
					lst.RemoveAt(i);
					MonitorExit(events);
					return true;
				}
			}
			
			MonitorExit(events);
			return false;
		}

		public static bool DeregisterOut(object obj)
		{
			return Deregister(eventsOut, obj);
		}

		public static bool DeregisterIn(object obj)
		{
			return Deregister(eventsIn, obj);
		}
		
		private static bool Deregister(Dictionary<string, List<Pair>> events, object obj)
		{
			MonitorEnter(events);
			
			var iter = events.GetEnumerator();
			while (iter.MoveNext())
			{
				List<Pair> lst = iter.Current.Value;
				for (int i = lst.Count - 1; i >= 0; i--)
				{
					if (obj == lst[i].obj)
					{
						lst.RemoveAt(i);
					}
				}
			}
			
			MonitorExit(events);
			return true;
		}

        //Net插件触发事件(out = Net->render)
        //通常由渲染表现层来注册, 例如：监听角色血量属性的变化， 如果UI层注册这个事件，
        //事件触发后就可以根据事件所附带的当前血量值来改变角色头顶的血条值。
		public static void FireOut(string eventName, params object[] args)
		{
			Fire(eventsOut, firedEventsOut, eventName, args);
		}

        //渲染表现层抛出事件(in = render->Net)
        //通常由Net插件层来注册， 例如：UI层点击登录， 此时需要触发一个事件给Net插件层进行与服务端交互的处理。
		public static void FireIn(string eventName, params object[] args)
		{
			Fire(eventsIn, firedEventsIn, eventName, args);
		}

		/*
			触发kbe插件和渲染表现层都能够收到的事件
		*/
		public static void FireAll(string eventName, params object[] args)
		{
			Fire(eventsIn, firedEventsIn, eventName, args);
			Fire(eventsOut, firedEventsOut, eventName, args);
		}
		
		private static void Fire(Dictionary<string, List<Pair>> events, LinkedList<EventObj> firedEvents, string eventName, object[] args)
		{
			MonitorEnter(events);
			List<Pair> lst = null;
			
			if(!events.TryGetValue(eventName, out lst))
			{
				if(events == eventsIn)
					Dbg.WARNING_MSG("Event::fireIn: event/" + eventName + "/not found!");
				else
					Dbg.WARNING_MSG("Event::fireOut: event/" + eventName + "/not found!");
				
				MonitorExit(events);
				return;
			}
			
			for(int i=0; i<lst.Count; i++)
			{
				EventObj eObj = new EventObj();
				eObj.info = lst[i];
				eObj.args = args;
				firedEvents.AddLast(eObj);
			}
			MonitorExit(events);
		}
		
		public static void ProcessOutEvents()
		{
			MonitorEnter(eventsOut);
			if(firedEventsOut.Count > 0)
			{
				var iter = firedEventsOut.GetEnumerator();
				while (iter.MoveNext())
				{
					doingEventsOut.AddLast(iter.Current);
				}

				firedEventsOut.Clear();
			}
			MonitorExit(eventsOut);

			while (doingEventsOut.Count > 0 && !isPause) 
			{
				EventObj eObj = doingEventsOut.First.Value;
				try
				{
					eObj.info.method.Invoke(eObj.info.obj, eObj.args);
				}
	            catch (Exception e)
	            {
	            	Dbg.ERROR_MSG("Event::processOutEvents/" + eObj.info.funcName + "/" + e.ToString());
	            }
				if(doingEventsOut.Count > 0)
					doingEventsOut.RemoveFirst();
			}
		}
		
		public static void ProcessInEvents()
		{
			MonitorEnter(eventsIn);
			if(firedEventsIn.Count > 0)
			{
				var iter = firedEventsIn.GetEnumerator();
				while (iter.MoveNext())
				{
					doingEventsIn.AddLast(iter.Current);
				}

				firedEventsIn.Clear();
			}
			MonitorExit(eventsIn);

			while (doingEventsIn.Count > 0) 
			{
				EventObj eobj = doingEventsIn.First.Value;
				try
				{
					eobj.info.method.Invoke(eobj.info.obj, eobj.args);
				}
	            catch (Exception e)
	            {
	            	Dbg.ERROR_MSG("Event::processInEvents/" + eobj.info.funcName + "/" + e.ToString());
	            }
				if(doingEventsIn.Count > 0)
					doingEventsIn.RemoveFirst();
			}
		}
	
    }
} 
