namespace ZSerializer
{
    using System;
    using System.IO;

    static partial class BuildInTypeSerializer
    {
        internal static byte[] ToBytes(this bool arg)
        {
            return BitConverter.GetBytes(arg);
        }

        internal static bool ToBoolean(this byte[] buffer)
        {
            return BitConverter.ToBoolean(buffer, 0);
        }
    }
}