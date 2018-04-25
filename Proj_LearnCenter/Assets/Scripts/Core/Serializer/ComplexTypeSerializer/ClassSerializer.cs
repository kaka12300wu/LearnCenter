namespace ZSerializer
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Collections.Generic;

    internal static partial class ComplexSerializer
    {
        static List<LeaguerInfo> GetObjectPropertyInfos(Object obj)
        {
            List<LeaguerInfo> list = new List<LeaguerInfo>();
            Type type = obj.GetType();
            FieldInfo[] fileds = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach(FieldInfo info in fileds)
            {
                object val = info.GetValue(obj);
                if (null == val)
                    continue;
                LeaguerInfo pro = new LeaguerInfo();
                pro.key = info.Name;
                pro.typeCode = SerializeType.GetSerializeType(val.GetType());
                pro.valBuffer = Serializer.GetBytes(val);
                list.Add(pro);
            }

            PropertyInfo[] props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach(PropertyInfo info in props)
            {
                object val = info.GetValue(obj, null);
                if (null == val)
                    continue;
                LeaguerInfo pro = new LeaguerInfo();
                pro.key = info.Name;                
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
            
            List<LeaguerInfo> list = GetObjectPropertyInfos(arg);
            List<byte> res = new List<byte>();
            foreach(LeaguerInfo info in list)
            {
                res.AddRange(info.key.ToKeyBytes());
                res.AddRange(info.typeCode.ToBytes());
                res.AddRange(info.valBuffer.Length.ToBytes());
                res.AddRange(info.valBuffer);
            }
            res.InsertRange(0,res.Count.ToBytes());
            return res.ToArray();
        }

        public static void BytesToClass(byte[] buffer,Type type,ref Object res)
        {
            byte typeCode = SerializeType.GetSerializeType(type);
            if(typeCode != SerializeType.st_class)
            {
                throw new Exception("Wrong call BytesToClass of:" + type.ToString());
            }

            using(MemoryStream stream = new MemoryStream(buffer))
            {
                try
                {
                    res = Activator.CreateInstance(type);
                    BinaryReader reader = new BinaryReader(stream);
                    Dictionary<string, LeaguerInfo> dicInfos = new Dictionary<string, LeaguerInfo>();
                    while(!reader.IsReadOver())
                    {
                        LeaguerInfo info = new LeaguerInfo();
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

                    FieldInfo[] fields = res.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                    foreach (FieldInfo field in fields)
                    {
                        if(dicInfos.ContainsKey(field.Name))
                        {
                            object obj = default(object);                           
                            Serializer.DeSerialize(dicInfos[field.Name].valBuffer, field.FieldType, ref obj);
                            field.SetValue(res, obj);
                        }                       
                    }

                    PropertyInfo[] props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    foreach (PropertyInfo info in props)
                    {
                        if (dicInfos.ContainsKey(info.Name))
                        {
                            object obj = default(object);
                            Serializer.DeSerialize(dicInfos[info.Name].valBuffer, info.PropertyType, ref obj);
                            info.SetValue(res, obj, null);
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

    internal class LeaguerInfo
    {
        public string key;
        public byte typeCode;
        public byte[] valBuffer;
    }
}