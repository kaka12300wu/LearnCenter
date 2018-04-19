namespace ZSerializer
{
    using System;
    internal static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this double arg)
        {
            return BitConverter.GetBytes(arg);
        }

        internal static double ToDouble(this byte[] buffer)
        {
            return BitConverter.ToDouble(buffer, 0);
        }
    }
}