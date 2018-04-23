using UnityEngine;
using Net;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Net
{
	public enum DebugLevel : int
	{
		debug = 0,
		info,
		warning,
		error,
		nolog,              //放在最后面，使用这个时表示不输出任何日志
	}

	public class Dbg 
	{
		static public DebugLevel debugLevel = DebugLevel.debug;
#if UNITY_EDITOR
		static Dictionary<string, Profile> profiles = new Dictionary<string, Profile>();
#endif

		public static string GetHead()
		{
			return "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + "] ";
		}

		public static void InfoMsg(object s)
		{
			if (DebugLevel.info >= debugLevel)
				Debug.Log(GetHead() + s);
		}

		public static void DebugMsg(object s)
		{
			if (DebugLevel.debug >= debugLevel)
				Debug.Log(GetHead() + s);
		}

		public static void WarningMsg(object s)
		{
			if (DebugLevel.warning >= debugLevel)
				Debug.LogWarning(GetHead() + s);
		}

		public static void ErrorMsg(object s)
		{
			if (DebugLevel.error >= debugLevel)
				Debug.LogError(GetHead() + s);
		}

		public static void ProfileStart(string name)
		{
#if UNITY_EDITOR
			Profile p = null;
			if(!profiles.TryGetValue(name, out p))
			{
				p = new Profile(name);
				profiles.Add(name, p);
			}
			p.start();
#endif
		}

		public static void ProfileEnd(string name)
		{
#if UNITY_EDITOR
			profiles[name].end();
#endif
		}
		
	}
}
