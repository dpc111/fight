namespace NetModel
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    public class Event
    {
        public struct EventInfo
        {
            public object obj;
            public string funcName;
            public System.Reflection.MethodInfo method;
        };

        public struct EventObj
        {
            public EventInfo info;
            public object[] args;
        };

        static public Dictionary<string, List<EventInfo>> eventsOut = new Dictionary<string, List<EventInfo>>();
        static LinkedList<EventObj> firedEventsOut = new LinkedList<EventObj>();
        static LinkedList<EventObj> doingEventsOut = new LinkedList<EventObj>();
        static public Dictionary<string, List<EventInfo>> eventsIn = new Dictionary<string, List<EventInfo>>();
        static LinkedList<EventObj> firedEventsIn = new LinkedList<EventObj>();
        static LinkedList<EventObj> doingEventsIn = new LinkedList<EventObj>();

        public static void MonitorEnter(object obj)
        {
            Monitor.Enter(obj);
        }

        public static void MonitorExit(object obj)
        {
            Monitor.Exit(obj);
        }

        public Event()
        {

        }

        public static void Clear()
        {
            eventsOut.Clear();
            eventsIn.Clear();
            ClearFiredEvents();
        }

        public static void ClearFiredEvents()
        {
            MonitorEnter(eventsOut);
            firedEventsOut.Clear();
            MonitorExit(eventsOut);
            doingEventsOut.Clear();
            MonitorEnter(eventsIn);
            firedEventsIn.Clear();
            MonitorExit(eventsIn);
            doingEventsIn.Clear();
        }

        public static bool HasRegisterOut(string eventName)
        {
            return HasRegister(eventsOut, eventName);
        }

        public static bool HasRegisterIn(string eventName)
        {
            return HasRegister(eventsIn, eventName);
        }

        public static bool RegisterOut(string eventName, object obj, string funcName)
        {
            return Register(eventsOut, eventName, obj, funcName);
        }

        public static bool RegisterIn(string eventName, object obj, string funcName)
        {
            return Register(eventsIn, eventName, obj, funcName);
        }

        public static bool DeregisterOut(string eventName, object obj, string funcName)
        {
            return Deregister(eventsOut, eventName, obj, funcName);
        }

        public static bool DeregisterIn(string eventName, object obj, string funcName)
        {
            return Deregister(eventsIn, eventName, obj, funcName);
        }

        public static bool DeregisterOut(object obj)
        {
            return Deregister(eventsOut, obj);
        }

        public static bool DeregisterIn(object obj)
        {
            return Deregister(eventsIn, obj);
        }

        public static void FireOut(string eventName, params object[] args)
        {
            Fire(eventsOut, firedEventsOut, eventName, args);
        }

        public static void FireIn(string eventName, params object[] args)
        {
            Fire(eventsIn, firedEventsIn, eventName, args);
        }

        public static void FireAll(string eventName, params object[] args)
        {
            Fire(eventsIn, firedEventsIn, eventName, args);
            Fire(eventsOut, firedEventsOut, eventName, args);
        }

        public static void ProcessOutEvent()
        {
            MonitorEnter(eventsOut);
            if (firedEventsOut.Count > 0)
            {
                var iter = firedEventsOut.GetEnumerator();
                while (iter.MoveNext())
                {
                    doingEventsOut.AddLast(iter.Current);
                }
                firedEventsOut.Clear();
            }
            MonitorExit(eventsOut);
            while (doingEventsOut.Count > 0)
            {
                EventObj eObj = doingEventsOut.First.Value;
                try
                {
                    eObj.info.method.Invoke(eObj.info.obj, eObj.args);
                }
                catch (Exception e)
                {
                    // error
                }
                if (doingEventsOut.Count > 0)
                {
                    doingEventsOut.RemoveFirst();
                }
            }
        }

        public static void ProcessInEvent()
        {
            MonitorEnter(eventsIn);
            if (firedEventsIn.Count > 0)
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
                EventObj eObj = doingEventsIn.First.Value;
                try
                {
                    eObj.info.method.Invoke(eObj.info.obj, eObj.args);
                }
                catch (Exception e)
                {
                    // error
                }
                if (doingEventsIn.Count > 0)
                {
                    doingEventsIn.RemoveFirst();
                }
            }
        }

        private static bool HasRegister(Dictionary<string, List<EventInfo>> events, string eventName)
        {
            bool has = false;
            MonitorEnter(events);
            has = events.ContainsKey(eventName);
            MonitorExit(events);
            return has;
        }

        private static bool Register(Dictionary<string, List<EventInfo>> events, string eventName, object obj, string funcName)
        {
            Deregister(events, eventName, obj, funcName);
            List<EventInfo> lst = null;
            EventInfo info = new EventInfo();
            info.obj = obj;
            info.funcName = funcName;
            info.method = obj.GetType().GetMethod(funcName);
            if (info.method == null)
            {
                // error
                return false;
            }
            MonitorEnter(events);
            if (!events.TryGetValue(eventName, out lst))
            {
                lst = new List<EventInfo>();
                lst.Add(info);
                events.Add(eventName, lst);
                MonitorExit(events);
                return true;
            }
            lst.Add(info);
            MonitorExit(events);
            return true;
        }

        private static bool Deregister(Dictionary<string, List<EventInfo>> events, string eventName, object obj, string funcName)
        {
            MonitorEnter(events);
            List<EventInfo> lst = null;
            if (!events.TryGetValue(eventName, out lst))
            {
                MonitorExit(events);
                return false;
            }
            for (int i = 0; i < lst.Count; i++)
            {
                if (obj == lst[i].obj && lst[i].funcName == funcName)
                {
                    lst.RemoveAt(i);
                    MonitorExit(events);
                    return true;
                }
            }
            MonitorExit(events);
            return false;
        }

        private static bool Deregister(Dictionary<string, List<EventInfo>> events, object obj)
        {
            MonitorEnter(events);
            var iter = events.GetEnumerator();
            while (iter.MoveNext())
            {
                List<EventInfo> lst = iter.Current.Value;
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

        private static void Fire(Dictionary<string, List<EventInfo>> events, LinkedList<EventObj> fireEvents, string eventName, object[] args)
        {
            MonitorEnter(events);
            List<EventInfo> lst = null;
            if (!events.TryGetValue(eventName, out lst))
            {
                // error
                MonitorExit(events);
                return;
            }
            for (int i = 0; i < lst.Count; i++)
            {
                EventObj eObj = new EventObj();
                eObj.info = lst[i];
                eObj.args = args;
                fireEvents.AddLast(eObj);
            }
            MonitorExit(events);
        }
    }
}