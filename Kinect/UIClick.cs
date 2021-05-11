
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using com.rfilkov.kinect;

namespace Assets.Script.Tools.Kinect
{

    /// <summary>
    /// 用Kinect实现UI的点击：追踪左手手掌，移动到UI矩形内握拳表示点击该按钮。
    /// </summary>
    public class UIClick : MonoBehaviour
    {

        public Canvas canvas;
        public Image leftHandImage;  // 表示左手
        public Image rightHandImage;  // 表示左手
        public Image btnImage;       // 要被点击的UI控件

        KinectManager _manager;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_manager == null)
            {
                _manager = KinectManager.Instance;
            }

            // 是否初始化完成
            if (_manager && _manager.IsInitialized())
            {
                // 是否人物被检测到
                if (_manager.IsUserDetected(0))
                {
                    // 获取用户ID
                    long userId = (long)_manager.GetPrimaryUserID();
                    // 获取目标关节点的索引（以左手为例）
                    int jointIndex = (int)KinectInterop.JointType.HandLeft;
                    // 判断目标关节点是否被追踪
                    if (_manager.IsJointTracked((ulong)userId, jointIndex))
                    {
                        // 获取目标关节点在Kinect坐标系（世界坐标）的位置
                        Vector3 leftHandPos = _manager.GetJointKinectPosition((ulong)userId, KinectInterop.JointType.HandLeft, false);
                        // 左手的世界坐标 --> 屏幕坐标
                        Vector3 leftHandScreenPos = Camera.main.WorldToScreenPoint(leftHandPos);
                        Vector2 leftHandScreenPosTemp = new Vector2(leftHandScreenPos.x, leftHandScreenPos.y); // 降维

                        // 判断左手的UGUI坐标是否在Canvas所表示的矩形内
                        Vector2 leftHandUguiPos;
                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, leftHandScreenPosTemp, null, out leftHandUguiPos))
                        {
                            RectTransform leftHandRTF = rightHandImage.transform as RectTransform;
                            // 屏幕坐标 --> UGUI坐标
                            leftHandRTF.anchoredPosition = leftHandUguiPos;
                        }

                        // 判断左手的UGUI坐标是否在Button所表示的矩形内
                        if (RectTransformUtility.RectangleContainsScreenPoint(btnImage.rectTransform, leftHandScreenPosTemp, null))
                        {
                            Debug.Log("在按钮中");
                            // 获取左手的手势状态
                            KinectInterop.HandState leftHandState = _manager.GetLeftHandState((ulong)userId);
                            if (leftHandState == KinectInterop.HandState.Closed)
                            {
                                Debug.Log("左手握拳");
                                // todo：点击按钮触发的事件
                            }
                        }
                        else
                        {
                            Debug.Log("在按钮外");
                        }
                    }
                }
            }
        }
    }
}