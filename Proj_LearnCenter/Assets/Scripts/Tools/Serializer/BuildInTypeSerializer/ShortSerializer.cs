namespace ZSerializer
{
    using System;
    internal static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this short arg)
        {
            return BitConverter.GetBytes(arg);
        }

        internal static short ToShort(this byte[] buffer)
        {
            return BitConverter.ToInt16(buffer, 0);
        }
    }
}