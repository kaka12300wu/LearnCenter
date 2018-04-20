using System;
using UnityEngine;
using ZSerializer;
public class Entry : MonoBehaviour
{

	// Use this for initialization
	void Awake ()
	{
        GLog.Init();
        int[]  arg = new int[6]{2,4,6,8,10,12};
        byte[] buffer = Serializer.GetBytes(arg);
        object obj = default(object);
        Serializer.DeSerialize(buffer, typeof(int[]), ref obj);
        int[] deArg = obj as int[];
        foreach(int a in deArg)
        {
            Debug.Log(a);
        }
	}

    
}