namespace ZSerializer
{
    using System;
    internal static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this long arg)
        {
            return BitConverter.GetBytes(arg);
        }

        internal static long ToLong(this byte[] buffer)
        {
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}