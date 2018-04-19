namespace ZSerializer
{
    using System;
    internal static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this ulong arg)
        {
            return BitConverter.GetBytes(arg);
        }

        internal static ulong ToULong(this byte[] buffer)
        {
            return BitConverter.ToUInt64(buffer, 0);
        }
    }
}