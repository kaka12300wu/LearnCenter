using UnityEngine;

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
}
