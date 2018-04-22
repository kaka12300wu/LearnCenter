using UnityEngine;
using UnityEngine.UI;

public class ScreenLayoutElement : LayoutElement
{
    public int ScreenWidthPercent = 0;
    public float maxWidth;
    public int ScreenHeightPercent = 0;
    public float maxHeight;
    public Vector2 aspect;

    public override float preferredWidth
    {
        get
        {
            float retVal = base.preferredWidth;
            if (0 != ScreenWidthPercent)
            {
                retVal = Screen.width * (ScreenWidthPercent / 100.0f);
            }
            if (0 != minWidth)
            {
                retVal = Mathf.Max(minWidth, retVal);
            }
            if (0 != maxWidth)
            {
                retVal = Mathf.Min(maxWidth, retVal);
            }
            if(0 != aspect.x && 0 != aspect.y)
            {
                ///横屏模式下，需要根据高度和比例进行宽度限制
                if(Screen.width > Screen.height)
                {
                    retVal = Mathf.Clamp(this.preferredHeight * aspect.x / aspect.y,minWidth,maxWidth);
                }
            }
            return retVal;
        }

        set
        {
            base.preferredWidth = value;
        }
    }

    public override float preferredHeight
    {
        get
        {
            float retVal = base.preferredHeight;
            if (0 != ScreenHeightPercent)
            {
                retVal = Screen.height * (ScreenHeightPercent / 100.0f);
            }
            if (0 != minHeight)
            {
                retVal = Mathf.Max(minHeight, retVal);
            }
            if (0 != maxHeight)
            {
                retVal = Mathf.Min(maxHeight, retVal);
            }
            if (0 != aspect.x && 0 != aspect.y)
            {
                ///竖屏模式下，需要根据宽度和比例进行高度限制
                if (Screen.height > Screen.width)
                {
                    retVal = Mathf.Clamp(this.preferredWidth * aspect.y / aspect.x,minHeight,maxHeight);
                }
            }
            return retVal;
        }

        set
        {
            base.preferredHeight = value;
        }
    }

}
