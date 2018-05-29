using UnityEngine;

[ExecuteInEditMode]
public class ScrollElement : MonoBehaviour
{
    /// <summary>
    /// 忽略复用系统，仅在复用模块存在时有效\n一般不自己主动勾选，由程序配合居中组件决定是否忽略复用系统
    /// </summary>
    [HideInInspector]
    public bool ignoreReuse;

    public bool userBool;

    public int userInt;

    public object userData;

    [Tooltip("UI元素变量引用")]
    public UIReferences refer;

    [HideInInspector]
    public Vector2 size;

    //仅在静态尺寸下读取设置好的宽高
    public void ReadSize()
    {
        RectTransform trans = transform as RectTransform;
        if(null != trans)
        {
            size = new Vector2(trans.rect.width,trans.rect.height);
        }
    }
}
