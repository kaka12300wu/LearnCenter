using System;
using UnityEngine;
using ZSerializer;
public class Entry : MonoBehaviour
{

	// Use this for initialization
	void Awake ()
	{
        GLog.Init();
        bool[,] arr = new bool[3,2];
        arr.Initialize();
        arr.ToBytes();
        
	}

    
}