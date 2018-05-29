using UnityEngine;
using System.Collections.Generic;

public class TabButtonGroup : MonoBehaviour
{
    public Callback<int, WButton> mOnIndex;
    public Callback<WButton> mOnBlurButton;

    public WButton[] m_Configbtns;
    //本tab组所有按钮
    private List<WButton> mBtnList;
    
    //当前选中的按钮Index
    public int m_curOnIndex = 0;

    void Awake()
    {
        if(null != m_Configbtns && m_Configbtns.Length > 0)
        {
            for(int i = 0,max = m_Configbtns.Length;i<max;++i)
            {
                AddBtn(m_Configbtns[i]);
            }
        }

        RefreshTabView();
    }
    
    public void RefreshTabView()
    {
        if (null == mBtnList || mBtnList.Count <= 0)
        {
            return;
        }
        for (int i = 0, max = mBtnList.Count; i < max; ++i)
        {
            if(i == m_curOnIndex)
            {
                mBtnList[i].m_curState = WButton.ButtonState.HighLight;
            }
            else
            {
                mBtnList[i].m_curState = WButton.ButtonState.Normal;                
            }
        }
    }

    public int AddBtn(WButton a_btn)
    {
        if (null == mBtnList)
        {
            mBtnList = new List<WButton>();
        }
        mBtnList.Add(a_btn);

        return mBtnList.Count;
    }

    public void SetOnIndex(int index)
    {
        WButton nowOnBtn = null;
        if (m_curOnIndex >= 0 && m_curOnIndex < mBtnList.Count)
        {
            nowOnBtn = mBtnList[m_curOnIndex]; 
        }
        if (null != nowOnBtn && null != mOnBlurButton)
        {
            mOnBlurButton(nowOnBtn);
        }

        index -= 1;
        if(index >= 0 && index < mBtnList.Count)
        {
            m_curOnIndex = index;
            RefreshTabView();
            if(null != mOnIndex)
            {
                mOnIndex(index, mBtnList[index]);
            }
        }

    }

}
