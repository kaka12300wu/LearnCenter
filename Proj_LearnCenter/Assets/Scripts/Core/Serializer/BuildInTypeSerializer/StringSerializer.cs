namespace ZSerializer
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    internal static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this string arg)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(arg);
            byte[] lenBuffer = buffer.Length.ToBytes();
            List<byte> list = new List<byte>();
            list.AddRange(lenBuffer);
            list.AddRange(buffer);
            return list.ToArray();
        }

        internal static byte[] ToKeyBytes(this string key)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(key);
            byte length = (byte)buffer.Length;
            List<byte> list = new List<byte>();
            list.Add(length);
            list.AddRange(buffer);
            return list.ToArray();
        }

        internal static string ToUTF8String(this byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer);
        }
    }
}