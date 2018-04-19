namespace ZSerializer
{
    using System;
    internal static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this float arg)
        {
            return BitConverter.GetBytes(arg);
        }

        internal static byte[] ToBytes(this decimal arg)
        {
            return ((float)arg).ToBytes();
        }

        internal static float ToFloat(this byte[] buffer)
        {
            return BitConverter.ToSingle(buffer, 0);
        }
    }
}