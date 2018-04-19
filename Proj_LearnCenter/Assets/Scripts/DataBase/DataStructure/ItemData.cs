using System;
using System.Collections.Generic;
using ProtoBuf;


[ProtoContract]
public class ItemData
{
    [ProtoMember(1)]
    public uint propID;
    [ProtoMember(2)]
    public ushort count;
    [ProtoMember(3)]
    public byte pos;

    public ProtoStructure ToPST()
    {
        ProtoStructure pst = new ProtoStructure();
        pst.list = new List<ProtoStructureTest>();
        ProtoStructureTest propIDPstElem = new ProtoStructureTest();
        propIDPstElem.name = "propID";
        propIDPstElem.value = propID;
        pst.list.Add(propIDPstElem);

        ProtoStructureTest elemCount = new ProtoStructureTest();
        elemCount.name = "count";
        elemCount.value = count;
        pst.list.Add(elemCount);

        ProtoStructureTest elemPos = new ProtoStructureTest();
        elemPos.name = "pos";
        elemPos.value = pos;
        pst.list.Add(elemPos);

        return pst;
    }
}
