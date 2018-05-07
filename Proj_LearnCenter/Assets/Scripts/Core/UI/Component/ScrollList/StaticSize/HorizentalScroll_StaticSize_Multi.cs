using UnityEngine;
using System.Collections;

public class HorizentalScroll_StaticSize_Multi : ScrollListBase
{
    public int lineCount;
    public HorizontalScrollDirection direction;
    public override void ScrollTo(int index)
    {
        throw new System.NotImplementedException();
    }

    protected override void CalculateSize(int startIndex = 0)
    {
        lineCount = (short)Mathf.Max(lineCount, 2);
        elemReuse = datas.Count / lineCount > 30;
    }
}
