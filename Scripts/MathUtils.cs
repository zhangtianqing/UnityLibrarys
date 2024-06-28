using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtils
{
    /// <summary>
    /// ��Χ�ж�-��ά
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
    /// ��Χ�ж�-��ά
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
    /// ��ֵ��ӳ��
    /// </summary>
    /// <param name="value">����ֵ</param>
    /// <param name="inputMin">������Сֵ</param>
    /// <param name="inputMax">�������ֵ</param>
    /// <param name="outputMin">�����Сֵ</param>
    /// <param name="outputMax">������ֵ</param>
    /// <param name="InRange">�������ֵ���������</param>
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
    /// ��ӳ�� Ĭ�����뷶ΧΪ1-100
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
    #region �������� - ��Ļ����ת�������ꡢ�����������ȡ�������ġ������������ȡ���ĵ��λ��
    /// <summary>
    /// ��Ļ����ת��������
    /// </summary>
    /// <param name="screenPos"></param>
    /// <returns></returns>
    public Vector3 GetWorldPos(Vector2 screenPos)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane + 25));
    }
    #endregion
}
