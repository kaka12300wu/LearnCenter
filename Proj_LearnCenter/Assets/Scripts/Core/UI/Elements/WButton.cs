using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class WButton : Button
{
    public enum ButtonState
    {
        Normal = 0,
        HighLight = 1,
        Disabled = 2
    }
    
    public Callback<GameObject> m_OnClickEvent;
    public bool m_autoListen = false;
    
    public WImage graphicImage;

    public Sprite normalSprite;
    public Sprite highLightSprite;
    public Sprite disableSprite;

    public AudioClip normalClip;
    public AudioClip highlightClip;
    public AudioClip disableClip;


    [SetProperty("m_curState")]
    private ButtonState _curState;
    public ButtonState m_curState
    {
        get { return _curState; }
        set 
        {            
            _curState = value;
            UpdateButtonView();
        }
    }

    bool IsButtonSpriteSwapDisable()
    {
        return null == normalSprite && null == highLightSprite && null == disableSprite;
    }
        
    public void UpdateButtonView()
    {
        if (null == graphicImage || IsButtonSpriteSwapDisable()) return;
        graphicImage.sprite = normalSprite;
        if (_curState == ButtonState.HighLight && null != highLightSprite)
        {
            graphicImage.sprite = highLightSprite;
        }
        else if (_curState == ButtonState.Disabled && null != disableSprite)
        {
            graphicImage.sprite = disableSprite;
        }
        if (graphicImage.type == Image.Type.Simple || !graphicImage.hasBorder)
        {
            graphicImage.SetNativeSize();
        }
    }
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (null != m_OnClickEvent && null != eventData.pointerPress && _curState != ButtonState.Disabled)
        {
            m_OnClickEvent(eventData.pointerPress);
        }
    }

    void InitGraphicImage()
    {
        if (null == graphicImage)
        {
            graphicImage = GetComponentInChildren<WImage>();            
        }
        if (null != graphicImage && null == normalSprite)
        {
            normalSprite = graphicImage.sprite;
        }
    }

    protected override void Awake()
    {
        InitGraphicImage();
    }

    protected override void OnEnable()
    {
        InitGraphicImage();
    }
	
}
