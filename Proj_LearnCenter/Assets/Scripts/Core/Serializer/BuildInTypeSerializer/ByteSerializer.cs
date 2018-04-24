namespace ZSerializer
{
    using System;
    using System.IO;
    static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this byte arg)
        {
            return new byte[1] { arg };
        }

        internal static byte ToByte(this byte[] buffer)
        {
            return buffer[0];
        }
    }
}
