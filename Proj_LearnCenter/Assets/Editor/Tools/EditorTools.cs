using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class EditorTools
{
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        string path = AssetDatabase.GetAssetPath(instanceID);
        if (path.EndsWith(".prefab"))
        {
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (null != obj)
            {
                GameObject o = PrefabUtility.InstantiatePrefab(obj) as GameObject;
                o.transform.SetAsLastSibling();
                Selection.activeObject = o;
                return true;
            }
            return false;
        }
        return false;
    }
}