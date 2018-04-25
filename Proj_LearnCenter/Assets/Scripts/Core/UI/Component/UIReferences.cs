using System.Collections.Generic;
using UnityEngine;

public class UIReferences : MonoBehaviour
{
    public List<Object> monos;

    public List<string> names;

    public float userFloat;

    public int userInt;

    public object userObject;

    public Object Get(int a_index)
    {
        a_index -= 1;
        if(a_index < 0 || a_index >= monos.Count)
        {
            GLog.LogError("index out of Range! got null reference!:" + name);
            return null;
        }
        return monos[a_index];
    }

    public Object Get(string a_name)
    {
        if(names.Contains(a_name))
        {
            return Get(names.IndexOf(a_name) + 1);
        }
        GLog.LogError(a_name + " does not exists in UIRefrence namelist!:" + name);
        return null;
    }

    public int Length
    {
        get { return monos.Count; }
    }
}

