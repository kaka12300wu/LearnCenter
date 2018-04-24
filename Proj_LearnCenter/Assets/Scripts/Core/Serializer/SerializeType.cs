﻿namespace ZSerializer
{
    using System;
    using System.Collections.Generic;
    public class SerializeType
    {
        public const byte st_error      = 0;
        public const byte st_byte       = 1;
        public const byte st_char       = 2;
        public const byte st_short      = 3;
        public const byte st_ushort     = 4;
        public const byte st_int        = 5;
        public const byte st_uint       = 6;
        public const byte st_long       = 7;
        public const byte st_ulong      = 8;
        public const byte st_float      = 9;
        public const byte st_double     = 10;
        public const byte st_string     = 11;
        public const byte st_bool       = 12;
        public const byte st_class      = 13;
        public const byte st_array      = 14;
        public const byte st_list       = 15;
        public const byte st_dictionary = 16;

        public static byte GetSerializeType(Type type)
        {
            TypeCode code = Type.GetTypeCode(type);
            switch (code)
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return st_byte;
                case TypeCode.Char:
                    return st_char;
                case TypeCode.Int16:
                    return st_short;
                case TypeCode.UInt16:
                    return st_ushort;
                case TypeCode.Int32:
                    return st_int;
                case TypeCode.UInt32:
                    return st_uint;
                case TypeCode.Int64:
                    return st_long;
                case TypeCode.UInt64:
                    return st_ulong;
                case TypeCode.Decimal:
                case TypeCode.Single:
                    return st_float;
                case TypeCode.Double:
                    return st_double;
                case TypeCode.String:
                    return st_string;
                case TypeCode.Boolean:
                    return st_bool;
            }

            if (type.IsArray)
                return st_array;

            string typeStr = type.ToString();
            if (typeStr.Contains("System.Collections.Generic.List"))
                return st_list;
            if (typeStr.Contains("System.Collections.Generic.Dictionary"))
                return st_dictionary;
            if (type.IsClass)
                return st_class;
            return st_error;
        }

        public static byte GetSerializeType(object obj)
        {
            if (null == obj)
                return st_error;
            return GetSerializeType(obj.GetType());            
        }
    }
}