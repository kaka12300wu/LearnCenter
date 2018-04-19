namespace ZSerializer
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    internal static partial class ComplexSerializer
    {
        public static byte[] ToBytes(this Array arg)
        {
            int rank = arg.Rank;
            List<List<int>> keysList = new List<List<int>>();
            
            Action<int> doAddDimension = (leng) =>
            {
                int pos = 0;
                for(int i = 0,max = keysList.Count;i<max;++i)
                {
                    if (pos >= leng)
                        pos++;
                    keysList[i].Add(pos);
                }
            };


            for (int i = 0, max = arg.Length; i < max; ++i)
            {
                keysList.Add(new List<int>());
            }

            for (int i = 0; i < rank;++i)
            {
                int dimensionLeng = arg.GetLength(i);
                for (int j = 0; j < dimensionLeng;++j)
                {
                    doAddDimension(dimensionLeng);
                }                    
            }

            System.Text.StringBuilder sbuilder = new System.Text.StringBuilder();
            foreach(List<int> list in keysList)
            {
                sbuilder.Remove(0, sbuilder.Length);
                for(int i = 0,max = list.Count;i<max;++i)
                {
                    sbuilder.Append(i + "-"); 
                }
                sbuilder.Remove(sbuilder.Length - 1,1);
                GLog.Log(sbuilder.ToString());
            }

            return default(byte[]);
        }
        
        public static byte[] ArrayToBytes<T>(T[] arg)
        {
            byte typeCode = SerializeType.GetSerializeType(typeof(T));
            if(typeCode == SerializeType.st_array)
            {
                throw new Exception("Can not serialize multi dimensional array");
            }

            List<byte> list = new List<byte>();
            bool isSimple = SerializeType.IsSimple(typeof(T));
            for(int i = 0,max = arg.Length;i<max;++i)
            {
                if (isSimple)
                {
                    list.AddRange(Serializer.GetBytes(arg[i]));
                }
                else if (typeCode == SerializeType.st_class)
                {
                    list.AddRange(ClassToBytes(arg[i]));
                }
            }
            return list.ToArray();
        }

        public static T[] BytesToArrat<T>(byte[] buffer)
        {
            List<T> list = new List<T>();
            byte typeCode = SerializeType.GetSerializeType(typeof(T));            
            using(MemoryStream stream = new MemoryStream(buffer))
            {
                BinaryReader reader = new BinaryReader(stream);
                if(SerializeType.IsSimple(typeof(T)) || typeCode == SerializeType.st_class)
                {
                    try
                    {
                        T obj = Serializer.Read<T>(reader);
                        while (true)
                        {
                            list.Add(obj);
                            if (reader.IsReadOver())
                                break;
                            obj = Serializer.Read<T>(reader);
                        }
                    }
                    catch(Exception e)
                    {
                        GLog.LogError(e.ToString());
                    }
                }
                else
                {

                }
                reader.Close();
            };
            return list.ToArray();
        }
    }
}