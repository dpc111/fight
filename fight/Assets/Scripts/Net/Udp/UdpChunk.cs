namespace Net {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    public class UdpChunk {
        public byte size;
        public byte type;
        public int seq;
        public int ack;
        public byte[] buff = new byte[512];
    }
}