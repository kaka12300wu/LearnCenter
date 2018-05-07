using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class ExtentionMethods
{
    public static void AddClickEvent(this Button btn,UnityAction clickFunc)
    {        
        btn.onClick.RemoveListener(clickFunc);
        btn.onClick.AddListener(clickFunc);
    }


}
