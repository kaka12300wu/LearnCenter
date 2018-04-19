namespace ZSerializer
{
    using System;
    using System.IO;
    internal static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this char arg)
        {
            return BitConverter.GetBytes(arg); ;
        }

        internal static char ToChar(this byte[] buffer)
        {
            return BitConverter.ToChar(buffer, 0); ;
        }
    }
}
