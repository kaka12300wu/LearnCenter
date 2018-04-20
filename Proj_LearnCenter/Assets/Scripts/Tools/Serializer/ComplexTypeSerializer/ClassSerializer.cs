namespace ZSerializer
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Collections.Generic;

    internal static partial class ComplexSerializer
    {
        static List<PropertyInfo> GetObjectPropertyInfos(Object obj)
        {
            List<PropertyInfo> list = new List<PropertyInfo>();
            Type type = obj.GetType();
            FieldInfo[] fileds = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach(FieldInfo info in fileds)
            {
                PropertyInfo pro = new PropertyInfo();
                pro.key = info.Name;
                object val = info.GetValue(obj);
                pro.typeCode = SerializeType.GetSerializeType(val.GetType());
                pro.valBuffer = Serializer.GetBytes(val);
                list.Add(pro);
            }

            return list;
        }

        public static byte[] ClassToBytes(Object arg)
        {
            if (SerializeType.st_class != SerializeType.GetSerializeType(arg))
            {
                throw new Exception("ClassToBytes must get arguments with typeof class!");
            }
            List<PropertyInfo> list = GetObjectPropertyInfos(arg);
            List<byte> res = new List<byte>();
            foreach(PropertyInfo info in list)
            {
                res.AddRange(info.key.ToKeyBytes());
                res.AddRange(info.typeCode.ToBytes());
                res.AddRange(info.valBuffer.Length.ToBytes());
                res.AddRange(info.valBuffer);
            }
            res.InsertRange(0,res.Count.ToBytes());
            return res.ToArray();
        }

        internal static bool IsReadOver(this BinaryReader reader)
        {
            return reader.BaseStream.Position >= reader.BaseStream.Length;
        }

        public static void BytesToClass(byte[] buffer,ref Object res)
        {
            byte typeCode = SerializeType.GetSerializeType(res.GetType());
            if(typeCode != SerializeType.st_class)
            {
                throw new Exception("Wrong call BytesToClass of:" + res.GetType().ToString());
            }

            using(MemoryStream stream = new MemoryStream(buffer))
            {
                try
                {
                    BinaryReader reader = new BinaryReader(stream);
                    Dictionary<string, PropertyInfo> dicInfos = new Dictionary<string, PropertyInfo>();
                    while(!reader.IsReadOver())
                    {
                        PropertyInfo info = new PropertyInfo();
                        object keyLength = 0;
                        Serializer.Read(reader, typeof(byte), ref keyLength);
                        byte[] keyBuffer = new byte[(byte)keyLength];
                        reader.Read(keyBuffer, 0, (byte)keyLength);
                        info.key = keyBuffer.ToUTF8String();
                        object typeCodeO = SerializeType.st_error;
                        Serializer.Read(reader,typeof(byte),ref typeCodeO);
                        info.typeCode = (byte)typeCodeO;
                        object valLengthO = 0;
                        Serializer.Read(reader,typeof(int),ref valLengthO);
                        int valLength = (int)valLengthO;
                        info.valBuffer = new byte[valLength];
                        reader.Read(info.valBuffer,0,valLength);
                        dicInfos.Add(info.key,info);
                    }

                    FieldInfo[] fileds = res.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                    foreach (FieldInfo field in fileds)
                    {
                        if(dicInfos.ContainsKey(field.Name))
                        {
                            object obj = Activator.CreateInstance(field.FieldType);
                            Serializer.DeSerialize(dicInfos[field.Name].valBuffer, field.FieldType, ref obj);
                            field.SetValue(res, obj);
                        }                       
                    }
                }
                catch (Exception e)
                {
                    GLog.LogError(e.ToString());
                }
            };
        }
    }

    internal class PropertyInfo
    {
        public string key;
        public byte typeCode;
        public byte[] valBuffer;
    }
}