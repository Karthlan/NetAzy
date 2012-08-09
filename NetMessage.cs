using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetAzy
{
    public class IncomingNetMessage
    {
        protected byte[] bytes;
        protected int index = 0;

        public int Index
        {
            get { return index; }
        }

        public int Lenght
        {
            get { return bytes.Count(); }
        }

        public byte[] Bytes
        {
            get { return bytes; }
        }

        public IncomingNetMessage(byte[] bytes)
        {
            this.bytes = bytes;
        }

        #region Readers

        protected void TestLenght()
        {
            if (index >= Lenght)
                throw new Exception("Reached end of NetworkStream");
        }

        public Int32 ReadInt32()
        {
            TestLenght();
            Int32 Int = BitConverter.ToInt32(bytes, index);
            index += 2;
            return Int;

        }

        public Int16 ReadInt16()
        {
            TestLenght();
            Int16 Int = BitConverter.ToInt16(bytes, index);
            index += 2;
            return Int;
        }

        public string ReadString()
        {
            TestLenght();
            int lenght = ReadInt16();
            string str = "";
            for (int i = 0; i < lenght; i++)
            {
                str += BitConverter.ToChar(bytes, index);
                index += 2;
            }
            return str;
        }

        public bool ReadBool()
        {
            TestLenght();
            return false;
        }
        #endregion

    }

    public class OutgoingNetMessage
    {
        protected List<byte> bytes = new List<byte>();

        public int Size
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

        public void WriteInt16(Int16 number)
        {
            byte[] intBytes = BitConverter.GetBytes(number);
            foreach (byte b in intBytes)
                bytes.Add(b);
        }

        public void WriteChar(char c)
        {
            byte[] charBytes = BitConverter.GetBytes(c);
            foreach (byte b in charBytes)
                bytes.Add(b);
        }

        public void WriteString(string str)
        {
            char[] chars = str.ToCharArray();

            WriteInt16((Int16)str.Count());
            foreach (char c in chars)
                WriteChar(c);
        }
    }
}
