using UnityEngine;
using System.Collections;

public class VerticalScroll_StaticSize_Multi : ScrollListBase
{
    public short lineCount;
    public VerticalScrollDirection direction;
    public override void ScrollTo(int index)
    {
        throw new System.NotImplementedException();
    }

    protected override void CalculateSize(int startIndex = 0)
    {
        lineCount = (short)Mathf.Max(lineCount,2);
        elemReuse = datas.Count / lineCount > 30;
    }
}
