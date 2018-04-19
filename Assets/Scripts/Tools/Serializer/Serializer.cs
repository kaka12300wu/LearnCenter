namespace ZSerializer
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Text;
    public class Serializer
    {
        public static byte[] GetBytes(Object arg)
        {
            byte typeCode = SerializeType.GetSerializeType(arg);
            switch(typeCode)
            {
                case SerializeType.st_bool:
                    {
                        return Convert.ToBoolean(arg).ToBytes();
                    }
                case SerializeType.st_byte:
                    {
                        return Convert.ToByte(arg).ToBytes();
                    }
                case SerializeType.st_char:
                    {
                        return Convert.ToChar(arg).ToBytes();
                    }
                case SerializeType.st_float:
                    {
                        return Convert.ToSingle(arg).ToBytes();
                    }
                case SerializeType.st_double:
                    {
                        return Convert.ToDouble(arg).ToBytes();
                    }
                case SerializeType.st_short:
                    {
                        return Convert.ToInt16(arg).ToBytes();
                    }
                case SerializeType.st_ushort:
                    {
                        return Convert.ToUInt16(arg).ToBytes();
                    }
                case SerializeType.st_int:
                    {
                        return Convert.ToInt32(arg).ToBytes();
                    }
                case SerializeType.st_uint:
                    {
                        return Convert.ToUInt32(arg).ToBytes();
                    }
                case SerializeType.st_long:
                    {
                        return Convert.ToInt64(arg).ToBytes();
                    }
                case SerializeType.st_ulong:
                    {
                        return Convert.ToUInt64(arg).ToBytes();
                    }
                case SerializeType.st_string:
                    {
                        return Convert.ToString(arg).ToBytes();
                    }
                case SerializeType.st_class:
                    {                        
                        return ComplexSerializer.ClassToBytes(arg);
                    }
                case SerializeType.st_array:
                    {
                        return ((Array)Convert.ChangeType(arg, typeof(Array))).ToBytes();
                    }
            }
            return default(byte[]);
        }

        public static byte[] GetBytes<T>(T[] arg)
        {
            return ComplexSerializer.ArrayToBytes<T>(arg);
        }

        public static T Read<T>(BinaryReader reader)
        {
            Type type = typeof(T);
            byte typeCode = SerializeType.GetSerializeType(type);
            switch (typeCode)
            {
                case SerializeType.st_bool:
                    {
                        return (T)Convert.ChangeType(reader.ReadBoolean(), type);
                    }
                case SerializeType.st_byte:
                    {
                        return (T)Convert.ChangeType(reader.ReadByte(), type);
                    }
                case SerializeType.st_char:
                    {
                        return (T)Convert.ChangeType(reader.ReadChar(), type);
                    }
                case SerializeType.st_float:
                    {
                        return (T)Convert.ChangeType(reader.ReadSingle(), type);
                    }
                case SerializeType.st_double:
                    {
                        return (T)Convert.ChangeType(reader.ReadDouble(), type);
                    }
                case SerializeType.st_short:
                    {
                        return (T)Convert.ChangeType(reader.ReadInt16(), type);
                    }
                case SerializeType.st_ushort:
                    {
                        return (T)Convert.ChangeType(reader.ReadUInt16(), type);
                    }
                case SerializeType.st_int:
                    {
                        return (T)Convert.ChangeType(reader.ReadInt32(), type);
                    }
                case SerializeType.st_uint:
                    {
                        return (T)Convert.ChangeType(reader.ReadUInt32(), type);
                    }
                case SerializeType.st_long:
                    {
                        return (T)Convert.ChangeType(reader.ReadInt64(), type);
                    }
                case SerializeType.st_ulong:
                    {
                        return (T)Convert.ChangeType(reader.ReadUInt64(), type);
                    }
                case SerializeType.st_string:
                    {
                        int length = reader.ReadInt32();
                        byte[] buffer = new byte[length];
                        reader.Read(buffer, 0, length);
                        return (T)Convert.ChangeType(buffer.ToUTF8String(), type);
                    }
                case SerializeType.st_class:
                    {
                        int size = reader.ReadInt32();
                        byte[] buffer = new byte[size];
                        reader.Read(buffer, 0, size);
                        List<byte> list = new List<byte>(size.ToBytes());
                        list.AddRange(buffer);
                        Object obj = Activator.CreateInstance(typeof(T));
                        ComplexSerializer.BytesToClass(list.ToArray(), ref obj);
                        return (T)obj;
                    }
            }
            return default(T);
        }

        public static T DeSerialize<T>(byte[] buffer)
        {
            Type type = typeof(T);
            object obj;
            DeSerialize(buffer, type, out obj);
            return (T)obj;
        }

        public static void DeSerialize(byte[] buffer,Type type,out object obj)
        {
            byte typeCode = SerializeType.GetSerializeType(type);
            switch (typeCode)
            {
                case SerializeType.st_bool:
                    {
                        obj = Convert.ChangeType(buffer.ToBoolean(), type);
                    }
                    break;
                case SerializeType.st_byte:
                    {
                        obj = Convert.ChangeType(buffer.ToByte(), type);
                    }
                    break;
                case SerializeType.st_char:
                    {
                        obj = Convert.ChangeType(buffer.ToChar(), type);
                    }
                    break;
                case SerializeType.st_float:
                    {
                        obj = Convert.ChangeType(buffer.ToFloat(), type);
                    }
                    break;
                case SerializeType.st_double:
                    {
                        obj = Convert.ChangeType(buffer.ToDouble(), type);
                    }
                    break;
                case SerializeType.st_short:
                    {
                        obj = Convert.ChangeType(buffer.ToShort(), type);
                    }
                    break;
                case SerializeType.st_ushort:
                    {
                        obj = Convert.ChangeType(buffer.ToUShort(), type);
                    }
                    break;
                case SerializeType.st_int:
                    {
                        obj = Convert.ChangeType(buffer.ToInt(), type);
                    }
                    break;
                case SerializeType.st_uint:
                    {
                        obj = Convert.ChangeType(buffer.ToUInt(), type);
                    }
                    break;
                case SerializeType.st_long:
                    {
                        obj = Convert.ChangeType(buffer.ToLong(), type);
                    }
                    break;
                case SerializeType.st_ulong:
                    {
                        obj = Convert.ChangeType(buffer.ToULong(), type);
                    }
                    break;
                case SerializeType.st_string:
                    {
                        obj = Convert.ChangeType(buffer.ToUTF8String(), type);
                    }
                    break;
                case SerializeType.st_class:
                    {
                        obj = Activator.CreateInstance(type);
                        ComplexSerializer.BytesToClass(buffer,ref obj);
                        break;
                    }
                default:
                    {
                        obj = Activator.CreateInstance(type);
                    }
                    break;
            }
        }
        
        public static T[] DeSerializeArray<T>(byte[] buffer)
        {
            return ComplexSerializer.BytesToArrat<T>(buffer);
        }
    }
}
