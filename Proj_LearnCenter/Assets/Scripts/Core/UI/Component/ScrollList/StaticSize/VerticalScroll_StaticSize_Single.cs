using UnityEngine;
using System.Collections;

public class VerticalScroll_StaticSize_Single : ScrollListBase
{
    public VerticalScrollDirection direction;
    public override void ScrollTo(int index)
    {
        throw new System.NotImplementedException();
    }

    protected override bool CheckItemShow(ScrollDataElement dataElem)
    {
        ///TODO


        float parentY = container.anchoredPosition.y;
        return Mathf.Abs(dataElem.pos.y + parentY) < viewPort.rect.height * 1.5f;
        
    }

    protected override void CalculateSize(int startIndex = 0)
    {
        base.CalculateSize();

        scrollElement.ReadSize();
        elemReuse = datas.Count * scrollElement.size.y > viewPort.rect.height * 2;

        float edge = 0;
        int flag = 1;
        if (direction == VerticalScrollDirection.TopToBottom)
        {
            flag = -1;
            edge = padding.top;
            container.pivot = new Vector2(0.5f, 1);
        }
        else if (direction == VerticalScrollDirection.BottomToTop)
        {
            flag = 1;
            edge = padding.bottom;
            container.pivot = new Vector2(0.5f, 0);
        }
        container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,viewPort.rect.width - padding.horizontal);
        container.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,datas.Count * scrollElement.size.y - padding.vertical);
        for (int i = 0, max = datas.Count; i < max; ++i)
        {
            datas[i].size = scrollElement.size;
            datas[i].pos = new Vector2(padding.left + scrollElement.size.x / 2, edge + scrollElement.size.y * (i + 0.5f) * flag);
            if (!elemReuse)
            {
                ScrollElement elem = PoolManager.GetObject(prefabStoreKey).GetComponent<ScrollElement>();
                if(null != onItemUpdate && null != elem)
                {
                    onItemUpdate(elem,i,datas[i]);
                }
            }
        }
    }

}
