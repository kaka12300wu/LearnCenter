using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;

using Object = UnityEngine.Object;

public static class EditorHelper
{
    public static bool IsAsset(Object o)
    {
        return AssetDatabase.Contains(o);
    }

    public static bool IsHirachyGameObject(Object o)
    {
        return !IsAsset(o) && o.GetType() == typeof(GameObject);
    }

}
