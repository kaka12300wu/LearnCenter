using UnityEngine;
using System.Collections.Generic;

public class PoolManager
{
    /// <summary>
    /// 超时时间
    /// </summary>
    public const int EXPIRE_TIME = 1 * 10;

    private static Dictionary<string, PoolItem> itemList;

    /// <summary>
    /// 添加一条数据
    /// </summary>
    /// <param name="path">Path.</param>
    /// <param name="maxSize">Max size.</param>
    public static void PushData(string path)
    {
        if (itemList == null) itemList = new Dictionary<string, PoolItem>();
        if (!itemList.ContainsKey(path)) itemList.Add(path, new PoolItem(path));
    }

    /// <summary>
    /// 添加 GameObject 对象
    /// </summary>
    /// <param name="path">Path.</param>
    /// <param name="gameObject">Game object.</param>
    public static void PushObject(string path, GameObject gameObject)
    {
        if (itemList == null || !itemList.ContainsKey(path)) PushData(path);
        // 添加对象
        itemList[path].PushObject(gameObject);
    }

    /// <summary>
    /// 移除一条数据
    /// </summary>
    /// <param name="path">Path.</param>
    public static void RemoveData(string path)
    {
        if (itemList == null) return;
        itemList.Remove(path);
    }

    /// <summary>
    /// 移除 GameObject 对象
    /// </summary>
    /// <param name="path">Path.</param>
    /// <param name="gameObject">Game object.</param>
    public static void RemoveObject(string path, GameObject gameObject)
    {
        if (itemList == null || !itemList.ContainsKey(path)) return;

        // 移除对象
        itemList[path].RemoveObject(gameObject);
    }

    /// <summary>
    /// 获取缓存的对象
    /// </summary>
    /// <param name="path">Path.</param>
    public static GameObject GetObject(string path)
    {
        if (itemList == null || !itemList.ContainsKey(path)) return null;
        return itemList[path].GetObject();
    }

    /// <summary>
    /// 禁用对象
    /// </summary>
    /// <param name="path">Path.</param>
    /// <param name="gameObject">Game object.</param>
    public static void DestoryObject(string path, GameObject gameObject)
    {
        if (itemList == null || !itemList.ContainsKey(path)) return;
        itemList[path].DestoryObject(gameObject);
    }

    /// <summary>
    /// 处理超时的对象
    /// </summary>
    public static void ExpireObject()
    {
        if (itemList == null)
        {
            return;
        }
        // 筛选符合条件的数据
        foreach (PoolItem poolItem in itemList.Values)
        {
            poolItem.ExpireObject();
        }
    }

    /// <summary>
    /// 销毁对象
    /// </summary>
    public static void Destroy()
    {
        foreach (PoolItem poolItem in itemList.Values)
        {
            poolItem.Destory();
        }
        itemList = null;
    }
}
