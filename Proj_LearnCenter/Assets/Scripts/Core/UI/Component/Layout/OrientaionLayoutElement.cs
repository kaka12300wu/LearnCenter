using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 使用此layoutelem 元素会随屏幕横屏竖屏采用不同的布局大小
/// </summary>
[RequireComponent(typeof(RectTransform)), ExecuteInEditMode, AddComponentMenu("Layout/OrientaionLayoutElement", 150)]
public sealed class OrientaionLayoutElement : UIBehaviour, ILayoutElement, ILayoutIgnorer
{
    public Vector2 potraitPreferredSize;

    [Tooltip("val.x and val.y will devide 100")]
    public Vector2 potraitPreferredPercent;
        
    public Vector2 landscapePreferredSize;

    [Tooltip("val.x and val.y will devide 100")]
    public Vector2 landscapePreferredPercent;

    public Vector2 potraitMaxSize;
    
    public Vector2 landScapeMaxSize;

    public Vector2 potraitMinSize;

    public Vector2 landscapeMinSize;

    [Tooltip("两个值都不为0才会生效,保持纵横比")]
    public Vector2 aspectRatio;

    [SerializeField]
    private bool m_IgnoreLayout;
    
    public float preferredWidth
    {

        get
        {
            return GetProperWidth();
        }

        set
        {
            if(SingletonObject.getInstance<DeviceOrientationManager>().IsLandscape())
            {
                landscapePreferredSize.x = value;
            }
            else
            {
                potraitPreferredSize.x = value;
            }
            SetDirty();
        }
    }

    public float preferredHeight
    {
        get
        {
            return GetProperHeight();
        }

        set
        {
            if (SingletonObject.getInstance<DeviceOrientationManager>().IsLandscape())
            {
                landscapePreferredSize.y = value;
            }
            else
            {
                potraitPreferredSize.y = value;
            }
            SetDirty();
        }
    }

    public float flexibleHeight
    {
        get
        {
            return GetProperHeight();
        }
        set
        {
            
            this.SetDirty();
        }
    }

    public float flexibleWidth
    {
        get
        {
            return GetProperWidth();
        }
        set
        {
            this.SetDirty();
        }
    }

    public float minHeight
    {
        get
        {
            if (SingletonObject.getInstance<DeviceOrientationManager>().IsLandscape())
            {
                return landscapeMinSize.y;
            }
            else
            {
                return potraitMinSize.y;
            }
        }
        set
        {
            if (SingletonObject.getInstance<DeviceOrientationManager>().IsLandscape())
            {
                landscapeMinSize.y = value;
            }
            else
            {
                potraitMinSize.y = value;
            }
            this.SetDirty();
        }
    }

    public float minWidth
    {
        get
        {
            if (SingletonObject.getInstance<DeviceOrientationManager>().IsLandscape())
            {
                return landscapeMinSize.x;
            }
            else
            {
                return potraitMinSize.x;
            }
        }
        set
        {
            if (SingletonObject.getInstance<DeviceOrientationManager>().IsLandscape())
            {
                landscapeMinSize.x = value;
            }
            else
            {
                potraitMinSize.x = value;
            }
            this.SetDirty();
        }
    }

    public bool ignoreLayout
    {
        get
        {
            return this.m_IgnoreLayout;
        }
        set
        {
            m_IgnoreLayout = value;
            this.SetDirty();
        }
    }

    public int layoutPriority
    {
        get
        {
            return 1;
        }
    }
    
    private float GetProperWidth()
    {
        float retVal = 0;
        float max = 0;
        float min = 0;
        if(SingletonObject.getInstance<DeviceOrientationManager>().IsLandscape())
        {
            retVal = landscapePreferredSize.x;
            if (0 != landscapePreferredPercent.x)
                retVal = Screen.width * landscapePreferredPercent.x / 100;
            max = landScapeMaxSize.x <= 0 ? retVal : landScapeMaxSize.x;
            min = landscapeMinSize.x <= 0 ? retVal : landscapeMinSize.x;
            retVal = Mathf.Clamp(retVal, min, max);
        }
        else
        {
            retVal = potraitPreferredSize.x;
            if (0 != potraitPreferredPercent.x)
                retVal = Screen.width * potraitPreferredPercent.x / 100;
            max = potraitMaxSize.x <= 0 ? retVal : potraitMaxSize.x;
            min = potraitMinSize.x <= 0 ? retVal : potraitMinSize.x;
            retVal = Mathf.Clamp(retVal, min, max);
        }

        return retVal;
    }

    private float GetProperHeight()
    {
        float retVal = 0;
        if (SingletonObject.getInstance<DeviceOrientationManager>().IsLandscape())
        {
            retVal = landscapePreferredSize.y;
            retVal = Mathf.Clamp(retVal, landscapeMinSize.y, landScapeMaxSize.y);
        }
        else
        {
            retVal = potraitPreferredSize.y;
            retVal = Mathf.Clamp(retVal, potraitMinSize.y, potraitMaxSize.y);
        }
         
        return retVal;
    }

    void SetDirty()
    {
        if (this.IsActive())
        {
            LayoutRebuilder.MarkLayoutForRebuild(base.transform as RectTransform);
        }
    }

    void OnScreenOrientationChange(DeviceOrientation preOri,DeviceOrientation newOri)
    {
        this.SetDirty();
    }

    protected override void OnBeforeTransformParentChanged()
    {
        this.SetDirty();
    }

    protected override void OnDidApplyAnimationProperties()
    {
        this.SetDirty();
    }

    protected override void OnDisable()
    {
        this.SetDirty();
        base.OnDisable();
        DeviceOrientationManager.OnDeviceOrientationChange -= OnScreenOrientationChange;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.SetDirty();
        DeviceOrientationManager.OnDeviceOrientationChange += OnScreenOrientationChange;
    }

    protected override void OnTransformParentChanged()
    {
        this.SetDirty();
    }

    protected override void OnValidate()
    {
        this.SetDirty();
    }

    public void CalculateLayoutInputHorizontal()
    {
    }

    public void CalculateLayoutInputVertical()
    {
    }
}
