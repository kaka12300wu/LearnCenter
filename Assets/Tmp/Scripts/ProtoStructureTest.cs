using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

[ProtoContract]
public class ProtoStructureTest
{
    [ProtoMember(1)]
    public string name;
    [ProtoMember(2)]
    public Object value;
}

[ProtoContract]
public class ProtoStructure
{
    [ProtoMember(1)]
    public List<ProtoStructureTest> list;
}
