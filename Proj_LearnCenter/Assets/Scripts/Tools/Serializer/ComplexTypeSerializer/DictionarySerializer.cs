namespace ZSerializer
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    internal static partial class ComplexSerializer
    {

        static Type GetPropertyTypeWithKey(Object arg,string key)
        {
            Type type = arg.GetType();
            PropertyInfo propKey = type.GetProperty(key, BindingFlags.Instance | BindingFlags.Public);
            Object valProp = propKey.GetValue(arg, null);
            Type typeProp = valProp.GetType();
            MethodInfo minfo = typeProp.GetMethod("GetEnumerator", BindingFlags.Instance | BindingFlags.Public);
            object enumerator = minfo.Invoke(typeProp, null);
            enumerator.GetType().GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.Public).Invoke(enumerator, null);
            object ero = enumerator.GetType().GetProperty("Current", BindingFlags.Instance | BindingFlags.Public).GetValue(enumerator, null);
            return ero.GetType();
        }

        internal static byte[] DicToBytes(Object arg)
        {
            try
            {
                Type key = GetPropertyTypeWithKey(arg, "Keys");
                Type value = GetPropertyTypeWithKey(arg, "Values");
                
            }
            catch(Exception e)
            {
                GLog.LogError(e.ToString());
            }
            return default(byte[]);
        }

    }
}
