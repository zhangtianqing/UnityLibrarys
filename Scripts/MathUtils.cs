using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtils
{
    /// <summary>
    /// 范围判断-三维
    /// </summary>
    /// <param name="input"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static bool VectorInRange(Vector3 input, Vector3 min, Vector3 max)
    {
        return input.x >= min.x && input.x <= max.x
            && input.y >= min.y && input.y <= max.y
            && input.z >= min.z && input.z <= max.z
        ;
    }
    /// <summary>
    /// 范围判断-二维
    /// </summary>
    /// <param name="input"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static bool VectorInRange(Vector2 input, Vector2 min, Vector2 max)
    {
        return input.x >= min.x && input.x <= max.x
            && input.y >= min.y && input.y <= max.y
        ;
    }

    /// <summary>
    /// 数值重映射
    /// </summary>
    /// <param name="value">输入值</param>
    /// <param name="inputMin">输入最小值</param>
    /// <param name="inputMax">输入最大值</param>
    /// <param name="outputMin">输出最小值</param>
    /// <param name="outputMax">输出最大值</param>
    /// <param name="InRange">限制输出值在输出区间</param>
    /// <returns></returns>
    public static float ReMap(float value, float inputMin, float inputMax, float outputMin, float outputMax, bool InRange = true)
    {
        float returnValue = (value - inputMin) / (inputMax - inputMin);
        returnValue = (outputMax - outputMin) * returnValue + outputMin;
        if (InRange)
        {
            if (returnValue < outputMin)
            {
                return outputMin;
            }
            if (returnValue > outputMax)
            {
                return outputMax;
            }
        }

        return returnValue;
    }
    public static Vector3 ReMap(Vector3 input, Vector3 minInput, Vector3 maxInput, Vector3 minOutput, Vector3 maxOutput)
    {
        return new Vector3(
                ReMap(input.x, minInput.x, maxInput.x, minOutput.x, maxOutput.x),
                ReMap(input.y, minInput.y, maxInput.y, minOutput.y, maxOutput.y),
                ReMap(input.z, minInput.z, maxInput.z, minOutput.z, maxOutput.z)
            );
    }

    /// <summary>
    /// 重映射 默认输入范围为1-100
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxValue"></param>
    /// <param name="minValue"></param>
    /// <returns></returns>
    private static float ReMap(float value, float outputMin, float outputMax)
    {
        float returnValue = (value - 1.0f) / (100.0f - 1.0f);
        returnValue = (outputMax - outputMin) * returnValue + outputMin;
        return returnValue;
    }
    #region 辅助操作 - 屏幕坐标转世界坐标、根据三个点获取物体重心、根据三个点获取中心点的位置
    /// <summary>
    /// 屏幕坐标转世界坐标
    /// </summary>
    /// <param name="screenPos"></param>
    /// <returns></returns>
    public Vector3 GetWorldPos(Vector2 screenPos)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane + 25));
    }
    #endregion
}
