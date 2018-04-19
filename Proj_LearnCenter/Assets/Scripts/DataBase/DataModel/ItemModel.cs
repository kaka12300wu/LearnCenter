using System;
using System.Collections.Generic;

public class ItemModel
{
    Dictionary<uint, ItemData> dicItem;

    public void Init(ItemData[] allItems = null)
    {
        if (null == allItems)
            return;
        dicItem = new Dictionary<uint, ItemData>();
        foreach(ItemData item in allItems)
        {
            dicItem.Add(item.propID,item);
        }
    }

    public void AddItem(ItemData item)
    {
        if (dicItem.ContainsKey(item.propID))
            dicItem[item.propID].count += item.count;
        else
            dicItem.Add(item.propID, item);
    }

    public void DeleteItem(uint propID, ushort count)
    {
        if (dicItem.ContainsKey(propID))
        {
            dicItem[propID].count -= count;
            if (dicItem[propID].count <= 0)
                dicItem.Remove(propID);
        }
    }




}
