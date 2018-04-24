using ProtoBuf;
using System;
using System.IO;

public class ProtoSerializer
{
    public static byte[] ProtoSerialize(Object obj)
    {
        byte[] buffer;
        using (MemoryStream stream = new MemoryStream())
        {
            Serializer.Serialize(stream, obj);
            buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
        };
        return buffer;
    }

    public static T ProtoDeSerialize<T>(byte[] buffer)
    {
        T obj;
        using (MemoryStream stream = new MemoryStream(buffer))
        {
            stream.Position = 0;
            obj = Serializer.Deserialize<T>(stream);
        };
        return obj;
    }
}
