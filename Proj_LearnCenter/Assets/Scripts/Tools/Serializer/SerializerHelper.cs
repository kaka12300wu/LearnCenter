namespace ZSerializer
{
    using System.IO;
    internal static class SerializerHelper
    {

        internal static bool IsReadOver(this BinaryReader reader)
        {
            return reader.BaseStream.Position >= reader.BaseStream.Length;
        }

    }
}
