using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WoWTools.WDBUpdater
{
    // from tauriparser
    public class Bit
    {
        public bool m_value { get; set; }

        public Bit()
        {
            m_value = false;
        }

        public static explicit operator bool(Bit x)
        {
            return x.m_value == true;
        }
    }

    public class ByteBuffer
    {
        public int Rpos { get; private set; }
        private int m_bitpos;
        private byte m_curbitval;
        public List<byte> Storage { get; private set; }

        public ByteBuffer()
        {
            ResetBits();
            Rpos = 0;
            Storage = new List<byte>();
        }

        public ByteBuffer(uint reserve)
        {
            ResetBits();
            Rpos = 0;
            Storage = new List<byte>((int)reserve);
        }

        public ByteBuffer(ByteBuffer buffer)
        {
            ResetBits();
            Rpos = buffer.Rpos;
            Storage = buffer.Storage;
            buffer.Storage = new List<byte>();
        }

        public ByteBuffer(byte[] data)
        {
            ResetBits();
            Rpos = 0;
            Storage = new List<byte>();
            Storage.AddRange(data);
        }

        public void Reset(byte[] data)
        {
            ResetBits();
            Rpos = 0;
            Storage.AddRange(data);
        }

        public byte[] Ptr() { return Storage.ToArray(); }
        public int Size() { return Storage.Count(); }

        public void Clear()
        {
            Rpos = 0;
            Storage.Clear();
        }

        public bool ReadBit()
        {
            ++m_bitpos;
            if (m_bitpos > 7)
            {
                m_bitpos = 0;
                m_curbitval = Storage[Rpos++];
            }

            bool bit = ((m_curbitval >> (7 - m_bitpos)) & 1) != 0;
            return bit;
        }

        public uint ReadBits(int bits)
        {
            uint value = 0;

            for (int i = bits - 1; ; --i)
            {
                if (ReadBit())
                    value |= (uint)(1 << i);
                if (i == 0)
                    break;
            }

            return value;
        }

        public void ResetBits()
        {
            if (m_bitpos == 8)
                return;

            m_curbitval = 0;
            m_bitpos = 8;
        }

        public UInt32 ReadUInt32()
        {
            ResetBits();
            byte[] data = Storage.GetRange(Rpos, 4).ToArray();
            Rpos += 4;
            return BitConverter.ToUInt32(data);
        }

        public UInt64 ReadUInt64()
        {
            ResetBits();
            byte[] data = Storage.GetRange(Rpos, 8).ToArray();
            Rpos += 8;
            return BitConverter.ToUInt64(data);
        }

        public Int64 ReadInt64()
        {
            ResetBits();
            byte[] data = Storage.GetRange(Rpos, 8).ToArray();
            Rpos += 8;
            return BitConverter.ToInt64(data);
        }

        public Int32 ReadInt32()
        {
            ResetBits();
            byte[] data = Storage.GetRange(Rpos, 4).ToArray();
            Rpos += 4;
            return BitConverter.ToInt32(data);
        }

        public Single ReadSingle()
        {
            ResetBits();
            byte[] data = Storage.GetRange(Rpos, 4).ToArray();
            Rpos += 4;
            return BitConverter.ToSingle(data);
        }

        public byte ReadByte()
        {
            ResetBits();
            return Storage[Rpos++];
        }

        public sbyte ReadSByte()
        {
            ResetBits();
            return unchecked((sbyte)Storage[Rpos++]);
        }

        public char ReadChar()
        {
            ResetBits();
            return unchecked((char)Storage[Rpos++]);
        }

        public string ReadString(uint countChar)
        {
            ResetBits();
            string value = "";
            if (countChar == 0)
                return value;

            char c = (char)0;
            while (Rpos < Size() && (countChar-- > 0))
            {
                c = ReadChar();
                if (c == 0)
                    break;
                value += c;
            }

            return value;
        }

        public string ReadString()
        {
            ResetBits();
            string value = "";
            char c = (char)0;
            while (Rpos < Size())
            {
                c = ReadChar();
                if (c == 0)
                    break;
                value += c;
            }

            return value;
        }
    }
}
