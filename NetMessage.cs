using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetAzy
{
    public class RcvNetMessage
    {
        private byte[] bytes;
        private int messageLength;
        private int index = 0;

        public RcvNetMessage(byte[] bytes, int length)
        {
            this.bytes = bytes;
            messageLength = length;
        }

        public int ReadInt32()
        {
            Int32 Int = BitConverter.ToInt32(bytes, index);
            return Int;
        }

        public string ReadString()
        {
            int lenght = BitConverter.ToInt16(bytes, index - 2);
            string str = "";
            index += 2;
            for (int i = 0; i >= lenght; i++)
            {
                str += BitConverter.ToChar(bytes, index);
                index++;
            }
            return str;
        }

        public bool ReadBool()
        {
            return false;
        }

        public short ReadShort()
        {
            return 0;
        }
    }

    public class SndNetMessage
    {
        protected List<byte> bytes = new List<byte>();

        public int Lenght
        {
            get { return bytes.Count; }
        }

        public byte[] Bytes
        {
            get { return bytes.ToArray(); }
        }

        public void WriteInt32(Int32 number)
        {
            byte[] intBytes = BitConverter.GetBytes(number);
            foreach (byte b in intBytes)
                bytes.Add(b);
        }
    }
}
