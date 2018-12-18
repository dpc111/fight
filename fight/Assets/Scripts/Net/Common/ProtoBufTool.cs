namespace Net {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using System.IO;
    using ProtoBuf;

    public class ProtoBufTool {
        public static object MsgParse(Type type, ref byte[] data, int len) {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ms.Write(data, 0, len);
            ms.Position = 0;
            var msg = ProtoBuf.Serializer.NonGeneric.Deserialize(type, ms);
            return msg;
        }
    }
}
