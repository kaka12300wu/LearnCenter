using UnityEngine;
using System.Collections;

public class HorizentalScroll_StaticSize_Single : ScrollListBase
{
    public HorizontalScrollDirection direction;
    public override void ScrollTo(int index)
    {
        throw new System.NotImplementedException();
    }

    protected override void CalculateSize(int startIndex = 0)
    {
        elemReuse = datas.Count > 30;
    }
}
