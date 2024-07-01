using UnityEngine;

namespace Dll.UnityUtils
{
    /// <summary>
    /// 数据转换工具类
    /// </summary>
    public class MathUtils
    {
        #region 范围判断
        /// <summary>
        /// 范围判断-三维
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(Vector3 input, Vector3 min, Vector3 max) => InRange(input.x, min.x, max.x) && InRange(input.y, min.y, max.y) && InRange(input.z, min.z, max.z);
        /// <summary>
        /// 范围判断-二维
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(Vector2 input, Vector2 min, Vector2 max) => InRange(input.x, min.x, max.x) && InRange(input.y, min.y, max.y);
        /// <summary>
        /// 范围判断-一维
        /// </summary>
        /// <param name="input"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="whenMinGreaterMax"></param>
        /// <returns></returns>
        public static bool InRange(float input, float minValue, float maxValue, bool whenMinGreaterMax = true)
        {
            if (whenMinGreaterMax && minValue > maxValue)
            {
                return maxValue < input && input < minValue;
            }
            return minValue < input && input < maxValue;
        }
        #endregion

        #region 数值重映射
        /// <summary>
        /// 三维向量重映射
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="minInput">输入最小值</param>
        /// <param name="maxInput">输入最大值</param>
        /// <param name="minOutput">输出最小值</param>
        /// <param name="maxOutput">输出最大值</param>
        /// <returns></returns>
        public static Vector3 ReMap(Vector3 input, Vector3 minInput, Vector3 maxInput, Vector3 minOutput, Vector3 maxOutput)
        {
            return new Vector3(
                    ReMap(input.x, minInput.x, maxInput.x, minOutput.x, maxOutput.x),
                    ReMap(input.y, minInput.y, maxInput.y, minOutput.y, maxOutput.y),
                    ReMap(input.z, minInput.z, maxInput.z, minOutput.z, maxOutput.z)
                );
        }
        /// <summary>
        /// 二维向量重映射
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="minInput">输入最小值</param>
        /// <param name="maxInput">输入最大值</param>
        /// <param name="minOutput">输出最小值</param>
        /// <param name="maxOutput">输出最大值</param>
        /// <returns></returns>
        public static Vector2 ReMap(Vector2 input, Vector2 minInput, Vector2 maxInput, Vector2 minOutput, Vector2 maxOutput)
        {
            return new Vector2(
                    ReMap(input.x, minInput.x, maxInput.x, minOutput.x, maxOutput.x),
                    ReMap(input.y, minInput.y, maxInput.y, minOutput.y, maxOutput.y)
                );
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
        }/// <summary>
         /// 重映射 默认输入范围为0-1
         /// </summary>
         /// <param name="value">输入值</param>
         /// <param name="outputMin">输出最小值</param>
         /// <param name="outputMax">输出最大值</param>
         /// <returns></returns>
        public static float ReMap(float value, float outputMin, float outputMax, bool inRange = true)
        {
            float returnValue = value / 1.0f;
            returnValue = (outputMax - outputMin) * returnValue + outputMin;
            if (!inRange)
            {
                return returnValue;
            }
            if (returnValue < outputMin)
            {
                return outputMin;
            }
            if (returnValue > outputMax)
            {
                return outputMax;
            }
            return returnValue;
        }
        #endregion

        /// <summary>
        /// 判断相等
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static bool Eq(Vector3 vector1, Vector3 vector2, float threshold = 0.0001f)
        {
            return Mathf.Abs(vector1.x - vector2.x) < threshold &&
                     Mathf.Abs(vector1.y - vector2.y) < threshold &&
                     Mathf.Abs(vector1.z - vector2.z) < threshold;
        }
        #region 辅助操作 - 屏幕坐标转世界坐标
        /// <summary>
        /// 屏幕坐标转世界坐标
        /// </summary>
        /// <param name="screenPos">2D屏幕位置</param>
        /// <returns></returns>
        public static Vector3 GetWorldPos(Camera camera, Vector2 screenPos, float dis = 25)
        {
            return camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, dis));
        }
        #endregion
    }
}