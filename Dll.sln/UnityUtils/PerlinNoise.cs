using UnityEngine;

namespace Dll.UnityUtils
{
    public static class PerlinNoise
    {
        public enum LerpType
        {
            //线性插值
            Line,
            //hermite插值
            Hermite
        }
        static float Interpolate(float a0, float a1, float w, LerpType lerpType = LerpType.Hermite)
        {
            if (lerpType == LerpType.Line)
            {
                //线性插值
                return (a1 - a0) * w + a0;
            }
            else
            {
                //hermite插值
                return Mathf.SmoothStep(a0, a1, w);
            }
        }


        static Vector2 RandomVector2(Vector2 p)
        {
            float random = Mathf.Sin(666 + p.x * 5678 + p.y * 1234) * 4321;
            return new Vector2(Mathf.Sin(random), Mathf.Cos(random));
        }


        static float DotGridGradient(Vector2 p1, Vector2 p2)
        {
            Vector2 gradient = RandomVector2(p1);
            Vector2 offset = p2 - p1;
            return Vector2.Dot(gradient, offset) / 2 + 0.5f;
        }


        public static float Perlin(float x, float y, LerpType lerpType = LerpType.Line)
        {
            //声明二维坐标
            Vector2 pos = new Vector2(x, y);
            //声明该点所处的'格子'的四个顶点坐标
            Vector2 rightUp = new Vector2((int)x + 1, (int)y + 1);
            Vector2 rightDown = new Vector2((int)x + 1, (int)y);
            Vector2 leftUp = new Vector2((int)x, (int)y + 1);
            Vector2 leftDown = new Vector2((int)x, (int)y);

            //计算x上的插值
            float v1 = DotGridGradient(leftDown, pos);
            float v2 = DotGridGradient(rightDown, pos);
            float interpolation1 = Interpolate(v1, v2, x - (int)x, lerpType);

            //计算y上的插值
            float v3 = DotGridGradient(leftUp, pos);
            float v4 = DotGridGradient(rightUp, pos);
            float interpolation2 = Interpolate(v3, v4, x - (int)x, lerpType);

            float value = Interpolate(interpolation1, interpolation2, y - (int)y, lerpType);
            return value;
        }
    }
}