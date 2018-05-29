namespace ZSerializer
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    internal static partial class ComplexSerializer
    {
        static void GetPosition(int pos, int dimension, List<int> storedPos, byte[] dimensions, List<List<int>> ret)
        {
            List<int> dimesionPosList = new List<int>(storedPos.ToArray());
            dimesionPosList.Add(pos);
            if (dimension + 1 == dimensions.Length - 1)
            {
                for (int j = 0; j < dimensions[dimension + 1]; ++j)
                {
                    List<int> endPosList = new List<int>(dimesionPosList.ToArray());
                    endPosList.Add(j);
                    ret.Add(endPosList);
                }
            }
            else
            {
                for (int s = 0; s < dimensions[dimension + 1]; ++s)
                {
                    GetPosition(s, dimension + 1, dimesionPosList, dimensions, ret);
                }
            }
        }

        static List<List<int>> GetArrayPositions(Array arg,out byte[] dimensions)
        {
            List<List<int>> ret = new List<List<int>>();
            int rank = arg.Rank;
            dimensions = new byte[rank];
            for (int i = 0; i < rank; ++i)
            {
                dimensions[i] = (byte)arg.GetLength(i);
            }

            List<int> posList;
            for (int i = 0; i < dimensions[0]; ++i)
            {
                posList = new List<int>();
                GetPosition(i, 0, posList, dimensions, ret);
            }

            return ret;
        }

        public static byte[] ToBytes(this Array arg)
        {
            List<byte> list = new List<byte>();
            int rank = arg.Rank;
            list.AddRange(Serializer.GetBytes((byte)rank));
            if (rank == 1)
            {
                for (int i = 0, max = arg.Length; i < max; ++i)
                {
                    list.AddRange(Serializer.GetBytes(arg.GetValue(i)));
                }
            }
            else
            {
                byte[] dimensions;
                List<List<int>> keysList = GetArrayPositions(arg,out dimensions);
                list.AddRange(dimensions.ToBytes());
                foreach(List<int> l in keysList)
                {
                    list.AddRange(l.ToArray().ToBytes());
                    list.AddRange(Serializer.GetBytes(arg.GetValue(l.ToArray())));
                }
            }
            list.InsertRange(0,list.Count.ToBytes());
            return list.ToArray();
        }

        public static Array BytesToArray(byte[] buffer,Type elemType)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                BinaryReader reader = new BinaryReader(stream);
                Array arg;
                try
                {
                    int rank = reader.ReadByte();                    
                    if (rank == 1)
                    {
                        List<object> list = new List<object>();
                        while (!reader.IsReadOver())
                        {
                            object obj = default(object);
                            Serializer.Read(reader, elemType, ref obj);
                            list.Add(obj);
                        }
                        arg = Array.CreateInstance(elemType, list.Count);
                        for(int i = 0,max = list.Count;i<max;++i)
                        {
                            arg.SetValue(list[i], i);
                        }
                    }
                    else
                    {
                        object dimensionO = default(object);
                        Serializer.Read(reader, typeof(byte[]), ref dimensionO);
                        byte[] dimensions = (byte[])dimensionO;
                        long[] tempDimensions = new long[dimensions.Length];
                        for (int i = 0, max = dimensions.Length; i < max;++i)
                        {
                            tempDimensions[i] = (long)dimensions[i];
                        }
                        arg = Array.CreateInstance(elemType, tempDimensions);
                        while (!reader.IsReadOver())
                        {
                            object obj = default(object);
                            Serializer.Read(reader,typeof(int[]),ref obj);
                            int[] posIndex = (int[])obj;
                            obj = default(object);
                            Serializer.Read(reader, elemType, ref obj);
                            arg.SetValue(obj,posIndex);
                        }
                    }
                    return arg;
                }
                catch(Exception e)
                {
                    GLog.Log(e.ToString());
                }   
                finally
                {
                    reader.Close();
                }
                return null;
            }
        }
    }
}