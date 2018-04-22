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
                        Array obj = (Array)arg;
                        return obj.ToBytes();
                    }
                case SerializeType.st_dictionary:
                    {
                        return ComplexSerializer.DicToBytes(arg);
                    }
            }
            return default(byte[]);
        }

        public static void Read(BinaryReader reader,Type type,ref object obj)
        {
            byte typeCode = SerializeType.GetSerializeType(type);
            switch (typeCode)
            {
                case SerializeType.st_bool:
                    {
                        obj = Convert.ChangeType(reader.ReadBoolean(), type);
                    }
                    break;
                case SerializeType.st_byte:
                    {
                        obj = Convert.ChangeType(reader.ReadByte(), type);
                    }
                    break;
                case SerializeType.st_char:
                    {
                        obj = Convert.ChangeType(reader.ReadChar(), type);
                    } 
                    break;
                case SerializeType.st_float:
                    {
                        obj = Convert.ChangeType(reader.ReadSingle(), type);
                    } 
                    break;
                case SerializeType.st_double:
                    {
                        obj = Convert.ChangeType(reader.ReadDouble(), type);
                    } 
                    break;
                case SerializeType.st_short:
                    {
                        obj = Convert.ChangeType(reader.ReadInt16(), type);
                    } 
                    break;
                case SerializeType.st_ushort:
                    {
                        obj = Convert.ChangeType(reader.ReadUInt16(), type);
                    } 
                    break;
                case SerializeType.st_int:
                    {
                        obj = Convert.ChangeType(reader.ReadInt32(), type);
                    } 
                    break;
                case SerializeType.st_uint:
                    {
                        obj = Convert.ChangeType(reader.ReadUInt32(), type);
                    } 
                    break;
                case SerializeType.st_long:
                    {
                        obj = Convert.ChangeType(reader.ReadInt64(), type);
                    } 
                    break;
                case SerializeType.st_ulong:
                    {
                        obj = Convert.ChangeType(reader.ReadUInt64(), type);
                    } 
                    break;
                case SerializeType.st_string:
                    {
                        int length = reader.ReadInt32();
                        byte[] buffer = new byte[length];
                        reader.Read(buffer, 0, length);
                        obj = Convert.ChangeType(buffer.ToUTF8String(), type);
                    } 
                    break;
                case SerializeType.st_class:
                    {
                        int size = reader.ReadInt32();
                        byte[] buffer = new byte[size];
                        reader.Read(buffer, 0, size);
                        obj = Activator.CreateInstance(type);
                        ComplexSerializer.BytesToClass(buffer,type,ref obj);
                    } 
                    break;
                case SerializeType.st_array:
                    {
                        int length = reader.ReadInt32();
                        byte[] buffer = new byte[length];
                        reader.Read(buffer,0,length);                        
                        obj = ComplexSerializer.BytesToArray(buffer,type.GetElementType());
                    }
                    break;
            }
        }

        public static T DeSerialize<T>(byte[] buffer)
        {
            Type type = typeof(T);
            object obj = default(object);
            DeSerialize(buffer, type, ref obj);
            return (T)Convert.ChangeType(obj,type);
        }

        public static void DeSerialize(byte[] buffer,Type type,ref object obj)
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
                        byte[] realBuffer = new byte[buffer.Length - sizeof(int)];
                        Array.Copy(buffer, sizeof(int), realBuffer, 0, realBuffer.Length);
                        ComplexSerializer.BytesToClass(realBuffer, type, ref obj);
                    }
                    break;
                case SerializeType.st_array:
                    {
                        byte[] realBuffer = new byte[buffer.Length - sizeof(int)];
                        Array.Copy(buffer,sizeof(int),realBuffer,0,realBuffer.Length);
                        obj = ComplexSerializer.BytesToArray(realBuffer,type.GetElementType());
                    }
                    break;
            }
        }        
    }
}
