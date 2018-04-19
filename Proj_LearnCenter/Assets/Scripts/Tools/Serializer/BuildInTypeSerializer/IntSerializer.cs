namespace ZSerializer
{
    using System;
    internal static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this int arg)
        {
            return BitConverter.GetBytes(arg);
        }

        internal static int ToInt(this byte[] buffer)
        {
            return BitConverter.ToInt32(buffer, 0);
        }
    }
}