using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanelManager : MonoBehaviour
{
    List<TextAnchor> canLayoutCorners;
    int nowIndex = 0;
    public LayoutGroup layout;

    private void Awake()
    {
        canLayoutCorners = new List<TextAnchor>();
        canLayoutCorners.Add(TextAnchor.UpperLeft);
        canLayoutCorners.Add(TextAnchor.UpperRight);
        canLayoutCorners.Add(TextAnchor.LowerLeft);
        canLayoutCorners.Add(TextAnchor.LowerRight);
    }


    public void SwitchPosition()
    {
        ++nowIndex;
        if (nowIndex >= canLayoutCorners.Count)
            nowIndex = 0;
        layout.childAlignment = canLayoutCorners[nowIndex];
    }
}
