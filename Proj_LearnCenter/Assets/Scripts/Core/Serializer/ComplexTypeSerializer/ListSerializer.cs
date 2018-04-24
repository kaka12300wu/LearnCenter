namespace ZSerializer
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Collections.Generic;
    internal static partial class ComplexSerializer
    {
        internal static byte[] ListToBytes(Object obj)
        {
            Type type = obj.GetType();
            try
            {
                MethodInfo methodGetEnumerator = type.GetMethod("GetEnumerator");
                object enumrator = methodGetEnumerator.Invoke(obj,null);
                MethodInfo methodMoveNext = enumrator.GetType().GetMethod("MoveNext");
                bool gotNext = (bool)methodMoveNext.Invoke(enumrator,null);
                List<byte> list = new List<byte>();
                while(true)
                {
                    if (!gotNext)
                        break;
                    object arg = enumrator.GetType().GetProperty("Current", BindingFlags.Instance | BindingFlags.Public).GetValue(enumrator,null);
                    list.AddRange(Serializer.GetBytes(arg));
                    gotNext = (bool)methodMoveNext.Invoke(enumrator, null);
                }
                list.InsertRange(0,list.Count.ToBytes());
                return list.ToArray();                
            }
            catch(Exception e)
            {
                GLog.LogError(e.ToString());
            }
            return default(byte[]);
        }


        internal static Object BytesToList(byte[] buffer,Type type)
        {
            try
            {
                MethodInfo methodAdd = type.GetMethod("Add");
                ParameterInfo[] paramInfos = methodAdd.GetParameters();
                Type elemType = paramInfos[0].ParameterType;
                using(MemoryStream stream = new MemoryStream(buffer))
                {
                    BinaryReader reader = new BinaryReader(stream);
                    Object obj = Activator.CreateInstance(type);
                    object[] paramArray = new object[1];
                    while(!reader.IsReadOver())
                    {
                        object elem = default(object);
                        Serializer.Read(reader,elemType,ref elem);
                        paramArray[0] = elem;
                        methodAdd.Invoke(obj, paramArray);
                    }
                    return obj;
                }
            }
            catch (Exception e)
            {
                GLog.LogError(e.ToString());
            }
            return null;
        }       
    }
}
