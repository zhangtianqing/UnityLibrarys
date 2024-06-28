using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[Serializable]
public class V2
{
    [SerializeField]
    public double x = 0;
    [SerializeField]
    public double y = 0;
    public static V2 Map2V2(Vector2 ver, int bit = 2)
    {
        return Map2V2(ver.x, ver.y, bit);
    }
    public static V2 Map2V2(float x, float y, int bit = 2)
    {
        return new V2() { x = Math.Round(x, bit), y = Math.Round(y, bit) };
    }

    public static Vector2 Map2Vector2(V2 vec, int bit = 2)
    {
        return new Vector2() { x = (float)Math.Round(vec.x, bit), y = (float)Math.Round(vec.y, bit) };
    }
    public static Vector2 Map2Vector2(float x, float y, int bit = 2)
    {
        return new Vector2() { x = (float)Math.Round(x, bit), y = (float)Math.Round(y, bit) };
    }

    public Vector2 GetVector2(int bit = 2)
    {
        return new Vector2() { x = (float)Math.Round(x, bit), y = (float)Math.Round(y, bit) };
    }
    public Vector3 GetVector3(int bit = 2)
    {
        return new Vector3() { x = (float)Math.Round(x, bit), y = (float)Math.Round(y, bit), z = 0 };
    }
}