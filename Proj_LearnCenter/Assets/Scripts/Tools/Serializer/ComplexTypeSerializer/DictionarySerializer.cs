namespace ZSerializer
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    internal static partial class ComplexSerializer
    {
        internal static byte[] DicToBytes(Object arg)
        {
            try
            {
                Type type = arg.GetType();
                MethodInfo enumrator = type.GetMethod("GetEnumerator", BindingFlags.Instance | BindingFlags.Public);
                object objEnum = enumrator.Invoke(arg, null);
                MethodInfo moveNext = objEnum.GetType().GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.Public);
                List<byte> list = new List<byte>();
                bool gotNext = (bool)moveNext.Invoke(objEnum, null);
                while (true)
                {
                    if (!gotNext)
                        break;

                    object current = objEnum.GetType().GetProperty("Current", BindingFlags.Instance | BindingFlags.Public).GetValue(objEnum, null);
                    object key = current.GetType().GetProperty("Key", BindingFlags.Instance | BindingFlags.Public).GetValue(current, null);
                    object value = current.GetType().GetProperty("Value", BindingFlags.Instance | BindingFlags.Public).GetValue(current, null);

                    byte[] bufferKey = Serializer.GetBytes(key);
                    byte[] bufferVal = Serializer.GetBytes(value);
                    if (bufferKey == default(byte[]) || bufferVal == default(byte[]))
                        continue;
                    list.AddRange(bufferKey);
                    list.AddRange(bufferVal);

                    gotNext = (bool)moveNext.Invoke(objEnum, null);
                }
                list.InsertRange(0, list.Count.ToBytes());
                return list.ToArray();
            }
            catch (Exception e)
            {
                GLog.LogError(e.ToString());                
            }
            return default(byte[]);
        }

        internal static Object BytesToDictionary(byte[] buffer, Type type)
        {
            Object dicRet = Activator.CreateInstance(type);
            MethodInfo addMethod = type.GetMethod("Add");
            ParameterInfo[] paramInfos = addMethod.GetParameters();
            Type keyType = paramInfos[0].ParameterType;
            Type valType = paramInfos[1].ParameterType;

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                BinaryReader reader = new BinaryReader(stream);
                try
                {
                    object[] paramArray = new object[2];
                    while (!reader.IsReadOver())
                    {
                        object key = default(object);
                        Serializer.Read(reader,keyType,ref key);
                        paramArray[0] = key;
                        object val = default(object);
                        Serializer.Read(reader, valType, ref val);
                        paramArray[1] = val;
                        addMethod.Invoke(dicRet, paramArray);
                    }
                }
                catch(Exception e)
                {
                    GLog.LogError(e.ToString());
                    throw new Exception("Failed to read Dictionary from buffer,key Type is " + keyType.ToString() + ",ValueType is " + valType.ToString());
                }
            }
            
            return dicRet;
        }

    }
}
