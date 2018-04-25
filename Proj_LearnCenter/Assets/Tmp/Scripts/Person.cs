using System;
using UnityEngine;
using ZSerializer;

[Serializable]
public class Person
{
	public ZVector3 pos;
	public int lv;
	public string name;
	public float speed;
	public string desc;
    private string val;
    public string BigVal
    {
        get { return val; }
        set { val = value; }
    }


	public override string ToString ()
	{
		return string.Format ("{0}:Lv.{1},speed-{2},desc-{3},pos-{4},BigVal-{5}", name, lv, speed, desc, pos,BigVal);
	}
}

[Serializable]
public class ZVector3
{
	public float x;
	public float y;
    public float z;

	public ZVector3 ()
	{

	}
	public ZVector3 (Vector3 v)
	{
		x = v.x;
		y = v.y;
        z = v.z;
	}

	public ZVector3 (float _x, float _y, float _z)
	{
		x = _x;
		y = _y;
        z = _z;
	}

	public Vector3 ToVector3 ()
	{
		return new Vector3(x, y, z);
	}

	public override string ToString ()
	{
		return ToVector3 ().ToString ();
	}
}