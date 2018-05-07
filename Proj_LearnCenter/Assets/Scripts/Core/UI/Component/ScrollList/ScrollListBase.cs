using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum VerticalScrollDirection
{
    TopToBottom,
    BottomToTop
}

public enum HorizontalScrollDirection
{
    LeftToRight,
    RightToLeft
}

public abstract class ScrollListBase : MonoBehaviour
{
    public ScrollRect scroll;

    public ScrollElement scrollElement;

    public int curFirstIndex = 0;

    protected List<ScrollDataElement> datas;

    protected bool centerChild = false;

    protected bool elemReuse = false;
   
    public virtual void SetData(IEnumerable incomeDatas)
    {
        if (null == datas)
            datas = new List<ScrollDataElement>();
        else
            datas.Clear();
        IEnumerator enumerator = incomeDatas.GetEnumerator();
        while(enumerator.MoveNext())
        {
            ScrollDataElement elem = new ScrollDataElement(){data = enumerator.Current };
            datas.Add(elem);
        }
        
        CalculateSize();
        if (elemReuse)
        {
            ScrollListElemReuse reuse = gameObject.AddComponent<ScrollListElemReuse>();
            if (null != scroll)
                scroll.onValueChanged.AddListener(reuse.CalculateReuse);
        }
    }

    public abstract void ScrollTo(int index);
    public void ScrollTo(Object data)
    {
        ScrollDataElement elem = datas.Find((e)=> (Object)e.data == data);
        if(null != elem)
            ScrollTo(datas.IndexOf(elem));
    }

    protected abstract void CalculateSize(int startIndex = 0);

    /// <summary>
    /// 添加一条数据至数据表结尾处
    /// </summary>
    /// <param name="data">要添加的数据</param>
    /// <param name="scrollFlag">滚动flag true - 滚动到最后一个元素；默认是false - 不滚动</param>
    public virtual void AddData(Object _data, bool scrollFlag = false)
    {
        if (null != datas)
        {
            if (null == _data)
            {
                GLog.LogError("Can not add null data to scroll list!");
                return;
            }
            datas.Add(new ScrollDataElement() { data = _data });
            CalculateSize(datas.Count - 1);
            if (scrollFlag)
                ScrollTo(datas.Count - 1);
        }
        else
        {
            GLog.LogError("null datas can not add new data,use SetData instead!");
        }
    }

    /// <summary>
    /// 插入一条数据到指定位置
    /// </summary>
    /// <param name="data">要插入的数据</param>
    /// <param name="index">要插入数据的位置</param>
    /// <param name="scrollFlag">滚动flag true - 滚动到插入元素的位置； 默认false - 不滚动</param>
    /// <returns>如果插入的位置是正在显示中的位置元素，则返回这个元素</returns>
    public virtual ScrollElement InsertData(Object _data,int index,bool scrollFlag = false)
    {
        if(null == datas)
        {
            GLog.LogError("Can not insert data because scroll list has not called SetData!");
            return null;
        }
        datas.Insert(index,new ScrollDataElement() { data = _data});
        CalculateSize(index);
        if (scrollFlag)
            ScrollTo(index);
        return null;
    }
    
    /// <summary>
    /// 移除一条数据
    /// </summary>
    /// <param name="data">要移除的数据</param>
    public virtual void RemoveData(Object data)
    {
        ScrollDataElement elem = datas.Find((e) => (Object)e.data == data);
        if(null != elem)
        {
            RemoveDataAt(datas.IndexOf(elem));
        }
    }

    /// <summary>
    /// 移除指定位置的一条数据
    /// </summary>
    /// <param name="index">要移除的数据的位置</param>
    public virtual void RemoveDataAt(int index)
    {
        if(null == datas)
        {
            GLog.LogWarning("Failed to remove data at " + index + "because scroll list doesn't has any data");
            return;
        }

        datas.RemoveAt(index);
        CalculateSize(index);        
    }
}
