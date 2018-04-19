namespace ZSerializer
{
    using System;
    internal static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this uint arg)
        {
            return BitConverter.GetBytes(arg);
        }

        internal static uint ToUInt(this byte[] buffer)
        {
            return BitConverter.ToUInt32(buffer, 0);
        }
    }
}