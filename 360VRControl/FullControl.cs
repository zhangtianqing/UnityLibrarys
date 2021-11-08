﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///
///360VRControl 360全景控制
///
public class FullControl : MonoBehaviour
{
    public float moveSpeed = 1;//物体旋转速度  
    public GameObject target;

    private Vector2 oldPosition;
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;


    public float distance = 0;//max 1.14  min -0.6
    private readonly float DisStep = 0.1f;
    //private bool flag = false;
    //摄像头的位置
    private float x = 0f;
    private float y = 0f;
    //左右滑动移动速度
    public float xSpeed = 250f;
    public float ySpeed = 120f;
    //缩放限制系数
    public float yMinLimit = -360;
    public float yMaxLimit = 360;
    //是否旋转
    private bool isRotate = true;
    //计数器
    private float count = 0;

    public static FullControl _instance;
    //初始化游戏信息设置
    void Start()
    {
        _instance = this;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }



    // Update is called once per frame  
    void Update()
    {
        #region default
        if (isRotate)
        {
            target.transform.Rotate(Vector3.down, Time.deltaTime * moveSpeed, Space.World);
        }
        if (!isRotate)
        {
            count += Time.deltaTime;
            if (count > 5)
            {
                count = 0;
                isRotate = true;
            }
        }
        #endregion
        #region TouchControl
        //触摸类型为移动触摸  单点
        if (Input.touchCount == 1)
        {
            Touch touch = Input.touches[0];
            //根据触摸点计算X与Y位置
            x += touch.position.x * xSpeed * Time.deltaTime;
            y -= -touch.position.y * ySpeed * Time.deltaTime;
            isRotate = false;
        }
        //触摸类型为双点 放大放小
        if (Input.touchCount >= 2)
        {
            Touch touch1 = Input.touches[0],
                  touch2 = Input.touches[1];
            if (IsEnlarge(touch1.deltaPosition, touch2.deltaPosition, touch1.position, touch2.position))
            {
                // 这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                if (distance > -0.6 + DisStep)
                {
                    distance -= DisStep;
                }
            }
            else
            {
                // 这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                if (distance < 1.1 - DisStep)
                {
                    distance += DisStep;
                }
            }
        }

        #endregion
        #region MouseControl
        //触摸类型为移动触摸
        if (Input.GetMouseButton(0))
        {
            //根据触摸点计算X与Y位置
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= -Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
            isRotate = false;
        }
        //判断鼠标滑轮是否输入
        float temp = Input.GetAxis("Mouse ScrollWheel");
        if (temp != 0)
        {
            if (temp > 0)
            {
                // 这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                if (distance > -0.6 + DisStep)
                {
                    distance -= DisStep;
                }
            }
            if (temp < 0)
            {
                // 这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                if (distance < 1.1 - DisStep)
                {
                    distance += DisStep;
                }
            }
        }
        #endregion

    }

    //计算距离，判断放大还是缩小。放大返回true，缩小返回false  
    bool IsEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        //old distance  
        float oldDistance = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        //new distance  
        float newDistance = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));

        if (oldDistance < newDistance)
        {
            //zoom+  
            return true;
        }
        else
        {
            //zoom-  
            return false;
        }
    }

    //每帧执行，在Update后  
    void LateUpdate()
    {
        if (target)
        {
            //重置摄像机的位置
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * (new Vector3(0.0f, 0.0f, -distance)) + target.transform.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);

    }

}
