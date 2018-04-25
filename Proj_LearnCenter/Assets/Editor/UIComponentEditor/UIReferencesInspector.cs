using UnityEngine;
using UnityEditor;

using System.Collections.Generic;


[CustomEditor(typeof(UIReferences))]
public class UIReferencesInspector : Editor
{
    private UIReferences refers;
             
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();        
        EditorGUILayout.LabelField("UserFloat",GUILayout.Width(100));
        refers.userFloat = EditorGUILayout.FloatField(refers.userFloat);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("UserInt",GUILayout.Width(100));
        refers.userInt = EditorGUILayout.IntField(refers.userInt);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
               
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Monos:");
        GUILayout.FlexibleSpace();
        int countElem = 0;
        if (null != refers.monos)
            countElem = refers.monos.Count;
        EditorGUILayout.LabelField(countElem.ToString());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (null != refers.monos)
        {
            if (null != refers.names)
            {
                for (int i = refers.names.Count, min = refers.monos.Count; i > min; --i)
                {
                    refers.names.RemoveAt(i);
                }
            }
            else
                refers.names = new List<string>();

            if(null != refers.names && refers.names.Count < refers.monos.Count)
            {
                for(int i = refers.names.Count,max = refers.monos.Count;i<max;++i)
                {
                    refers.names.Add(refers.monos[i].name);
                }
            }

            for (int i = 0, max = refers.monos.Count; i < max; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField((i + 1).ToString(), GUILayout.Width(20));
                Object obj = refers.monos[i];
                if (null != obj)
                {
                    refers.names[i] = EditorGUILayout.TextField(refers.names[i]);
                    List<Object> comps = GetAllComponents(obj);
                    string[] allComps = ConverComponentsToStringArray(comps);
                    if (null != allComps)
                    {
                        int selectIndex = 0;
                        for (int j = 0, maxj = comps.Count; j < maxj; ++j)
                        {
                            if (comps[j] == refers.monos[i])
                            {
                                selectIndex = j;
                                break;
                            }
                        }
                        selectIndex = EditorGUILayout.Popup(selectIndex, allComps);
                        refers.monos[i] = comps[selectIndex];                        
                    }

                }
                refers.monos[i] = EditorGUILayout.ObjectField(refers.monos[i], typeof(Object), true);
                if (null != refers.monos[i] && string.IsNullOrEmpty(refers.names[i]))
                    refers.names[i] = refers.monos[i].name;
                if (null != refers.monos[i] && refers.names[i] != refers.monos[i].name)
                {
                    if(GUILayout.Button("Auto"))
                    {
                        if(null != obj)
                            refers.names[i] = obj.name;
                    }
                }

                if (GUILayout.Button("Del"))
                {
                    refers.monos.RemoveAt(i);
                    refers.names.RemoveAt(i);
                    max = refers.monos.Count;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }
        
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.Space ();
        if (GUILayout.Button("AddItem", GUILayout.Width(150)))
        {
            if (null == refers.monos)
                refers.monos = new List<Object>();
            refers.monos.Add(null);
            if (null == refers.names)
                refers.names = new List<string>();
            refers.names.Add("");
        }
		EditorGUILayout.Space ();
		EditorGUILayout.EndHorizontal ();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    void OnEnable()
    {
        refers = (UIReferences)target;
    }

    private List<Object> GetAllComponents(Object obj)
    {
        if (null == obj)
            return null;
        GameObject go = obj as GameObject;
        if (null == go)
            go = (obj as Component).gameObject;
        Component[] comps = go.GetComponents<Component>();
        List<Object> list = new List<Object>();
        list.Add(go);
        for (int i = 0, max = comps.Length; i < max; ++i)
        {
			if(comps[i].GetType() != typeof(CanvasRenderer) && comps[i] != refers)
                list.Add(comps[i]);
        }
        return list;
    }

    private string[] ConverComponentsToStringArray(List<Object> comps)
    {
        if (null == comps) return null;
        string[] strs = new string[comps.Count];
        for (int i = 0, max = comps.Count; i < max; ++i)
        {
			strs[i] = comps[i].GetType().Name;           
        }
        return strs;
    }



    
}
