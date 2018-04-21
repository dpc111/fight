namespace Net
{
  	using UnityEngine; 
	using System; 
	using System.Net; 
	using System.Collections; 
	using System.Collections.Generic;
	using System.Text;
    using System.Threading; 
	using System.Runtime.InteropServices;
	
    //二进制数据流模块
    //能够将一些基本类型序列化(writeXXX)成二进制流同时也提供了反序列化(readXXX)等操作
	public class MemoryStream : ObjectPool<MemoryStream>
    {
    	public const int bufferMax = 1460 * 4;
    	public int rpos = 0;
    	public int wpos = 0;
    	private byte[] datas = new byte[bufferMax]; 
    	private static System.Text.ASCIIEncoding converter = new System.Text.ASCIIEncoding();
    	
		[StructLayout(LayoutKind.Explicit, Size = 4)]
		struct PackFloatXType
		{
		    [FieldOffset(0)]
		    public float fv;
		    [FieldOffset(0)]
		    public UInt32 uv;
		    [FieldOffset(0)]
		    public Int32 iv;
		}

		//把自己放回缓冲池
		public void ReclaimObject()
		{
			Clear();
			ReclaimObject(this);
		}
		
		public byte[] Data()
    	{
    		return datas;
    	}
		
		public void SetData(byte[] data)
		{
			datas = data;
		}
		
		//---------------------------------------------------------------------------------
		public SByte ReadInt8()
		{
			return (SByte)datas[rpos++];
		}
	
		public Int16 ReadInt16()
		{
			rpos += 2;
			return BitConverter.ToInt16(datas, rpos - 2);
		}
			
		public Int32 ReadInt32()
		{
			rpos += 4;
			return BitConverter.ToInt32(datas, rpos - 4);
		}
	
		public Int64 ReadInt64()
		{
			rpos += 8;
			return BitConverter.ToInt64(datas, rpos - 8);
		}
		
		public Byte ReadUint8()
		{
			return datas[rpos++];
		}
	
		public UInt16 ReadUint16()
		{
			rpos += 2;
			return BitConverter.ToUInt16(datas, rpos - 2);
		}

		public UInt32 ReadUint32()
		{
			rpos += 4;
			return BitConverter.ToUInt32(datas, rpos - 4);
		}
		
		public UInt64 ReadUint64()
		{
			rpos += 8;
			return BitConverter.ToUInt64(datas, rpos - 8);
		}
		
		public float ReadFloat()
		{
			rpos += 4;
			return BitConverter.ToSingle(datas, rpos - 4);
		}

		public double ReadDouble()
		{
			rpos += 8;
			return BitConverter.ToDouble(datas, rpos - 8);
		}
		
		public string ReadString()
		{
			int offset = rpos;
			while(datas[rpos++] != 0)
			{
			}
			return converter.GetString(datas, offset, rpos - offset - 1);
		}
	
		public byte[] ReadBlob()
		{
			UInt32 size = ReadUint32();
			byte[] buf = new byte[size];
			Array.Copy(datas, rpos, buf, 0, size);
			rpos += (int)size;
			return buf;
		}
	
		public Vector2 ReadPackXZ()
		{
			PackFloatXType xPackData;
			PackFloatXType zPackData;
			
			xPackData.fv = 0f;
			zPackData.fv = 0f;
			
			xPackData.uv = 0x40000000;
			zPackData.uv = 0x40000000;
		
			Byte v1 = ReadUint8();
			Byte v2 = ReadUint8();
			Byte v3 = ReadUint8();
			
			UInt32 data = 0;
			data |= ((UInt32)v1 << 16);
			data |= ((UInt32)v2 << 8);
			data |= (UInt32)v3;

			xPackData.uv |= (data & 0x7ff000) << 3;
			zPackData.uv |= (data & 0x0007ff) << 15;

			xPackData.fv -= 2.0f;
			zPackData.fv -= 2.0f;
		
			xPackData.uv |= (data & 0x800000) << 8;
			zPackData.uv |= (data & 0x000800) << 20;
		
			Vector2 vec = new Vector2(xPackData.fv, zPackData.fv);
			return vec;
		}
	
		public float ReadPackY()
		{
			PackFloatXType yPackData; 
			yPackData.fv = 0f;
			yPackData.uv = 0x40000000;

			UInt16 data = ReadUint16();

			yPackData.uv |= ((UInt32)data & 0x7fff) << 12;
			yPackData.fv -= 2f;
			yPackData.uv |= ((UInt32)data & 0x8000) << 16;

			return yPackData.fv;
		}
		
		//---------------------------------------------------------------------------------
		public void WriteInt8(SByte v)
		{
			datas[wpos++] = (Byte)v;
		}
	
		public void WriteInt16(Int16 v)
		{	
			WriteInt8((SByte)(v & 0xff));
			WriteInt8((SByte)(v >> 8 & 0xff));
		}
			
		public void WriteInt32(Int32 v)
		{
			for(int i=0; i<4; i++)
				WriteInt8((SByte)(v >> i * 8 & 0xff));
		}
	
		public void WriteInt64(Int64 v)
		{
			byte[] getdata = BitConverter.GetBytes(v);
			for(int i=0; i<getdata.Length; i++)
			{
				datas[wpos++] = getdata[i];
			}
		}
		
		public void WriteUint8(Byte v)
		{
			datas[wpos++] = v;
		}
	
		public void WriteUint16(UInt16 v)
		{
			WriteUint8((Byte)(v & 0xff));
			WriteUint8((Byte)(v >> 8 & 0xff));
		}
			
		public void WriteUint32(UInt32 v)
		{
			for(int i=0; i<4; i++)
				WriteUint8((Byte)(v >> i * 8 & 0xff));
		}
	
		public void WriteUint64(UInt64 v)
		{
			byte[] getdata = BitConverter.GetBytes(v);
			for(int i=0; i<getdata.Length; i++)
			{
				datas[wpos++] = getdata[i];
			}
		}
		
		public void WriteFloat(float v)
		{
			byte[] getdata = BitConverter.GetBytes(v);
			for(int i=0; i<getdata.Length; i++)
			{
				datas[wpos++] = getdata[i];
			}
		}
	
		public void WriteDouble(double v)
		{
			byte[] getdata = BitConverter.GetBytes(v);
			for(int i=0; i<getdata.Length; i++)
			{
				datas[wpos++] = getdata[i];
			}
		}
	
		public void WriteBlob(byte[] v)
		{
			UInt32 size = (UInt32)v.Length;
			if(size + 4 > Space())
			{
				Dbg.ERROR_MSG("memorystream::writeBlob: no free!");
				return;
			}
			
			WriteUint32(size);
		
			for(UInt32 i=0; i<size; i++)
			{
				datas[wpos++] = v[i];
			}
		}
		
		public void WriteString(string v)
		{
			if(v.Length > Space())
			{
				Dbg.ERROR_MSG("memorystream::writeString: no free!");
				return;
			}

			byte[] getdata = System.Text.Encoding.ASCII.GetBytes(v);
			for(int i=0; i<getdata.Length; i++)
			{
				datas[wpos++] = getdata[i];
			}
			
			datas[wpos++] = 0;
		}

		//---------------------------------------------------------------------------------
		public void Append(byte[] datas, UInt32 offset, UInt32 size)
		{
			UInt32 free = Space();
			if (free < size) {
				byte[] newdatas = new byte[datas.Length + size * 2]; 
				Array.Copy(datas, 0, newdatas, 0, wpos);
				datas = newdatas;
			}

			Array.Copy(datas, offset, datas, wpos, size);
			wpos += (int)size;
		}

		//---------------------------------------------------------------------------------
		public void ReadSkip(UInt32 v)
		{
			rpos += (int)v;
		}
		
		//---------------------------------------------------------------------------------
		public UInt32 Space()
		{
			return (UInt32)(Data().Length - wpos);
		}
	
		//---------------------------------------------------------------------------------
		public UInt32 Length()
		{
			return (UInt32)(wpos - rpos);
		}
	
		//---------------------------------------------------------------------------------
		public bool ReadEOF()
		{
			return (bufferMax - rpos) <= 0;
		}

		//---------------------------------------------------------------------------------
		public void Done()
		{
			rpos = wpos;
		}
		
		//---------------------------------------------------------------------------------
		public void Clear()
		{
			rpos = wpos = 0;
			if(datas.Length > bufferMax)
			   datas = new byte[bufferMax]; 
		}
		
		//---------------------------------------------------------------------------------
		public byte[] Getbuffer()
		{
			byte[] buf = new byte[Length()];
			Array.Copy(Data(), rpos, buf, 0, Length());
			return buf;
		}
		
		//---------------------------------------------------------------------------------
		public string toString()
		{
			string s = "";
			int ii = 0;
			byte[] buf = Getbuffer();
			
			for(int i=0; i<buf.Length; i++)
			{
				ii += 1;
				if(ii >= 200)
				{
					s = "";
					ii = 0;
				}
							
				s += buf[i];
				s += " ";
			}
			
			return s;
		}
    }
    
} 
