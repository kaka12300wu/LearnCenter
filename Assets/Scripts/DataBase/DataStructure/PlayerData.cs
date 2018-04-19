using System;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class PlayerData
{
    [ProtoMember(1)]
    public Guid guid;
    [ProtoMember(2)]
    public string name;
    [ProtoMember(3)]
    public uint propID;
    [ProtoMember(4)]
    public uint lastLogin;
    [ProtoMember(5)]
    public short lv;
}
