using System;
using UnityEngine;
using ZSerializer;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class Entry : MonoBehaviour
{

	// Use this for initialization
	void Awake ()
	{
        GLog.Init();
        ZVector3[, ,] arg = new ZVector3[3, 3, 3];

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                for (int s = 0; s < 3; ++s)
                {
                    arg[i, j, s] = new ZVector3(Random.Range(i, (i + 5) * 15), Random.Range(j, (j + 5) * 15), Random.Range(i + j, (i + j + 5) * 15));
                }
            }
        }
        byte[] buffer = Serializer.GetBytes(arg);

        //SingletonObject.getInstance<FileHelper>().SaveFile(Application.persistentDataPath + "/Tmp/Serialization/27ZVector3.dno", buffer);
        //buffer = SingletonObject.getInstance<FileHelper>().ReadFile(Application.persistentDataPath + "/Tmp/Serialization/27ZVector3.dno");
        
        GLog.Log(buffer.Length.ToString());
        ZVector3[, ,] deArg = Serializer.DeSerialize<ZVector3[, ,]>(buffer);
        int leng1 = deArg.GetLength(0);
        int leng2 = deArg.GetLength(1);
        int leng3 = deArg.GetLength(2);
        for (int i = 0; i < leng1; ++i)
        {
            for (int j = 0; j < leng2; ++j)
            {
                for (int s = 0; s < leng3; ++s)
                {
                    GLog.Log(string.Format("[{0},{1},{2}]={3}", i, j, s, deArg[i, j, s].ToString()));
                }
            }
        }
	}

    
}