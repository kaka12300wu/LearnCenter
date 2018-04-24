namespace ZSerializer
{
    using System;
    internal static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this ushort arg)
        {
            return BitConverter.GetBytes(arg);
        }

        internal static ushort ToUShort(this byte[] buffer)
        {
            return BitConverter.ToUInt16(buffer, 0);
        }
    }
}