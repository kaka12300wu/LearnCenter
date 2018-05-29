using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 池数据
/// </summary>
public class PoolItem
{
    /// <summary>
    /// 路径，对象标识
    /// </summary>
    public string path;

    /// <summary>
    /// 对象列表
    /// </summary>
    public Dictionary<int, PoolItemTime> objectList;

    public PoolItem(string path)
    {
        this.path = path;
        this.objectList = new Dictionary<int, PoolItemTime>();
    }

    /// <summary>
    /// 添加对象
    /// </summary>
    /// <param name="gameObject">Game object.</param>
    public void PushObject(GameObject gameObject)
    {
        int hashKey = gameObject.GetHashCode();
        if (!this.objectList.ContainsKey(hashKey))
        {
            this.objectList.Add(hashKey, new PoolItemTime(gameObject));
        }
        else
        {
            this.objectList[hashKey].Active();
        }
    }

    /// <summary>
    /// 销毁对象
    /// </summary>
    /// <param name="gameObject">Game object.</param>
    public void DestoryObject(GameObject gameObject)
    {
        int hashKey = gameObject.GetHashCode();
        if (this.objectList.ContainsKey(hashKey))
        {
            this.objectList[hashKey].Destory();
        }
    }

    /// <summary>
    /// 获取未真正销毁的对象（池对象）
    /// </summary>
    public GameObject GetObject()
    {
        if (this.objectList == null || this.objectList.Count == 0) return null;

        foreach (PoolItemTime poolItemTime in this.objectList.Values)
        {
            if (poolItemTime.destoryStatus) return poolItemTime.Active();
        }

        return null;
    }

    /// <summary>
    /// 移除并且销毁对象
    /// </summary>
    /// <param name="gameObject">Game object.</param>
    public void RemoveObject(GameObject gameObject)
    {
        int hashKey = gameObject.GetHashCode();
        if (this.objectList.ContainsKey(hashKey))
        {
            GameObject.Destroy(gameObject);
            this.objectList.Remove(hashKey);
        }
    }

    /// <summary>
    /// 销毁对象
    /// </summary>
    public void Destory()
    {
        IList<PoolItemTime> poolList = new List<PoolItemTime>();
        foreach (PoolItemTime poolItemTime in this.objectList.Values)
        {
            poolList.Add(poolItemTime);
        }
        while (poolList.Count > 0)
        {
            if (poolList[0] != null && poolList[0].gameObject != null)
            {
                GameObject.Destroy(poolList[0].gameObject);
                poolList.RemoveAt(0);
            }
        }
        this.objectList = new Dictionary<int, PoolItemTime>();
    }

    /// <summary>
    /// 超时检测
    /// </summary>
    public void ExpireObject()
    {
        IList<PoolItemTime> expireList = new List<PoolItemTime>();
        foreach (PoolItemTime poolItemTime in this.objectList.Values)
        {
            if (poolItemTime.IsExpire()) expireList.Add(poolItemTime);
        }
        int expireCount = expireList.Count;
        for (int index = 0; index < expireCount; index++)
        {
            this.RemoveObject(expireList[index].gameObject);
        }
    }
}
