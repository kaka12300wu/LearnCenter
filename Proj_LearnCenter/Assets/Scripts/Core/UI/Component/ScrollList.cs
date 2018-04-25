#define ScrollListTest

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public enum ScrollDirection
{
    LeftToRight,
    RightToLeft,
    TopToBottom,
    BottomToTop
}

public class ScrollList : MonoBehaviour
{
    public bool dynamicSize = false;
    public ScrollDirection direction;    
    public UIReferences prefab;
    public ScrollRect scroll;
    public RectTransform viewPort;
    //public RTLineAnimation anim;
    public Vector2 offset;
    public RectOffset padding;
    public int pageSize = 1;
    [HideInInspector]
    public bool centerOnChild = false; 

    //[HideInInspector]
    public int currFirstIndex = 0;
    public object noteLast; //这个变量用于记录上次点击的对象，如果需要SingleOn，可以使用这个变量

    public List<object> data;
    public List<ScrollListItem> itemList;

    private ScrollPool pool;
    private Coroutine initsizeCoroutine;
    public Action<object, UIReferences> renderFunc;
    private bool needRefresh = false;
    private RectTransform containerTrans;
    private bool centering = false;

    void Awake()
    {
        pool = new ScrollPool();
        pool.Init(prefab);
        Init();
    }

#if ScrollListTest
    public InputField input;
    public int TestdataSize = 1000;
    void Start()
    {
        TestScrollList testscroll = new TestScrollList(TestdataSize,direction);
        testscroll.Run(this);
    }

    public void JumpTo()
    {
        int index;
        if (int.TryParse(input.text, out index))
            ScrollTo(index,0.3f);
    }

    public void InsertTo()
    {
        float val;
        if (float.TryParse(input.text, out val))
        {
            val = Mathf.Clamp(val, 40, 180.0f);
            InsertData(val, currFirstIndex + UnityEngine.Random.Range(0, pageSize + 1));
        }
    }

    public void RemoveAt()
    {
        int index;
        if(int.TryParse(input.text,out index))
        {
            RemoveDataAtIndex(index);
        }
    }

    public void Add()
    {
        float val;
        if (float.TryParse(input.text, out val))
        {
            val = Mathf.Clamp(val,40, 180.0f);
            AddData(val);
        }
    }
#endif

    void OnDestroy()
    {
        pool.Dispose();
    }
    
    void Init()
    {
        if(null == viewPort)
        {
            viewPort = transform.parent as RectTransform;    
        }

        //if (null == anim)
        //{
        //    anim = GetComponent<RTLineAnimation>();
        //}
        //if (null == anim)
        //{
        //    Debug.LogError("variable anim must be specified!");
        //    return;
        //}
      
        if (null == scroll)
            scroll = GetComponentInParent<ScrollRect>();
        if (null == scroll)
        {
            Debug.LogError("variable scroll must be specified!");
            return;
        }
        scroll.onValueChanged.AddListener(OnScroll);

        if (null == prefab)
        {
            Debug.LogError("variable prefab must be specified!");
            return;
        }

        RectTransform rectTrans = prefab.transform as RectTransform;
        containerTrans = transform as RectTransform;
        if(null != containerTrans)
        {
            if (direction == ScrollDirection.TopToBottom)
            {
                containerTrans.pivot = new Vector2(0.5f, 1);
                containerTrans.anchorMin = new Vector2(0.5f, 1);
                containerTrans.anchorMax = new Vector2(0.5f, 1);
                scroll.horizontal = false;
                scroll.vertical = true;
            }
            else if (direction == ScrollDirection.LeftToRight)
            {
                containerTrans.pivot = new Vector2(0, 0.5f);
                containerTrans.anchorMin = new Vector2(0, 0.5f);
                containerTrans.anchorMax = new Vector2(0, 0.5f);
                scroll.horizontal = true;
                scroll.vertical = false;
            }
            else if (direction == ScrollDirection.RightToLeft)
            {
                containerTrans.pivot = new Vector2(1, 0.5f);
                containerTrans.anchorMin = new Vector2(1, 0.5f);
                containerTrans.anchorMax = new Vector2(1, 0.5f);
                scroll.horizontal = true;
                scroll.vertical = false;
            }
            else if (direction == ScrollDirection.BottomToTop)
            {
                containerTrans.pivot = new Vector2(0.5f, 0);
                containerTrans.anchorMin = new Vector2(0.5f, 0);
                containerTrans.anchorMax = new Vector2(0.5f, 0);
                scroll.horizontal = false;
                scroll.vertical = true;
            }
        }
        if (null != rectTrans)
        {
            prefab.transform.SetParent(viewPort.parent, false);
            rectTrans.pivot = containerTrans.pivot;
            Vector3 pos = rectTrans.anchoredPosition;
            pos.x = -10000;
            rectTrans.anchoredPosition = pos;
            rectTrans.anchorMin = containerTrans.anchorMin;
            rectTrans.anchorMax = containerTrans.anchorMax;
            prefab.gameObject.SetActive(true);
        } 

        containerTrans.anchoredPosition = Vector3.zero;
    }

    void PositionItem(ScrollListItem item)
    {
        if(null == item.item)
        {
            throw new Exception("Null arg item to position! :" + name);
        }
        RectTransform itemTrans = item.item.transform as RectTransform;
        Vector3 pos = Vector3.zero;
        if(direction == ScrollDirection.LeftToRight)
        {
            pos.x = item.pos;
        }
        else if (direction == ScrollDirection.RightToLeft)
        {
            pos.x = -item.pos;
        }
        else if (direction == ScrollDirection.BottomToTop)
        {
            pos.y = item.pos;
        }
        else if (direction == ScrollDirection.TopToBottom)
        {
            pos.y = -item.pos;
        }
        itemTrans.anchoredPosition = pos;
    }

    void OnScroll(Vector2 vec)
    {
        if(needRefresh)
        {
            scroll.StopMovement();
            //anim.Stop();
        }

        float line = containerTrans.anchoredPosition.x;
        if (direction == ScrollDirection.BottomToTop || direction == ScrollDirection.TopToBottom)
            line = containerTrans.anchoredPosition.y;
        int startIndex = 0;
        int max = itemList.Count;
        if (max > 300)
        {
            startIndex = Mathf.Clamp(currFirstIndex - 5 * pageSize, 0, max - pageSize);
            max = Mathf.Clamp(currFirstIndex + 5 * pageSize, 0, max);
        }

//        if (centerOnChild && scroll.velocity.sqrMagnitude < 2 && !centering)            
//        {
//#if ScrollListTest
//            Debug.Log(scroll.velocity.sqrMagnitude);
//#endif
//            centering = true;
//            CenterChild(line,startIndex,max);
//        }        
        
        float dis = float.MaxValue;
        for (; startIndex < max; ++startIndex)
        {
            ScrollListItem item = itemList[startIndex];
            item.SetVisible(line);
            if (item.visible)
            {                
                if (!needRefresh && dis > Math.Abs(Math.Abs(line) - Math.Abs(item.pos)))
                {
                    dis = Math.Abs(Math.Abs(line) - Math.Abs(item.pos));
                    currFirstIndex = startIndex;
                }
                if (null == item.item)
                {
                    item.item = pool.Get();
                    item.item.transform.SetParent(containerTrans,false);
                    item.Render();
                    PositionItem(item);
                }               
            }
            else
            {
                UIReferences rfitem = item.Release();
                if(null != rfitem)
                {
                    pool.Save(rfitem);
                }
            }
        }
    }

//    void CenterChild(float line,int indexStart,int indexEnd)
//    {
//#if ScrollListTest
//        Debug.Log("Call CenterChild");
//        int CenterIndex = 0;
//#endif
//        float viewSize = viewPort.rect.width;
//        if (direction >= ScrollDirection.TopToBottom)
//            viewSize = viewPort.rect.height;
        

//        float dis = float.MaxValue;        
//        Vector2 bestCenter = Vector2.zero;
//        float itemDis;
//        float itemCenterLine;
//        for (int i = indexStart; i < indexEnd; ++i)
//        {
//            Vector2 posItem = IndexToPos(i);
//            ScrollListItem item = itemList[i];
//            itemCenterLine = 0;
//            if(direction == ScrollDirection.TopToBottom)
//            {
//                itemCenterLine = -posItem.y - viewSize / 2 + item.size / 2;                
//                //posItem.y -= viewSize / 2 + item.size / 2;
//            }
//            else if (direction == ScrollDirection.BottomToTop)
//            {
//                itemCenterLine = posItem.y + viewSize / 2 - item.size / 2;
//                //posItem.y += viewSize / 2 - item.size / 2;
//            }
//            else if (direction == ScrollDirection.LeftToRight)
//            {
//                itemCenterLine = posItem.x + viewSize / 2 - item.size / 2;
//                //posItem.x += viewSize / 2 - item.size / 2;
//            }
//            else if (direction == ScrollDirection.RightToLeft)
//            {
//                itemCenterLine = -posItem.x - viewSize / 2 + item.size / 2;
//                //posItem.x -= viewSize / 2 + item.size / 2;
//            }
//            itemDis = Math.Abs(itemCenterLine + line);
//            if(itemDis < dis)
//            {
//                dis = itemDis;
//                bestCenter = posItem;
//#if ScrollListTest
//                CenterIndex = i;
//#endif
//            }
//        }

//        if(Vector2.zero != bestCenter)
//        {
//#if ScrollListTest
//            Debug.Log("CenterIndex = " + CenterIndex);
//#endif
//            scroll.StopMovement();
//            anim.duration = 0.2f;
//            anim.from = containerTrans.anchoredPosition;
//            anim.to = bestCenter;
//            anim.finishedCallBack = (a) =>
//            {
//#if ScrollListTest
//                Debug.Log("Center child Finish");
//#endif
//                centering = false;
//                anim.finishedCallBack = null;
//            };
//            anim.Play(0);
//        }
//        else
//        {
//            centering = false;
//        }
//    }

    bool CheckIndexAvilable(int index)
    {
        if (null == data || null == itemList || index >= data.Count || index < 0)
        {
            return false;
        }
        return true;
    }

    Vector2 IndexToPos(int index)
    {
        if (!CheckIndexAvilable(index))
        {
            throw new Exception("data has not been set or Index out of Range!");
        }

        Vector2 vec = Vector2.zero;
        if (index == 0)
            return vec;
        ScrollListItem item = itemList[index];
        if (direction == ScrollDirection.LeftToRight)
        {
            vec.x = -item.pos;
        }
        else if (direction == ScrollDirection.RightToLeft)
        {
            vec.x = item.pos;
        }
        else if (direction == ScrollDirection.TopToBottom)
        {
            vec.y = item.pos;
        }
        else if (direction == ScrollDirection.BottomToTop)
        {
            vec.y = -item.pos;
        }
        return vec;
    }

    void CheckRelease(int index)
    {
        ScrollListItem item = itemList[index];
        if(!item.visible)
        {
            for(int i = currFirstIndex,max = itemList.Count;i<max;++i)
            {
                if (!itemList[i].visible)
                    break;
                else
                {
                    UIReferences rfitem = itemList[i].Release();
                    if (null != rfitem)
                        pool.Save(rfitem);
                }
            }
            for(int i = currFirstIndex - 1;i>=0;--i)
            {
                if (!itemList[i].visible)
                    break;
                else
                {
                    UIReferences rfitem = itemList[i].Release();
                    if (null != rfitem)
                        pool.Save(rfitem);
                }
            }
        }
    }
    
    IEnumerator InitAllItemSize()
    {
        float transSize = 0;
        float gap = offset.x;
        float viewSize = viewPort.rect.width;
        RectTransform rectPrefab = prefab.transform as RectTransform;
        RectTransform.Axis axis = RectTransform.Axis.Horizontal;
        float size = rectPrefab.sizeDelta.x;
        if (direction == ScrollDirection.LeftToRight)
            transSize = padding.left;
        else if (direction == ScrollDirection.RightToLeft)
            transSize = padding.right;
        else
        {
            if (direction == ScrollDirection.TopToBottom)
                transSize = padding.top;
            else
                transSize = padding.bottom;
            gap = offset.y;
            size = rectPrefab.sizeDelta.y;
            axis = RectTransform.Axis.Vertical;
            viewSize = viewPort.rect.height;
        }
        if (centerOnChild)
            transSize = viewSize / 2;
       
        for (int i = 0, max = itemList.Count; i < max; ++i)
        {
            ScrollListItem item = itemList[i];
            item.dataIndex = i;
            if (dynamicSize)
            {
                if (max > 100 && i / pageSize >= 2 && i % pageSize == 0)
                {
                    if(!needRefresh)
                        containerTrans.SetSizeWithCurrentAnchors(axis, transSize);
                    yield return new WaitForEndOfFrame();
                }                
                item.Init(prefab, direction);                                       
            }
            else
            {
                item.size = size;                
            }
            if(0 == i && centerOnChild)
            {
                transSize -= item.size / 2;
            }
            item.pos = transSize;
            transSize += item.size + gap;
            if (direction == ScrollDirection.TopToBottom || direction == ScrollDirection.RightToLeft)
            {
                item.minLine = item.pos - viewSize * 1.5f - item.size;
                item.maxLine = item.pos + viewSize / 2 + item.size;
            }
            else if (direction == ScrollDirection.BottomToTop || direction == ScrollDirection.LeftToRight)
            {
                item.minLine = -item.pos - viewSize / 2 - item.size;
                item.maxLine = -item.pos + viewSize * 1.5f + item.size;
            }
            if(needRefresh && null != item.item)
            {
                item.Render();
                PositionItem(item);
            }
        }

        if (centerOnChild)
            transSize += viewSize / 2 - itemList[itemList.Count - 1].size / 2;

        containerTrans.SetSizeWithCurrentAnchors(axis, transSize);

        if(needRefresh)
        {
            needRefresh = false;
            Refresh(currFirstIndex);
        }
    }

    public void SetData(List<object> _data)
    {
        if (null != itemList)
            itemList.Clear();
        else
            itemList = new List<ScrollListItem>();
        data = _data;
        for(int i = 0,max = data.Count;i<max;++i)
        {
            ScrollListItem item = new ScrollListItem();
            item.renderFunc = renderFunc;
            item.data = data[i];
            itemList.Add(item);
        }
        initsizeCoroutine = StartCoroutine("InitAllItemSize");        
    }
    
    public void Refresh(int headIndex = 0)
    {
        if(!CheckIndexAvilable(headIndex))
        {
            throw new Exception("data has not been set or Index out of Range!");
        }
        CheckRelease(headIndex);
        currFirstIndex = headIndex;
        Vector2 vec = IndexToPos(headIndex);        
        containerTrans.anchoredPosition = vec;
        OnScroll(vec);
    }

    public void ScrollTo(int index,float time = 0)
    {
        if (!CheckIndexAvilable(index))
        {
            throw new Exception("data has not been set or Index out of Range!");
        }
        scroll.StopMovement();
        if (0 >= time)
        {
            Refresh(index);
            return;
        }
        CheckRelease(index);
        currFirstIndex = index;
        Vector2 vec = IndexToPos(index);
        //anim.duration = time;
        //anim.from = containerTrans.anchoredPosition;
        //anim.to = vec;
        //anim.Play(0);
    }

    public void AddData(object elem)
    {
        if (null != initsizeCoroutine)
            StopCoroutine(initsizeCoroutine);
        if (null == data)
            data = new List<object>();
        if (null == itemList)
            itemList = new List<ScrollListItem>();
        data.Add(elem);
        ScrollListItem item = new ScrollListItem();
        item.renderFunc = renderFunc;
        item.data = elem;
        itemList.Add(item);
        needRefresh = true;
        initsizeCoroutine = StartCoroutine("InitAllItemSize");
    }

    public void InsertData(object elem, int insertIndex)
    {
        if (null != initsizeCoroutine)
            StopCoroutine(initsizeCoroutine);
        if(null == data || null == itemList || insertIndex >= data.Count || insertIndex < 0)
        {
            throw new Exception("data has not been set or Index out of Range!");
        }
        data.Insert(insertIndex, elem);
        ScrollListItem item = new ScrollListItem();
        item.renderFunc = renderFunc;
        item.data = elem;        
        itemList.Insert(insertIndex, item);
        needRefresh = true;
        initsizeCoroutine = StartCoroutine("InitAllItemSize");
    }

    public void RemoveData(object elem)
    {
        RemoveDataAtIndex(data.IndexOf(elem));
    }

    public void RemoveDataAtIndex(int index)
    {
        if (null != initsizeCoroutine)
            StopCoroutine(initsizeCoroutine);
        if (null == data || null == itemList || index >= data.Count || index < 0)
        {
            throw new Exception("data has not been set or Index out of Range!");
        }
        data.RemoveAt(index);
        ScrollListItem item = itemList[index];
        itemList.RemoveAt(index);
        if (null != item.item)
            pool.Save(item.Release());
        needRefresh = true;
        initsizeCoroutine = StartCoroutine("InitAllItemSize");
    }
}

public class ScrollListItem
{
    public object data;
    public int dataIndex = -1;
    public UIReferences item;
    public float size = 0;
    public float pos;
    public float minLine;
    public float maxLine;
    public bool visible = false;
    public Action<object, UIReferences> renderFunc;

    public void Init(UIReferences prefab,ScrollDirection dir)
    { 
        if(null != data && null != renderFunc)
        {
            renderFunc(data, prefab);
            RectTransform trans = prefab.transform as RectTransform;
            LayoutRebuilder.ForceRebuildLayoutImmediate(trans);
            if (dir == ScrollDirection.LeftToRight || dir == ScrollDirection.RightToLeft)
                size = trans.sizeDelta.x;
            else
                size = trans.sizeDelta.y;
        }
    }

    public void SetVisible(float line)
    {
        visible = line > minLine && line < maxLine;
    }

    public void Render()
    {        
        if(null != item && null != renderFunc && null != data)
        {
            item.userInt = dataIndex;
            renderFunc(data, item);
        }
    }

    public UIReferences Release()
    {
        UIReferences ret = item;
        item = null;
        return ret;
    }
}

public class ScrollPool : IDisposable
{
    private UIReferences original;
    private List<UIReferences> poolItems;

    public void Init(UIReferences prefab)
    {
        original = prefab;
        poolItems = new List<UIReferences>();
    }

    public UIReferences Get()
    { 
        UIReferences ret;
        if(poolItems.Count > 0)
        {
            ret = poolItems[0];
            poolItems.Remove(ret);
        }
        else
        {
            ret = GameObject.Instantiate<UIReferences>(original);
        }
        ret.gameObject.SetActive(true);
        return ret;
    }

    public void Save(UIReferences item)
    {
        if(null != item)
        {
            poolItems.Add(item);
            item.gameObject.SetActive(false);
        }
    }

    public void Dispose()
    {
        for(int i = 0,max = poolItems.Count;i<max;++i)
        {
            if(null != poolItems[i])
                GameObject.Destroy(poolItems[i].gameObject);
        }
        poolItems.Clear();
        poolItems = null;
        if(null != original)
            GameObject.Destroy(original.gameObject);
        original = null;
    }
}

#if ScrollListTest
    public class TestScrollList
    {
        public class FloatObj
        {
            public float size;
        }
        List<object> listdatas;
        ScrollDirection dir;
        public TestScrollList(int capacity,ScrollDirection direction)
        {
            dir = direction;
            listdatas = new List<object>();
            for (int i = 0; i < capacity; ++i)
            {
                float val = UnityEngine.Random.Range(150.0f, 250.0f);
                listdatas.Add((object)val);
            }
        }

        public void Run(ScrollList list)
        {
            list.renderFunc = OnRenderItem;
            list.SetData(listdatas);
            list.Refresh();
        }

        void OnRenderItem(object data, UIReferences item)
        {
            float val = (float)data;
            Image img = item.Get(1) as Image;
            Text info = item.Get(2) as Text;
            img.color = new Color(val / 255.0f, val / 255.0f, val / 255.0f);
            info.text = string.Format("{0}:{1}", item.userInt, val);
            RectTransform rectTrans = item.transform as RectTransform;
            RectTransform rectParent = rectTrans.parent as RectTransform;
            if (dir == ScrollDirection.RightToLeft || dir == ScrollDirection.LeftToRight)
            {
                rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, val);
                rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectParent.sizeDelta.y);
            }
            else
            {
                rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectParent.sizeDelta.x);
                rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, val);
            }
        }
    }
#endif