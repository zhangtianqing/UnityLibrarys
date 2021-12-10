using UnityEngine;
//using Windows.Kinect;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// This interface needs to be implemented by the Kinect gesture managers, like KinectGestures-class itself
/// </summary>
public interface GestureManagerInterface
{
    /// <summary>
    /// Gets the list of gesture joint indexes.
    /// </summary>
    /// <returns>The needed joint indexes.</returns>
    /// <param name="manager">The KinectManager instance</param>
    int[] GetNeededJointIndexes(KinectManager manager);


    /// <summary>
    /// Estimate the state and progress of the given gesture.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="gestureData">Gesture-data structure</param>
    /// <param name="timestamp">Current time</param>
    /// <param name="jointsPos">Joints-position array</param>
    /// <param name="jointsTracked">Joints-tracked array</param>
    void CheckForGesture(long userId, ref KinectGestures.GestureData gestureData, float timestamp, ref Vector3[] jointsPos, ref bool[] jointsTracked);
}


/// <summary>
/// KinectGestures is utility class that processes programmatic Kinect gestures
/// </summary>
public class KinectGestures : MonoBehaviour, GestureManagerInterface
{
    public Text text;

    /// <summary>
    /// This interface needs to be implemented by all Kinect gesture listeners
    /// </summary>
    public interface GestureListenerInterface
    {
        /// <summary>
        /// Invoked when a new user is detected. Here you can start gesture tracking by invoking KinectManager.DetectGesture()-function.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        void UserDetected(long userId, int userIndex);

        /// <summary>
        /// Invoked when a user gets lost. All tracked gestures for this user are cleared automatically.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        void UserLost(long userId, int userIndex);

        /// <summary>
        /// Invoked when a gesture is in progress.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        /// <param name="gesture">Gesture type</param>
        /// <param name="progress">Gesture progress [0..1]</param>
        /// <param name="joint">Joint type</param>
        /// <param name="screenPos">Normalized viewport position</param>
        void GestureInProgress(long userId, int userIndex, Gestures gesture, float progress,
                               KinectInterop.JointType joint, Vector3 screenPos);

        /// <summary>
        /// Invoked if a gesture is completed.
        /// </summary>
        /// <returns><c>true</c>, if the gesture detection must be restarted, <c>false</c> otherwise.</returns>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        /// <param name="gesture">Gesture type</param>
        /// <param name="joint">Joint type</param>
        /// <param name="screenPos">Normalized viewport position</param>
        bool GestureCompleted(long userId, int userIndex, Gestures gesture,
                              KinectInterop.JointType joint, Vector3 screenPos);

        /// <summary>
        /// Invoked if a gesture is cancelled.
        /// </summary>
        /// <returns><c>true</c>, if the gesture detection must be retarted, <c>false</c> otherwise.</returns>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        /// <param name="gesture">Gesture type</param>
        /// <param name="joint">Joint type</param>
        bool GestureCancelled(long userId, int userIndex, Gestures gesture,
                              KinectInterop.JointType joint);
    }


    /// <summary>
    /// The gesture types.
    /// </summary>
    public enum Gestures
    {
        None = 0,
        RaiseRightHand,
        RaiseLeftHand,
        Psi,
        Tpose,
        Stop,
        Wave,
        //		Click,
        SwipeLeft,
        SwipeRight,
        SwipeUp,
        SwipeDown,
        //		RightHandCursor,
        //		LeftHandCursor,
        ZoomIn,
        ZoomOut,
        Wheel,
        Jump,
        Squat,
        Push,
        Pull,
        ShoulderLeftFront,
        ShoulderRightFront,
        LeanLeft,
        LeanRight,
        LeanForward,
        LeanBack,
        KickLeft,
        KickRight,
        Run,

        RaisedRightHorizontalLeftHand,   // by Andrzej W
        RaisedLeftHorizontalRightHand,

        TouchRightElbow,   // suggested by Nayden N.
        TouchLeftElbow,

        MoveLeft,          // suggested by Indra Adi D. C
        MoveRight,
        /// <summary>
        /// 初始动作
        /// </summary>
        DefaultMotion,
        /// <summary>
        /// 垂直小臂于地面
        /// </summary>
        VerticalForearm,

        /// <summary>
        /// 垂直小臂于地面
        /// </summary>
        TouchShoulder,


        /// <summary>
        ///左手举起不超过45°（大于15小于45都视作做了）
        /// </summary>
        RaiseLeftHandLift45,
        /// <summary>
        /// 举起左手放到胸前附近
        /// </summary>
        RaiseLeftHandParallelChest,
        /// <summary>
        /// 双臂朝下45°
        /// </summary>
        DownBothHand45,

        /// <summary>
        /// 双臂朝上45°，略微向两边张开
        /// </summary>
        RaiseBothHand45,

        /// <summary>
        /// 跪姿，上半身直立
        /// </summary>
        Kneel,

        /// <summary>
        /// 判断伤者意识
        /// </summary>
        KneelAndJudegeConsciousness,

        /// <summary>
        /// cpr
        /// </summary>
        KneelAndCPR,

        /// <summary>
        /// 检查喉咙
        /// </summary>
        KneelAndCheckThroat,

        /// <summary>
        /// 检查是否恢复
        /// </summary>
        CheckIsRecovery,
        /// <summary>
        /// 期间是否恢复呼吸和关怀
        /// </summary>
        CheckBreathe,
        /// <summary>
        /// 检查情况
        /// </summary>
        CheckCondition,

        /// <summary>
        /// 结束动作
        /// </summary>
        KneelDefault,


        UserGesture1 = 101,
        UserGesture2 = 102,
        UserGesture3 = 103,
        UserGesture4 = 104,
        UserGesture5 = 105,
        UserGesture6 = 106,
        UserGesture7 = 107,
        UserGesture8 = 108,
        UserGesture9 = 109,
        UserGesture10 = 110,
    }


    /// <summary>
    /// Programmatic gesture data container.
    /// </summary>
    public struct GestureData
    {
        public long userId;
        public Gestures gesture;
        public int state;
        public float timestamp;
        public int joint;
        public Vector3 jointPos;
        public Vector3 screenPos;
        public float tagFloat;
        public Vector3 tagVector;
        public Vector3 tagVector2;
        public float progress;
        public bool complete;
        public bool cancelled;
        public List<Gestures> checkForGestures;
        public float startTrackingAtTime;
    }


    // Gesture related constants, variables and functions
    protected int leftHandIndex;
    protected int rightHandIndex;

    protected int leftFingerIndex;
    protected int rightFingerIndex;

    protected int leftElbowIndex;
    protected int rightElbowIndex;

    protected int leftShoulderIndex;
    protected int rightShoulderIndex;

    protected int hipCenterIndex;
    protected int shoulderCenterIndex;

    protected int leftHipIndex;
    protected int rightHipIndex;

    protected int leftKneeIndex;
    protected int rightKneeIndex;

    protected int leftAnkleIndex;
    protected int rightAnkleIndex;
    /// <summary>
    /// 脖子骨骼点左边
    /// </summary>
    protected int neckIndex;
    protected int leftWristIndex;
    protected int RightWristIndex;
    /// <summary>
    /// 两个肩膀中间的骨骼点
    /// </summary>
    protected int spineShoulderIndex;

    protected int headIndex;

    /// <summary>
    /// 两个肩膀中间的骨骼点
    /// </summary>
    protected int spineMidIndex;

    /// <summary>
    /// 腰眼的骨骼点
    /// </summary>
    protected int spineBaseIndex;





    /// <summary>
    /// Gets the list of gesture joint indexes.
    /// </summary>
    /// <returns>The needed joint indexes.</returns>
    /// <param name="manager">The KinectManager instance</param>
    public virtual int[] GetNeededJointIndexes(KinectManager manager)
    {
        //		if (manager == null)
        //			return new int[0];

        leftHandIndex = (int)KinectInterop.JointType.HandLeft;
        rightHandIndex = (int)KinectInterop.JointType.HandRight;

        leftFingerIndex = (int)KinectInterop.JointType.HandTipLeft;
        rightFingerIndex = (int)KinectInterop.JointType.HandTipRight;

        leftElbowIndex = (int)KinectInterop.JointType.ElbowLeft;
        rightElbowIndex = (int)KinectInterop.JointType.ElbowRight;

        leftShoulderIndex = (int)KinectInterop.JointType.ShoulderLeft;
        rightShoulderIndex = (int)KinectInterop.JointType.ShoulderRight;

        hipCenterIndex = (int)KinectInterop.JointType.SpineBase;
        shoulderCenterIndex = (int)KinectInterop.JointType.SpineShoulder;

        leftHipIndex = (int)KinectInterop.JointType.HipLeft;
        rightHipIndex = (int)KinectInterop.JointType.HipRight;

        leftKneeIndex = (int)KinectInterop.JointType.KneeLeft;
        rightKneeIndex = (int)KinectInterop.JointType.KneeRight;

        leftAnkleIndex = (int)KinectInterop.JointType.AnkleLeft;
        rightAnkleIndex = (int)KinectInterop.JointType.AnkleRight;

        neckIndex = (int)KinectInterop.JointType.Neck;

        leftWristIndex = (int)KinectInterop.JointType.WristLeft;
        RightWristIndex = (int)KinectInterop.JointType.WristRight;

        spineShoulderIndex = (int)KinectInterop.JointType.SpineShoulder;
        spineMidIndex = (int)KinectInterop.JointType.SpineMid;
        spineBaseIndex = (int)KinectInterop.JointType.SpineBase;
        headIndex = (int)KinectInterop.JointType.Head;



        int[] neededJointIndexes = {
            leftHandIndex, rightHandIndex, leftFingerIndex, rightFingerIndex, leftElbowIndex, rightElbowIndex, leftShoulderIndex, rightShoulderIndex,
            hipCenterIndex, shoulderCenterIndex, leftHipIndex, rightHipIndex, leftKneeIndex, rightKneeIndex, leftAnkleIndex, rightAnkleIndex, neckIndex,
            leftWristIndex, RightWristIndex, spineShoulderIndex
        };

        return neededJointIndexes;
    }


    protected void SetGestureJoint(ref GestureData gestureData, float timestamp, int joint, Vector3 jointPos)
    {
        gestureData.joint = joint;
        gestureData.jointPos = jointPos;
        gestureData.timestamp = timestamp;
        gestureData.state++;
    }

    protected void SetGestureCancelled(ref GestureData gestureData)
    {
        gestureData.state = 0;
        gestureData.progress = 0f;
        gestureData.cancelled = true;
    }

    protected void CheckPoseComplete(ref GestureData gestureData, float timestamp, Vector3 jointPos, bool isInPose, float durationToComplete)
    {
        if (isInPose)
        {
            float timeLeft = timestamp - gestureData.timestamp;
            gestureData.progress = durationToComplete > 0f ? Mathf.Clamp01(timeLeft / durationToComplete) : 1.0f;

            if (timeLeft >= durationToComplete)
            {
                gestureData.timestamp = timestamp;
                gestureData.jointPos = jointPos;
                gestureData.state++;
                gestureData.complete = true;
            }
        }
        else
        {
            SetGestureCancelled(ref gestureData);
        }
    }

    protected void SetScreenPos(long userId, ref GestureData gestureData, ref Vector3[] jointsPos, ref bool[] jointsTracked)
    {
        Vector3 handPos = jointsPos[rightHandIndex];
        bool calculateCoords = false;

        if (gestureData.joint == rightHandIndex)
        {
            if (jointsTracked[rightHandIndex] /**&& jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex]*/)
            {
                calculateCoords = true;
            }
        }
        else if (gestureData.joint == leftHandIndex)
        {
            if (jointsTracked[leftHandIndex] /**&& jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex]*/)
            {
                handPos = jointsPos[leftHandIndex];
                calculateCoords = true;
            }
        }

        if (calculateCoords)
        {
            if (jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] &&
                jointsTracked[leftShoulderIndex] && jointsTracked[rightShoulderIndex])
            {
                Vector3 shoulderToHips = jointsPos[shoulderCenterIndex] - jointsPos[hipCenterIndex];
                Vector3 rightToLeft = jointsPos[rightShoulderIndex] - jointsPos[leftShoulderIndex];

                gestureData.tagVector2.x = rightToLeft.x; // * 1.2f;
                gestureData.tagVector2.y = shoulderToHips.y; // * 1.2f;

                if (gestureData.joint == rightHandIndex)
                {
                    gestureData.tagVector.x = jointsPos[rightShoulderIndex].x - gestureData.tagVector2.x / 2;
                    gestureData.tagVector.y = jointsPos[hipCenterIndex].y;
                }
                else
                {
                    gestureData.tagVector.x = jointsPos[leftShoulderIndex].x - gestureData.tagVector2.x / 2;
                    gestureData.tagVector.y = jointsPos[hipCenterIndex].y;
                }
            }

            if (gestureData.tagVector2.x != 0 && gestureData.tagVector2.y != 0)
            {
                Vector3 relHandPos = handPos - gestureData.tagVector;
                gestureData.screenPos.x = Mathf.Clamp01(relHandPos.x / gestureData.tagVector2.x);
                gestureData.screenPos.y = Mathf.Clamp01(relHandPos.y / gestureData.tagVector2.y);
            }

        }
    }

    protected void SetZoomFactor(long userId, ref GestureData gestureData, float initialZoom, ref Vector3[] jointsPos, ref bool[] jointsTracked)
    {
        Vector3 vectorZooming = jointsPos[rightHandIndex] - jointsPos[leftHandIndex];

        if (gestureData.tagFloat == 0f || gestureData.userId != userId)
        {
            gestureData.tagFloat = 0.5f; // this is 100%
        }

        float distZooming = vectorZooming.magnitude;
        gestureData.screenPos.z = initialZoom + (distZooming / gestureData.tagFloat);
    }

    protected void SetWheelRotation(long userId, ref GestureData gestureData, Vector3 initialPos, Vector3 currentPos)
    {
        float angle = Vector3.Angle(initialPos, currentPos) * Mathf.Sign(currentPos.y - initialPos.y);
        gestureData.screenPos.z = angle;
    }


    /// <summary>
    /// Estimate the state and progress of the given gesture.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="gestureData">Gesture-data structure</param>
    /// <param name="timestamp">Current time</param>
    /// <param name="jointsPos">Joints-position array</param>
    /// <param name="jointsTracked">Joints-tracked array</param>
    public virtual void CheckForGesture(long userId, ref GestureData gestureData, float timestamp, ref Vector3[] jointsPos, ref bool[] jointsTracked)
    {
        if (gestureData.complete)
            return;

        float bandTopY = jointsPos[rightShoulderIndex].y > jointsPos[leftShoulderIndex].y ? jointsPos[rightShoulderIndex].y : jointsPos[leftShoulderIndex].y;
        float bandBotY = jointsPos[rightHipIndex].y < jointsPos[leftHipIndex].y ? jointsPos[rightHipIndex].y : jointsPos[leftHipIndex].y;

        float bandCenter = (bandTopY + bandBotY) / 2f;
        float bandSize = (bandTopY - bandBotY);

        float gestureTop = bandCenter + bandSize * 1.2f / 2f;
        float gestureBottom = bandCenter - bandSize * 1.3f / 4f;
        float gestureRight = jointsPos[rightHipIndex].x;
        float gestureLeft = jointsPos[leftHipIndex].x;

        switch (gestureData.gesture)
        {
            // check for RaiseRightHand
            case Gestures.RaiseRightHand:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[rightHandIndex] && jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
                            (jointsPos[rightHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.1f &&
                               (jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) < 0f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                        }
                        break;

                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
                            (jointsPos[rightHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.1f &&
                            (jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) < 0f;

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
                        break;
                }
                break;

            // check for RaiseLeftHand
            case Gestures.RaiseLeftHand:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
                            (jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f &&
                               (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                        }
                        break;

                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
                            (jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f &&
                            (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0f;

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
                        break;
                }
                break;

            // check for Psi
            case Gestures.Psi:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[rightHandIndex] && jointsTracked[leftHandIndex] && jointsTracked[shoulderCenterIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[shoulderCenterIndex].y) > 0.1f &&
                           (jointsPos[leftHandIndex].y - jointsPos[shoulderCenterIndex].y) > 0.1f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                        }
                        break;

                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[leftHandIndex] && jointsTracked[shoulderCenterIndex] &&
                            (jointsPos[rightHandIndex].y - jointsPos[shoulderCenterIndex].y) > 0.1f &&
                            (jointsPos[leftHandIndex].y - jointsPos[shoulderCenterIndex].y) > 0.1f;

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
                        break;
                }
                break;

            // check for Tpose
            case Gestures.Tpose:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex] &&
                            Mathf.Abs(jointsPos[rightElbowIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.07f
                            Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.7f
                            jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex] &&
                            Mathf.Abs(jointsPos[leftElbowIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f &&
                            Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                        }
                        break;

                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex] &&
                            Mathf.Abs(jointsPos[rightElbowIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.7f
                            Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.7f
                            jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex] &&
                            Mathf.Abs(jointsPos[leftElbowIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f &&
                            Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f;

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
                        break;
                }
                break;

            // check for Stop
            case Gestures.Stop:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightHipIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y) < 0.2f &&
                              (jointsPos[rightHandIndex].x - jointsPos[rightHipIndex].x) >= 0.4f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[leftHipIndex] &&
                           (jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y) < 0.2f &&
                           (jointsPos[leftHandIndex].x - jointsPos[leftHipIndex].x) <= -0.4f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                        }
                        break;

                    case 1:  // gesture complete
                        bool isInPose = (gestureData.joint == rightHandIndex) ?
                            (jointsTracked[rightHandIndex] && jointsTracked[rightHipIndex] &&
                            (jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y) < 0.2f &&
                             (jointsPos[rightHandIndex].x - jointsPos[rightHipIndex].x) >= 0.4f) :
                            (jointsTracked[leftHandIndex] && jointsTracked[leftHipIndex] &&
                            (jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y) < 0.2f &&
                             (jointsPos[leftHandIndex].x - jointsPos[leftHipIndex].x) <= -0.4f);

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
                        break;
                }
                break;

            // check for raised right hand & horizontal left hand 
            case Gestures.RaisedRightHorizontalLeftHand:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] && // check right hand is straight up 
                           (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.5f &&  // ensure right hand is higher than shoulder 
                           Mathf.Abs(jointsPos[rightHandIndex].z - jointsPos[rightShoulderIndex].z) < 0.35f &&   // ensue hand is vertical straight enough 
                           Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightShoulderIndex].x) < 0.35f &&   // ensue hand is vertical straight enough                             
                           jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&  // check left hand is straight flat 
                           Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) < 0.25f &&       // ensure hand and shoulder are on close height 
                           (jointsPos[leftHandIndex] - jointsPos[leftShoulderIndex]).sqrMagnitude > 0.25f)  // ensure hand and shoulder are horizontal straight enough 						
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                        }
                        break;


                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] && // check right hand is straight up 
                           (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.5f &&  // ensure right hand is higher than shoulder 
                           Mathf.Abs(jointsPos[rightHandIndex].z - jointsPos[rightShoulderIndex].z) < 0.35f &&   // ensue hand is vertical straight enough 
                           Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightShoulderIndex].x) < 0.35f &&   // ensue hand is vertical straight enough                             
                           jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&  // check left hand is straight flat 
                           Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) < 0.25f &&       // ensure hand and shoulder are on close height 
                           (jointsPos[leftHandIndex] - jointsPos[leftShoulderIndex]).sqrMagnitude > 0.25f;  // ensure hand and shoulder are horizontal straight enough

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
                        break;
                }
                break;

            // check for raised left hand & horizontal right hand 
            case Gestures.RaisedLeftHorizontalRightHand:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] && // check left hand is straight up 
                           (jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.5f &&  // ensure left hand is higher than shoulder 
                           Mathf.Abs(jointsPos[leftHandIndex].z - jointsPos[leftShoulderIndex].z) < 0.35f &&   // ensue hand is vertical straight enough 
                           Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[leftShoulderIndex].x) < 0.35f &&   // ensue hand is vertical straight enough                             
                           jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&  // check right hand is straight flat 
                           Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0.25f &&       // ensure hand and shoulder are on close height 
                           (jointsPos[rightHandIndex] - jointsPos[rightShoulderIndex]).sqrMagnitude > 0.25f)  // ensure hand and shoulder are horizontal straight enough 
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                        }
                        break;

                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] && // check left hand is straight up 
                           (jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.5f &&  // ensure left hand is higher than shoulder 
                           Mathf.Abs(jointsPos[leftHandIndex].z - jointsPos[leftShoulderIndex].z) < 0.35f &&   // ensue hand is vertical straight enough 
                           Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[leftShoulderIndex].x) < 0.35f &&   // ensue hand is vertical straight enough                             
                           jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&  // check right hand is straight flat 
                           Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0.25f &&       // ensure hand and shoulder are on close height 
                           (jointsPos[rightHandIndex] - jointsPos[rightShoulderIndex]).sqrMagnitude > 0.25f;  // ensure hand and shoulder are horizontal straight enough

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
                        break;
                }
                break;

            // check for TouchedRightElbow
            case Gestures.TouchRightElbow:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[leftFingerIndex] && jointsTracked[rightElbowIndex] &&
                            Vector3.Distance(jointsPos[leftFingerIndex], jointsPos[rightElbowIndex]) <= 0.12f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftFingerIndex, jointsPos[leftFingerIndex]);
                        }

                        //Debug.Log ("TRE0 - Distance: " + Vector3.Distance(jointsPos[leftFingerIndex], jointsPos[rightElbowIndex]));
                        break;

                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[leftFingerIndex] && jointsTracked[rightElbowIndex] &&
                            Vector3.Distance(jointsPos[leftFingerIndex], jointsPos[rightElbowIndex]) <= 0.12f;

                        //Debug.Log ("TRE1 - Distance: " + Vector3.Distance(jointsPos[leftFingerIndex], jointsPos[rightElbowIndex]) + ", progress: " + gestureData.progress);

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 1.5f /**KinectInterop.Constants.PoseCompleteDuration*/);
                        break;
                }
                break;

            // check for TouchedLeftElbow
            case Gestures.TouchLeftElbow:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection
                        if (jointsTracked[rightFingerIndex] && jointsTracked[leftElbowIndex] &&
                            Vector3.Distance(jointsPos[rightFingerIndex], jointsPos[leftElbowIndex]) <= 0.12f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightFingerIndex, jointsPos[rightFingerIndex]);
                        }

                        //Debug.Log ("TLE0 - Distance: " + Vector3.Distance(jointsPos[rightFingerIndex], jointsPos[leftElbowIndex]));
                        break;

                    case 1:  // gesture complete
                        bool isInPose = jointsTracked[rightFingerIndex] && jointsTracked[leftElbowIndex] &&
                            Vector3.Distance(jointsPos[rightFingerIndex], jointsPos[leftElbowIndex]) <= 0.12f;

                        //Debug.Log ("TLE1- Distance: " + Vector3.Distance(jointsPos[rightFingerIndex], jointsPos[leftElbowIndex]));

                        Vector3 jointPos = jointsPos[gestureData.joint];
                        CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 1.5f /**KinectInterop.Constants.PoseCompleteDuration*/);
                        break;
                }
                break;

            // check for Wave
            case Gestures.Wave:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f &&
                           (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0.05f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.3f;
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
                                (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < -0.05f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture - phase 2
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f &&
                                (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) < -0.05f :
                                jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
                                (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) > 0.05f;

                            if (isInPose)
                            {
                                gestureData.timestamp = timestamp;
                                gestureData.state++;
                                gestureData.progress = 0.7f;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;

                    case 2:  // gesture phase 3 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
                                (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f &&
                                (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0.05f :
                                jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
                                (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < -0.05f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            //			// check for Click
            //			case Gestures.Click:
            //				switch(gestureData.state)
            //				{
            //					case 0:  // gesture detection - phase 1
            //						if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
            //					       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f)
            //						{
            //							SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
            //							gestureData.progress = 0.3f;
            //
            //							// set screen position at the start, because this is the most accurate click position
            //							SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
            //						}
            //						else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
            //					            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f)
            //						{
            //							SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
            //							gestureData.progress = 0.3f;
            //
            //							// set screen position at the start, because this is the most accurate click position
            //							SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
            //						}
            //						break;
            //				
            //					case 1:  // gesture - phase 2
            ////						if((timestamp - gestureData.timestamp) < 1.0f)
            ////						{
            ////							bool isInPose = gestureData.joint == rightHandIndex ?
            ////								jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
            ////								//(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f && 
            ////								Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f &&
            ////								(jointsPos[rightHandIndex].z - gestureData.jointPos.z) < -0.05f :
            ////								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
            ////								//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
            ////								Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f &&
            ////								(jointsPos[leftHandIndex].z - gestureData.jointPos.z) < -0.05f;
            ////				
            ////							if(isInPose)
            ////							{
            ////								gestureData.timestamp = timestamp;
            ////								gestureData.jointPos = jointsPos[gestureData.joint];
            ////								gestureData.state++;
            ////								gestureData.progress = 0.7f;
            ////							}
            ////							else
            ////							{
            ////								// check for stay-in-place
            ////								Vector3 distVector = jointsPos[gestureData.joint] - gestureData.jointPos;
            ////								isInPose = distVector.magnitude < 0.05f;
            ////
            ////								Vector3 jointPos = jointsPos[gestureData.joint];
            ////								CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, Constants.ClickStayDuration);
            ////							}
            ////						}
            ////						else
            //						{
            //							// check for stay-in-place
            //							Vector3 distVector = jointsPos[gestureData.joint] - gestureData.jointPos;
            //							bool isInPose = distVector.magnitude < 0.05f;
            //
            //							Vector3 jointPos = jointsPos[gestureData.joint];
            //							CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.ClickStayDuration);
            ////							SetGestureCancelled(gestureData);
            //						}
            //						break;
            //									
            ////					case 2:  // gesture phase 3 = complete
            ////						if((timestamp - gestureData.timestamp) < 1.0f)
            ////						{
            ////							bool isInPose = gestureData.joint == rightHandIndex ?
            ////								jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
            ////								//(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f && 
            ////								Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f &&
            ////								(jointsPos[rightHandIndex].z - gestureData.jointPos.z) > 0.05f :
            ////								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
            ////								//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
            ////								Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f &&
            ////								(jointsPos[leftHandIndex].z - gestureData.jointPos.z) > 0.05f;
            ////
            ////							if(isInPose)
            ////							{
            ////								Vector3 jointPos = jointsPos[gestureData.joint];
            ////								CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
            ////							}
            ////						}
            ////						else
            ////						{
            ////							// cancel the gesture
            ////							SetGestureCancelled(ref gestureData);
            ////						}
            ////						break;
            //				}
            //				break;

            // check for SwipeLeft
            case Gestures.SwipeLeft:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] &&
                           jointsPos[rightHandIndex].y >= gestureBottom && jointsPos[rightHandIndex].y <= gestureTop &&
                               jointsPos[rightHandIndex].x >= gestureRight /**&& jointsPos[rightHandIndex].x > gestureLeft*/)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.1f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) <= 1.0f)
                        {
                            bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] &&
                                    jointsPos[rightHandIndex].y >= gestureBottom && jointsPos[rightHandIndex].y <= gestureTop &&
                                    jointsPos[rightHandIndex].x <= gestureLeft;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                            else if (jointsPos[rightHandIndex].x <= gestureRight)
                            {
                                float gestureSize = gestureRight - gestureLeft;
                                gestureData.progress = gestureSize > 0.01f ? (gestureRight - jointsPos[rightHandIndex].x) / gestureSize : 0f;
                            }

                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for SwipeRight
            case Gestures.SwipeRight:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[leftHandIndex] && jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] &&
                           jointsPos[leftHandIndex].y >= gestureBottom && jointsPos[leftHandIndex].y <= gestureTop &&
                               jointsPos[leftHandIndex].x <= gestureLeft /**&& jointsPos[leftHandIndex].x < gestureRight*/)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.1f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) <= 1.0f)
                        {
                            bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] &&
                                    jointsPos[leftHandIndex].y >= gestureBottom && jointsPos[leftHandIndex].y <= gestureTop &&
                                    jointsPos[leftHandIndex].x >= gestureRight;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                            else if (jointsPos[leftHandIndex].x >= gestureLeft)
                            {
                                float gestureSize = gestureRight - gestureLeft;
                                gestureData.progress = gestureSize > 0.01f ? (jointsPos[leftHandIndex].x - gestureLeft) / gestureSize : 0f;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for SwipeUp
            case Gestures.SwipeUp:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[leftElbowIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[leftElbowIndex].y) < -0.0f &&
                           (jointsPos[rightHandIndex].y - jointsPos[leftElbowIndex].y) > -0.15f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[rightElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[rightElbowIndex].y) < -0.0f &&
                                (jointsPos[leftHandIndex].y - jointsPos[rightElbowIndex].y) > -0.15f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[leftShoulderIndex] &&
                                (jointsPos[rightHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.05f &&
                                Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) <= 0.15f :
                                jointsTracked[leftHandIndex] && jointsTracked[rightShoulderIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.05f &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) <= 0.15f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for SwipeDown
            case Gestures.SwipeDown:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[leftShoulderIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[leftShoulderIndex].y) >= 0.05f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[rightShoulderIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y) >= 0.05f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[leftElbowIndex] &&
                                (jointsPos[rightHandIndex].y - jointsPos[leftElbowIndex].y) < -0.15f &&
                                Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) <= 0.15f :
                                jointsTracked[leftHandIndex] && jointsTracked[rightElbowIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[rightElbowIndex].y) < -0.15f &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) <= 0.15f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            //			// check for RightHandCursor
            //			case Gestures.RightHandCursor:
            //				switch(gestureData.state)
            //				{
            //					case 0:  // gesture detection - phase 1 (perpetual)
            //						if(jointsTracked[rightHandIndex] && jointsTracked[rightHipIndex] &&
            //							//(jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y) > -0.1f)
            //				   			(jointsPos[rightHandIndex].y - jointsPos[hipCenterIndex].y) >= 0f)
            //						{
            //							gestureData.joint = rightHandIndex;
            //							gestureData.timestamp = timestamp;
            //							gestureData.jointPos = jointsPos[rightHandIndex];
            //
            //							SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
            //							gestureData.progress = 0.7f;
            //						}
            //						else
            //						{
            //							// cancel the gesture
            //							//SetGestureCancelled(ref gestureData);
            //							gestureData.progress = 0f;
            //						}
            //						break;
            //				
            //				}
            //				break;
            //
            //			// check for LeftHandCursor
            //			case Gestures.LeftHandCursor:
            //				switch(gestureData.state)
            //				{
            //					case 0:  // gesture detection - phase 1 (perpetual)
            //						if(jointsTracked[leftHandIndex] && jointsTracked[leftHipIndex] &&
            //							//(jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y) > -0.1f)
            //							(jointsPos[leftHandIndex].y - jointsPos[hipCenterIndex].y) >= 0f)
            //						{
            //							gestureData.joint = leftHandIndex;
            //							gestureData.timestamp = timestamp;
            //							gestureData.jointPos = jointsPos[leftHandIndex];
            //
            //							SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
            //							gestureData.progress = 0.7f;
            //						}
            //						else
            //						{
            //							// cancel the gesture
            //							//SetGestureCancelled(ref gestureData);
            //							gestureData.progress = 0f;
            //						}
            //						break;
            //				
            //				}
            //				break;

            // check for ZoomIn
            case Gestures.ZoomIn:
                Vector3 vectorZoomOut = (Vector3)jointsPos[rightHandIndex] - jointsPos[leftHandIndex];
                float distZoomOut = vectorZoomOut.magnitude;

                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] &&
                               jointsPos[leftHandIndex].y >= gestureBottom && jointsPos[leftHandIndex].y <= gestureTop &&
                               jointsPos[rightHandIndex].y >= gestureBottom && jointsPos[rightHandIndex].y <= gestureTop &&
                           distZoomOut < 0.3f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.tagVector = Vector3.right;
                            gestureData.tagFloat = 0f;
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture phase 2 = zooming
                        if ((timestamp - gestureData.timestamp) < 1.0f)
                        {
                            float angleZoomOut = Vector3.Angle(gestureData.tagVector, vectorZoomOut) * Mathf.Sign(vectorZoomOut.y - gestureData.tagVector.y);
                            bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] &&
                                    jointsPos[leftHandIndex].y >= gestureBottom && jointsPos[leftHandIndex].y <= gestureTop &&
                                    jointsPos[rightHandIndex].y >= gestureBottom && jointsPos[rightHandIndex].y <= gestureTop &&
                                distZoomOut < 1.5f && Mathf.Abs(angleZoomOut) < 20f;

                            if (isInPose)
                            {
                                SetZoomFactor(userId, ref gestureData, 1.0f, ref jointsPos, ref jointsTracked);
                                gestureData.timestamp = timestamp;
                                gestureData.progress = 0.7f;
                            }
                            //							else
                            //							{
                            //								// cancel the gesture
                            //								SetGestureCancelled(ref gestureData);
                            //							}
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for ZoomOut
            case Gestures.ZoomOut:
                Vector3 vectorZoomIn = (Vector3)jointsPos[rightHandIndex] - jointsPos[leftHandIndex];
                float distZoomIn = vectorZoomIn.magnitude;

                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] &&
                           jointsPos[leftHandIndex].y >= gestureBottom && jointsPos[leftHandIndex].y <= gestureTop &&
                           jointsPos[rightHandIndex].y >= gestureBottom && jointsPos[rightHandIndex].y <= gestureTop &&
                           distZoomIn >= 0.7f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.tagVector = Vector3.right;
                            gestureData.tagFloat = distZoomIn;
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture phase 2 = zooming
                        if ((timestamp - gestureData.timestamp) < 1.0f)
                        {
                            float angleZoomIn = Vector3.Angle(gestureData.tagVector, vectorZoomIn) * Mathf.Sign(vectorZoomIn.y - gestureData.tagVector.y);
                            bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] &&
                                    jointsPos[leftHandIndex].y >= gestureBottom && jointsPos[leftHandIndex].y <= gestureTop &&
                                    jointsPos[rightHandIndex].y >= gestureBottom && jointsPos[rightHandIndex].y <= gestureTop &&
                                distZoomIn >= 0.2f && Mathf.Abs(angleZoomIn) < 20f;

                            if (isInPose)
                            {
                                SetZoomFactor(userId, ref gestureData, 0.0f, ref jointsPos, ref jointsTracked);
                                gestureData.timestamp = timestamp;
                                gestureData.progress = 0.7f;
                            }
                            //							else
                            //							{
                            //								// cancel the gesture
                            //								SetGestureCancelled(ref gestureData);
                            //							}
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for Wheel
            case Gestures.Wheel:
                Vector3 vectorWheel = (Vector3)jointsPos[rightHandIndex] - jointsPos[leftHandIndex];
                float distWheel = vectorWheel.magnitude;

                //				Debug.Log(string.Format("{0}. Dist: {1:F1}, Tag: {2:F1}, Diff: {3:F1}", gestureData.state,
                //				                        distWheel, gestureData.tagFloat, Mathf.Abs(distWheel - gestureData.tagFloat)));

                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] &&
                           jointsPos[leftHandIndex].y >= gestureBottom && jointsPos[leftHandIndex].y <= gestureTop &&
                           jointsPos[rightHandIndex].y >= gestureBottom && jointsPos[rightHandIndex].y <= gestureTop &&
                           distWheel >= 0.3f && distWheel < 0.7f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.tagVector = Vector3.right;
                            gestureData.tagFloat = distWheel;
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture phase 2 = zooming
                        if ((timestamp - gestureData.timestamp) < 0.5f)
                        {
                            float angle = Vector3.Angle(gestureData.tagVector, vectorWheel) * Mathf.Sign(vectorWheel.y - gestureData.tagVector.y);
                            bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] &&
                                jointsPos[leftHandIndex].y >= gestureBottom && jointsPos[leftHandIndex].y <= gestureTop &&
                                jointsPos[rightHandIndex].y >= gestureBottom && jointsPos[rightHandIndex].y <= gestureTop &&
                                distWheel >= 0.3f && distWheel < 0.7f &&
                                Mathf.Abs(distWheel - gestureData.tagFloat) < 0.1f;

                            if (isInPose)
                            {
                                //SetWheelRotation(userId, ref gestureData, gestureData.tagVector, vectorWheel);
                                gestureData.screenPos.z = angle;  // wheel angle
                                gestureData.timestamp = timestamp;
                                gestureData.tagFloat = distWheel;
                                gestureData.progress = 0.7f;
                            }
                            //							else
                            //							{
                            //								// cancel the gesture
                            //								SetGestureCancelled(ref gestureData);
                            //							}
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for Jump
            case Gestures.Jump:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[hipCenterIndex] &&
                            (jointsPos[hipCenterIndex].y > 0.6f) && (jointsPos[hipCenterIndex].y < 1.2f))
                        {
                            SetGestureJoint(ref gestureData, timestamp, hipCenterIndex, jointsPos[hipCenterIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[hipCenterIndex] &&
                                (jointsPos[hipCenterIndex].y - gestureData.jointPos.y) > 0.15f &&
                                Mathf.Abs(jointsPos[hipCenterIndex].x - gestureData.jointPos.x) < 0.2f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for Squat
            case Gestures.Squat:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[hipCenterIndex] &&
                            (jointsPos[hipCenterIndex].y <= 0.7f))
                        {
                            SetGestureJoint(ref gestureData, timestamp, hipCenterIndex, jointsPos[hipCenterIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[hipCenterIndex] &&
                                (jointsPos[hipCenterIndex].y - gestureData.jointPos.y) < -0.15f &&
                                Mathf.Abs(jointsPos[hipCenterIndex].x - gestureData.jointPos.x) < 0.2f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for Push
            case Gestures.Push:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[rightShoulderIndex] &&
                               (jointsPos[rightHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
                               Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightShoulderIndex].x) < 0.2f &&
                               (jointsPos[rightHandIndex].z - jointsPos[leftElbowIndex].z) < -0.2f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[leftShoulderIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[leftShoulderIndex].x) < 0.2f &&
                                (jointsPos[leftHandIndex].z - jointsPos[rightElbowIndex].z) < -0.2f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[rightShoulderIndex] &&
                                (jointsPos[rightHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
                                Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.2f &&
                                (jointsPos[rightHandIndex].z - gestureData.jointPos.z) < -0.2f :
                                jointsTracked[leftHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[leftShoulderIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.2f &&
                                (jointsPos[leftHandIndex].z - gestureData.jointPos.z) < -0.2f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for Pull
            case Gestures.Pull:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[rightShoulderIndex] &&
                           (jointsPos[rightHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
                           Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightShoulderIndex].x) < 0.2f &&
                           (jointsPos[rightHandIndex].z - jointsPos[leftElbowIndex].z) < -0.3f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else if (jointsTracked[leftHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[leftShoulderIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[leftShoulderIndex].x) < 0.2f &&
                                (jointsPos[leftHandIndex].z - jointsPos[rightElbowIndex].z) < -0.3f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = gestureData.joint == rightHandIndex ?
                                jointsTracked[rightHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[rightShoulderIndex] &&
                                (jointsPos[rightHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
                                Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.2f &&
                                (jointsPos[rightHandIndex].z - gestureData.jointPos.z) > 0.25f :
                                jointsTracked[leftHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[leftShoulderIndex] &&
                                (jointsPos[leftHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f &&
                                Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.2f &&
                                (jointsPos[leftHandIndex].z - gestureData.jointPos.z) > 0.25f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for ShoulderLeftFron
            case Gestures.ShoulderLeftFront:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[leftShoulderIndex] && jointsTracked[rightShoulderIndex] && jointsTracked[leftHipIndex] &&
                              (jointsPos[rightShoulderIndex].z - jointsPos[leftHipIndex].z) < 0f &&
                           (jointsPos[rightShoulderIndex].z - jointsPos[leftShoulderIndex].z) > -0.15f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightShoulderIndex, jointsPos[rightShoulderIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[leftShoulderIndex] && jointsTracked[rightShoulderIndex] && jointsTracked[leftHipIndex] &&
                                    (jointsPos[rightShoulderIndex].z - jointsPos[leftShoulderIndex].z) < -0.2f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for ShoulderRightFront
            case Gestures.ShoulderRightFront:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[leftShoulderIndex] && jointsTracked[rightShoulderIndex] && jointsTracked[rightHipIndex] &&
                           (jointsPos[leftShoulderIndex].z - jointsPos[rightHipIndex].z) < 0f &&
                           (jointsPos[leftShoulderIndex].z - jointsPos[rightShoulderIndex].z) > -0.15f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftShoulderIndex, jointsPos[leftShoulderIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[leftShoulderIndex] && jointsTracked[rightShoulderIndex] && jointsTracked[rightHipIndex] &&
                                    (jointsPos[leftShoulderIndex].z - jointsPos[rightShoulderIndex].z) < -0.2f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for LeanLeft
            case Gestures.LeanLeft:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1  (right shoulder is left of the right hip, means leaning left)
                        if (jointsTracked[rightShoulderIndex] && jointsTracked[rightHipIndex] &&
                           (jointsPos[rightShoulderIndex].x - jointsPos[rightHipIndex].x) < 0f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightShoulderIndex, jointsPos[rightShoulderIndex]);
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 0.5f)
                        {
                            // check if right shoulder is still left of the right hip (leaning left)
                            bool isInPose = jointsTracked[rightShoulderIndex] && jointsTracked[rightHipIndex] &&
                                (jointsPos[rightShoulderIndex].x - jointsPos[rightHipIndex].x) < 0f;

                            if (isInPose)
                            {
                                // calculate lean angle
                                Vector3 vSpineLL = jointsPos[shoulderCenterIndex] - jointsPos[hipCenterIndex];
                                gestureData.screenPos.z = Vector3.Angle(Vector3.up, vSpineLL);

                                gestureData.timestamp = timestamp;
                                gestureData.progress = 0.7f;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for LeanRight
            case Gestures.LeanRight:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1 (left shoulder is right of the left hip, means leaning right)
                        if (jointsTracked[leftShoulderIndex] && jointsTracked[leftHipIndex] &&
                           (jointsPos[leftShoulderIndex].x - jointsPos[leftHipIndex].x) > 0f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftShoulderIndex, jointsPos[leftShoulderIndex]);
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 0.5f)
                        {
                            // check if left shoulder is still right of the left hip (leaning right)
                            bool isInPose = jointsTracked[leftShoulderIndex] && jointsTracked[leftHipIndex] &&
                                (jointsPos[leftShoulderIndex].x - jointsPos[leftHipIndex].x) > 0f;

                            if (isInPose)
                            {
                                // calculate lean angle
                                Vector3 vSpineLR = jointsPos[shoulderCenterIndex] - jointsPos[hipCenterIndex];
                                gestureData.screenPos.z = Vector3.Angle(Vector3.up, vSpineLR);

                                gestureData.timestamp = timestamp;
                                gestureData.progress = 0.7f;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for LeanForward
            case Gestures.LeanForward:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1 (shoulder center in front of hip center, means leaning forward)
                        if (jointsTracked[shoulderCenterIndex] && jointsTracked[hipCenterIndex] &&
                            (jointsPos[shoulderCenterIndex].z - jointsPos[hipCenterIndex].z) < -0.1f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, shoulderCenterIndex, jointsPos[shoulderCenterIndex]);
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 0.5f)
                        {
                            // check if shoulder center is still in front of the hip center (leaning forward)
                            bool isInPose = jointsTracked[shoulderCenterIndex] && jointsTracked[hipCenterIndex] &&
                                (jointsPos[shoulderCenterIndex].z - jointsPos[leftHipIndex].z) < -0.1f;

                            if (isInPose)
                            {
                                // calculate lean angle
                                Vector3 vSpineLL = jointsPos[shoulderCenterIndex] - jointsPos[hipCenterIndex];
                                gestureData.screenPos.z = Vector3.Angle(Vector3.up, vSpineLL);

                                gestureData.timestamp = timestamp;
                                gestureData.progress = 0.7f;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for LeanBack
            case Gestures.LeanBack:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1 (shoulder center behind hip center, means leaning back)
                        if (jointsTracked[shoulderCenterIndex] && jointsTracked[hipCenterIndex] &&
                            (jointsPos[shoulderCenterIndex].z - jointsPos[hipCenterIndex].z) > 0.1f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, shoulderCenterIndex, jointsPos[shoulderCenterIndex]);
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 0.5f)
                        {
                            // check if shoulder center is still behind of the hip center (leaning back)
                            bool isInPose = jointsTracked[shoulderCenterIndex] && jointsTracked[hipCenterIndex] &&
                                (jointsPos[shoulderCenterIndex].z - jointsPos[leftHipIndex].z) > 0.1f;

                            if (isInPose)
                            {
                                // calculate lean angle
                                Vector3 vSpineLR = jointsPos[shoulderCenterIndex] - jointsPos[hipCenterIndex];
                                gestureData.screenPos.z = Vector3.Angle(Vector3.up, vSpineLR);

                                gestureData.timestamp = timestamp;
                                gestureData.progress = 0.7f;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for KickLeft
            case Gestures.KickLeft:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[leftAnkleIndex] && jointsTracked[rightAnkleIndex] && jointsTracked[leftHipIndex] &&
                           (jointsPos[leftAnkleIndex].z - jointsPos[leftHipIndex].z) < 0f &&
                           (jointsPos[leftAnkleIndex].z - jointsPos[rightAnkleIndex].z) > -0.2f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftAnkleIndex, jointsPos[leftAnkleIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[leftAnkleIndex] && jointsTracked[rightAnkleIndex] && jointsTracked[leftHipIndex] &&
                                (jointsPos[leftAnkleIndex].z - jointsPos[rightAnkleIndex].z) < -0.4f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for KickRight
            case Gestures.KickRight:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[leftAnkleIndex] && jointsTracked[rightAnkleIndex] && jointsTracked[rightHipIndex] &&
                           (jointsPos[rightAnkleIndex].z - jointsPos[rightHipIndex].z) < 0f &&
                           (jointsPos[rightAnkleIndex].z - jointsPos[leftAnkleIndex].z) > -0.2f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightAnkleIndex, jointsPos[rightAnkleIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[leftAnkleIndex] && jointsTracked[rightAnkleIndex] && jointsTracked[rightHipIndex] &&
                                (jointsPos[rightAnkleIndex].z - jointsPos[leftAnkleIndex].z) < -0.4f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            case Gestures.Run:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                             // check if the left knee is up
                        if (jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] &&
                           (jointsPos[leftKneeIndex].y - jointsPos[rightKneeIndex].y) > 0.1f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftKneeIndex, jointsPos[leftKneeIndex]);
                            gestureData.progress = 0.3f;
                        }
                        break;

                    case 1:  // gesture complete
                        if ((timestamp - gestureData.timestamp) < 1.0f)
                        {
                            // check if the right knee is up
                            bool isInPose = jointsTracked[rightKneeIndex] && jointsTracked[leftKneeIndex] &&
                                (jointsPos[rightKneeIndex].y - jointsPos[leftKneeIndex].y) > 0.1f;

                            if (isInPose)
                            {
                                // go to state 2
                                gestureData.timestamp = timestamp;
                                gestureData.progress = 0.7f;
                                gestureData.state = 2;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;

                    case 2:  // gesture complete
                        if ((timestamp - gestureData.timestamp) < 1.0f)
                        {
                            // check if the left knee is up again
                            bool isInPose = jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] &&
                                (jointsPos[leftKneeIndex].y - jointsPos[rightKneeIndex].y) > 0.1f;

                            if (isInPose)
                            {
                                // go back to state 1
                                gestureData.timestamp = timestamp;
                                gestureData.progress = 0.8f;
                                gestureData.state = 1;
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for MoveLeft
            case Gestures.MoveLeft:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[hipCenterIndex])
                        {
                            SetGestureJoint(ref gestureData, timestamp, hipCenterIndex, jointsPos[hipCenterIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[hipCenterIndex] &&
                                (jointsPos[hipCenterIndex].x - gestureData.jointPos.x) < -0.25f &&
                                Mathf.Abs(jointsPos[hipCenterIndex].z - gestureData.jointPos.z) < 0.1f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            // check for MoveRight
            case Gestures.MoveRight:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[hipCenterIndex])
                        {
                            SetGestureJoint(ref gestureData, timestamp, hipCenterIndex, jointsPos[hipCenterIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[hipCenterIndex] &&
                                (jointsPos[hipCenterIndex].x - gestureData.jointPos.x) > 0.25f &&
                                Mathf.Abs(jointsPos[hipCenterIndex].z - gestureData.jointPos.z) < 0.1f;

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;

            case (Gestures.DefaultMotion):
                switch (gestureData.state)
                {
                    case 0:
                        if (jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex]//判断两个用来识别的左右手骨骼点是否识别到
                            && jointsPos[leftHandIndex].x - jointsPos[leftHipIndex].x < 0.2f
                            && jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y < 0.2f
                            && jointsPos[leftHandIndex].z - jointsPos[leftHipIndex].z < 0.2f
                            && jointsPos[rightHandIndex].x - jointsPos[rightHipIndex].x < 0.2f
                            && jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y < 0.2f
                            && jointsPos[rightHandIndex].z - jointsPos[rightHipIndex].z < 0.2f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftWristIndex, jointsPos[leftWristIndex]);
                            gestureData.progress = 0.5f;

                        }
                        break;
                    case 1:
                        bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex]//判断两个用来识别的左右手骨骼点是否识别到
                            && jointsPos[leftHandIndex].x - jointsPos[leftHipIndex].x < 0.2f
                            && jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y < 0.2f
                            && jointsPos[leftHandIndex].z - jointsPos[leftHipIndex].z < 0.2f
                            && jointsPos[rightHandIndex].x - jointsPos[rightHipIndex].x < 0.2f
                            && jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y < 0.2f
                            && jointsPos[rightHandIndex].z - jointsPos[rightHipIndex].z < 0.2f;

                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        break;
                }
                break;
            case (Gestures.VerticalForearm):
                switch (gestureData.state)
                {
                    case 0:
                        if (jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[neckIndex]
                            && jointsTracked[leftElbowIndex] && jointsTracked[rightElbowIndex]             // 判断五个骨骼点是否识别到
                            && jointsPos[leftHandIndex].y > jointsPos[neckIndex].y
                            && jointsPos[rightHandIndex].y > jointsPos[neckIndex].y
                            && jointsPos[leftElbowIndex].x - gestureLeft < 0.2f
                            && jointsPos[rightElbowIndex].x - gestureRight < 0.2f
                            )
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;

                        }
                        break;
                    case 1:
                        bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[neckIndex]
                            && jointsTracked[leftElbowIndex] && jointsTracked[rightElbowIndex]             // 判断五个骨骼点是否识别到
                            && jointsPos[leftHandIndex].y > jointsPos[neckIndex].y
                            && jointsPos[rightHandIndex].y > jointsPos[neckIndex].y
                            && jointsPos[leftElbowIndex].x - gestureLeft < 0.2f
                            && jointsPos[rightElbowIndex].x - gestureRight < 0.2f;

                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        break;
                }
                break;
            case Gestures.RaiseLeftHandParallelChest:
                switch (gestureData.state)
                {
                    case 0:
                        if (jointsTracked[leftShoulderIndex] && jointsTracked[leftElbowIndex] && jointsTracked[leftHandIndex] && jointsTracked[spineShoulderIndex] && jointsTracked[leftHipIndex]
                            && jointsPos[leftHandIndex].z - jointsPos[spineShoulderIndex].z < 0.2f
                            && jointsPos[leftHandIndex].y > jointsPos[leftHipIndex].y && jointsPos[leftHandIndex].y < jointsPos[spineShoulderIndex].y
                            && Vector3.Angle(jointsPos[leftShoulderIndex], jointsPos[leftElbowIndex]) > 5f && Vector3.Angle(jointsPos[leftShoulderIndex], jointsPos[leftElbowIndex]) < 60f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftWristIndex, jointsPos[hipCenterIndex]);
                            gestureData.progress = 0.5f;

                        }
                        break;
                    case 1:

                        bool isInPose = jointsTracked[leftShoulderIndex] && jointsTracked[leftElbowIndex] && jointsTracked[leftHandIndex] && jointsTracked[spineShoulderIndex] && jointsTracked[leftHipIndex]
                            && jointsPos[leftHandIndex].z - jointsPos[spineShoulderIndex].z < 0.2f
                            && jointsPos[leftHandIndex].y > jointsPos[leftHipIndex].y && jointsPos[leftHandIndex].y < jointsPos[spineShoulderIndex].y
                            && Vector3.Angle(jointsPos[leftShoulderIndex], jointsPos[leftElbowIndex]) > 5f && Vector3.Angle(jointsPos[leftShoulderIndex], jointsPos[leftElbowIndex]) < 60f;

                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        break;
                }
                break;
            case (Gestures.RaiseLeftHandLift45):
                switch (gestureData.state)
                {
                    case 0:
                        if (
                            //jointsTracked[leftWristIndex] && jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] && jointsTracked[leftShoulderIndex] &&
                        Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f //1-》左手高于右肩膀
                        && Mathf.Abs( jointsPos[leftHandIndex].x - gestureLeft) > 0.1f // 手臂张开不能超过一定距离
                        && jointsPos[leftHandIndex].y > gestureTop // 手部一定要穿过
                        //&& (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0f
                        && Vector3.Angle(jointsPos[leftHandIndex], jointsPos[leftShoulderIndex]) > 15f
                        && Vector3.Angle(jointsPos[leftHandIndex], jointsPos[leftShoulderIndex]) < 60f)// 角度无法准确锁定范围，特加上1-》判断
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftWristIndex, jointsPos[leftWristIndex]);
                            gestureData.progress = 0.5f;

                        }
                        break;
                    case 1:
                        bool isInPose =                             //jointsTracked[leftWristIndex] && jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] && jointsTracked[leftShoulderIndex] &&
                        Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f //1-》左手高于右肩膀
                        && Mathf.Abs(jointsPos[leftHandIndex].x - gestureLeft) > 0.1f // 手臂张开不能超过一定距离
                        && jointsPos[leftHandIndex].y > gestureTop // 手部一定要穿过
                        //&& (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0f
                        && Vector3.Angle(jointsPos[leftHandIndex], jointsPos[leftShoulderIndex]) > 15f
                        && Vector3.Angle(jointsPos[leftHandIndex], jointsPos[leftShoulderIndex]) < 60f; ;// 角度无法准确锁定范围，特加上1-》判断;
                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        break;
                }
                break;
            case (Gestures.RaiseBothHand45):
                switch (gestureData.state)
                {
                    case 0:
                        if (jointsTracked[leftWristIndex] && jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] && jointsTracked[leftShoulderIndex] &&
                            jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y > 0.1f //1-》左手高于右肩膀
                            && jointsPos[leftHandIndex].x > -0.2f // 手臂张开不能超过一定距离
                            && jointsPos[leftHandIndex].y > gestureTop // 手部一定要高过骨骼的最高点
                            && Vector3.Angle(jointsPos[leftHandIndex], jointsPos[leftShoulderIndex]) > 15f
                            && Vector3.Angle(jointsPos[leftHandIndex], jointsPos[leftShoulderIndex]) < 60f
                            && jointsPos[rightHandIndex].y - jointsPos[leftShoulderIndex].y > 0.1f //1-》左手高于右肩膀
                            && jointsPos[rightHandIndex].x > -0.2f // 手臂张开不能超过一定距离
                            && jointsPos[rightHandIndex].y > gestureTop // 手部一定要高过骨骼的最高点
                            && Vector3.Angle(jointsPos[rightHandIndex], jointsPos[rightShoulderIndex]) > 15f
                            && Vector3.Angle(jointsPos[rightHandIndex], jointsPos[rightShoulderIndex]) < 60f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftWristIndex, jointsPos[leftWristIndex]);
                            gestureData.progress = 0.5f;

                        }
                        break;
                    case 1:
                        bool isInPose = jointsTracked[leftWristIndex] && jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] && jointsTracked[leftShoulderIndex] &&
                            jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y > 0.1f //1-》左手高于右肩膀
                            && jointsPos[leftHandIndex].x > -0.2f // 手臂张开不能超过一定距离
                            && jointsPos[leftHandIndex].y > gestureTop // 手部一定要高过骨骼的最高点
                            && Vector3.Angle(jointsPos[leftHandIndex], jointsPos[leftShoulderIndex]) > 15f
                            && Vector3.Angle(jointsPos[leftHandIndex], jointsPos[leftShoulderIndex]) < 60f
                            && jointsPos[rightHandIndex].y - jointsPos[leftShoulderIndex].y > 0.1f //1-》左手高于右肩膀
                            && jointsPos[rightHandIndex].x > -0.2f // 手臂张开不能超过一定距离
                            && jointsPos[rightHandIndex].y > gestureTop // 手部一定要高过骨骼的最高点
                            && Vector3.Angle(jointsPos[rightHandIndex], jointsPos[rightShoulderIndex]) > 15f
                            && Vector3.Angle(jointsPos[rightHandIndex], jointsPos[rightShoulderIndex]) < 60f;
                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }

                        break;
                }
                break;
            case (Gestures.DownBothHand45):
                switch (gestureData.state)
                {
                   
                    case 0:
                        //Debug.LogError(Vector3.Angle(jointsPos[leftWristIndex], jointsPos[leftShoulderIndex]));
                        //Debug.LogError(Vector3.Angle(jointsPos[RightWristIndex], jointsPos[rightShoulderIndex]));
                        if ( 
                           
                             jointsPos[leftWristIndex].x > -0.2f // 手臂张开不能超过一定距离
                            && jointsPos[leftWristIndex].y > jointsPos[spineMidIndex].y // 手部一定要高过腰眼
                            && jointsPos[leftWristIndex].y < jointsPos[leftShoulderIndex].y
                            && Vector3.Angle(jointsPos[leftWristIndex], jointsPos[leftShoulderIndex]) > 15f
                            && Vector3.Angle(jointsPos[leftWristIndex], jointsPos[leftShoulderIndex]) < 60f
                            && jointsPos[RightWristIndex].x > -0.2f // 手臂张开不能超过一定距离
                            && jointsPos[RightWristIndex].y > jointsPos[spineMidIndex].y // 手部一定要高过脊椎的中心点
                            && jointsPos[RightWristIndex].y < jointsPos[neckIndex].y // 右手手腕的高度小于脖子的高度（可以再严格一点）
                            && Vector3.Angle(jointsPos[RightWristIndex], jointsPos[rightShoulderIndex]) > 15f
                            && Vector3.Angle(jointsPos[RightWristIndex], jointsPos[rightShoulderIndex]) < 60f)  // 保证手臂倾斜的角度
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftWristIndex, jointsPos[leftWristIndex]);
                            gestureData.progress = 0.5f;

                        }
                        else
                        {
                            //Debug.LogError("cuole shaluan");
                        }
                        break;
                    case 1:
                        bool isInPose = jointsPos[leftWristIndex].x > -0.2f 
                            && jointsPos[leftWristIndex].y > jointsPos[spineMidIndex].y 
                            && jointsPos[leftWristIndex].y < jointsPos[leftShoulderIndex].y
                            && Vector3.Angle(jointsPos[leftWristIndex], jointsPos[leftShoulderIndex]) > 15f
                            && Vector3.Angle(jointsPos[leftWristIndex], jointsPos[leftShoulderIndex]) < 60f
                            && jointsPos[RightWristIndex].x > -0.2f 
                            && jointsPos[RightWristIndex].y > jointsPos[spineMidIndex].y
                            && jointsPos[RightWristIndex].y < jointsPos[leftShoulderIndex].y
                            && Vector3.Angle(jointsPos[RightWristIndex], jointsPos[rightShoulderIndex]) > 15f
                            && Vector3.Angle(jointsPos[RightWristIndex], jointsPos[rightShoulderIndex]) < 60f;

                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        //Debug.Log("Done");
                        break;
                }
                break;
            case (Gestures.Kneel):
                switch (gestureData.state)
                {
                    case 0:
                        if (jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex] // 保证这几个骨骼点可以识别得到
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f // 左右膝盖倾斜不超过一定角度
                            && bandCenter < 0.7f // 保证是在跪着的姿势，高度小于1
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f 
                        )
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftWristIndex, jointsPos[hipCenterIndex]);
                            gestureData.progress = 0.5f;

                        }
                        break;
                    case 1:
                        bool isInPose = jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex]
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f;
                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        break;
                }
                break;

            case Gestures.KneelAndCPR:
                switch (gestureData.state)
                {
                    case 0:
                        if (
                        //jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex]
                        //    && jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[rightElbowIndex] //保证这些骨骼点被识别到才进行下面的判断
                             jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f // 判断姿势为跪姿
                            && jointsPos[spineBaseIndex].y > jointsPos[leftHandIndex].y
                            && jointsPos[spineBaseIndex].y > jointsPos[rightHandIndex].y// 双手必须定在腰眼下方，（严格话可以用差值控制一下范围）
                            && jointsPos[leftHandIndex].z - jointsPos[leftElbowIndex].z < 0.1f
                            && jointsPos[rightHandIndex].z - jointsPos[rightElbowIndex].z < 0.1f//控制手和轴是一条直线
                            && jointsPos[rightHandIndex].x - jointsPos[leftHandIndex].x < 0.1f// 左手和右手是大致在一起
                            && jointsPos[rightHandIndex].x > jointsPos[leftKneeIndex].x
                            && jointsPos[rightHandIndex].x < jointsPos[rightKneeIndex].x // 双手在两膝之间
                            )
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else
                        {
                            Debug.LogError("识别失败");
                        }

                        break;

                    case 1:
                        bool isInPose =
                            //jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex]
                            //&& jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[rightElbowIndex] //保证这些骨骼点被识别到才进行下面的判断
                            jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f // 判断姿势为跪姿
                            && jointsPos[spineBaseIndex].y > jointsPos[leftHandIndex].y
                            && jointsPos[spineBaseIndex].y > jointsPos[rightHandIndex].y// 双手必须定在腰眼下方，（严格话可以用差值控制一下范围）
                            && jointsPos[leftHandIndex].z - jointsPos[leftElbowIndex].z < 0.1f
                            && jointsPos[rightHandIndex].z - jointsPos[rightElbowIndex].z < 0.1f//控制手和轴是一条直线
                            && jointsPos[rightHandIndex].x - jointsPos[leftHandIndex].x < 0.1f
                            && jointsPos[rightHandIndex].x > jointsPos[leftKneeIndex].x
                            && jointsPos[rightHandIndex].x < jointsPos[rightKneeIndex].x;
                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        break;

                }
                break;
            case Gestures.KneelDefault:

                switch (gestureData.state)
                {
                    case 0:
                        if (jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex]//判断两个用来识别的左右手骨骼点是否识别到
                            &&jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[spineBaseIndex]
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f // 判断是否为跪姿
                            && jointsPos[leftHandIndex].x - jointsPos[leftHipIndex].x < 0.2f
                            && jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y < 0.2f
                            && jointsPos[leftHandIndex].z - jointsPos[leftHipIndex].z < 0.2f
                            && jointsPos[rightHandIndex].x - jointsPos[rightHipIndex].x < 0.2f
                            && jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y < 0.2f
                            && jointsPos[rightHandIndex].z - jointsPos[rightHipIndex].z < 0.2f)// 判断两只手是否贴紧左右胯
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }

                        break;
                    case 1:
                        bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[rightHandIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex]//判断两个用来识别的左右手骨骼点是否识别到
                            && jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[spineBaseIndex]
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f
                            && jointsPos[leftHandIndex].x - jointsPos[leftHipIndex].x < 0.2f
                            && jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y < 0.2f
                            && jointsPos[leftHandIndex].z - jointsPos[leftHipIndex].z < 0.2f
                            && jointsPos[rightHandIndex].x - jointsPos[rightHipIndex].x < 0.2f
                            && jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y < 0.2f
                            && jointsPos[rightHandIndex].z - jointsPos[rightHipIndex].z < 0.2f;
                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        break;
                }
                break;
                ///和chekcthroat一样
            case Gestures.CheckCondition:

                switch (gestureData.state)
                {
                    case 0:
                        if (jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex]
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f
                            //&& distCheckThroat < 2.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightKneeIndex].x) < 0.3f
                            && Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[rightKneeIndex].x) < 0.3f
                            && Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].z - jointsPos[rightKneeIndex].z) > 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].z - jointsPos[rightKneeIndex].z) > 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[rightHandIndex].x) > 0.1f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                            gestureData.progress = 0.5f;
                        }

                        break;
                    case 1:
                        bool isInPose = jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex]
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f
                            //&& distCheckThroat < 2.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightKneeIndex].x) < 0.3f
                            && Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[rightKneeIndex].x) < 0.3f
                            && Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].z - jointsPos[rightKneeIndex].z) > 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].z - jointsPos[rightKneeIndex].z) > 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[rightHandIndex].x) > 0.1f;
                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        break;
                }
                break;
            case Gestures.CheckBreathe:
                Vector3 checkBreathe = jointsPos[headIndex] - jointsPos[hipCenterIndex];
                float distCheckBreathe = Vector3.Magnitude(checkBreathe);
                //text.text = distCheckBreathe.ToString();
                float firstNum = 0;
                switch (gestureData.state)
                {
                    case 0:
                        firstNum = distCheckBreathe;
                        if (jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex]
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f
                            && distCheckBreathe <2.2f
                            )
                        {
                            SetGestureJoint(ref gestureData, timestamp, leftKneeIndex, jointsPos[leftKneeIndex]);
                            //gestureData.tagVector = Vector3.down;
                            //gestureData.tagFloat = distCheckBreathe;
                            gestureData.progress = 0.3f;
                        }
                        break;
                        
                    case 1:
                        if (timestamp - gestureData.timestamp < 3f)
                        {
                            //Debug.LogError(bandCenter);
                            //jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex] &&
                            //Debug.LogError(jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f);
                            //Debug.LogError(jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f);
                            //Debug.LogError(jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f);
                            //Debug.LogError(jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f);
                            //Debug.LogError(bandCenter < 0.7f);
                            //Debug.LogError(distCheckBreathe < 2.4f);
                            bool isInPose =  jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f
                            && distCheckBreathe -  firstNum  > 0.2f;
                            //Debug.LogError(isInPose);
                            if (isInPose)
                            {
                                //Debug.LogError("done");
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                                //gestureData.timestamp = 1f;
                                //gestureData.tagFloat = distCheckBreathe;
                                //gestureData.progress = 0.7f;
                            }
                            else
                            {
                                SetGestureCancelled(ref gestureData);
                            }
                        }
                        else
                        {
                            SetGestureCancelled(ref gestureData);
                        }

                        break;
                }
                break;
            case Gestures.CheckIsRecovery:
                Vector3 ICheckIsRecovery = jointsPos[headIndex] - jointsPos[hipCenterIndex];
                float distCheckRecovery = Vector3.Magnitude(ICheckIsRecovery);
                switch (gestureData.state)
                {
                    case 0:
                        if (
                            jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex]
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f
                            //&& distCheckThroat < 2.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightKneeIndex].x) < 0.3f
                            && Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[rightKneeIndex].x) < 0.3f
                            && Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].z - jointsPos[rightKneeIndex].z) > 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].z - jointsPos[rightKneeIndex].z) > 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[rightHandIndex].x) > 0.1f

                            )
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else
                        {
                            Debug.LogError("sssssssssssssssssssssssssssssssssssssssssssssss");
                        }

                        break;
                    case 1:
                        bool isInPose = jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex]
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f
                            //&& distCheckThroat < 2.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightKneeIndex].x) < 0.3f
                            && Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[rightKneeIndex].x) < 0.3f
                            && Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].z - jointsPos[rightKneeIndex].z) > 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].z - jointsPos[rightKneeIndex].z) > 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[rightHandIndex].x) > 0.1f;

                        //Debug.LogError(isInPose);
                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        else
                        {
                            Debug.LogError("jud");
                        }
                        break;
                }
                break;
            case Gestures.KneelAndCheckThroat:
                Vector3 CheckThroat = jointsPos[headIndex] - jointsPos[hipCenterIndex];
                float distCheckThroat = Vector3.Magnitude(CheckThroat);
                switch (gestureData.state)
                {
                    case 0:
                        if (
                            jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex]
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f
                            //&& distCheckThroat < 2.2f
                            && jointsPos[rightHandIndex].x - gestureLeft > 0.2f
                            && jointsPos[leftHandIndex].x - gestureLeft > 0.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].z - jointsPos[rightKneeIndex].z) > 0.1f
                            && Mathf.Abs(jointsPos[leftHandIndex].z - jointsPos[rightKneeIndex].z) > 0.1f
                            && Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[rightHandIndex].x) > 0.1f
                            && Mathf.Abs(jointsPos[leftHandIndex].z - jointsPos[rightHandIndex].z) > 0.1f

                            )
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else
                        {
                            Debug.LogError("aaaa");
                        }

                        break;
                    case 1:
                        bool isInPose = jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex]
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f
                            //&& distCheckThroat < 2.2f
                            && jointsPos[rightHandIndex].x - gestureLeft > 0.2f
                            && jointsPos[leftHandIndex].x - gestureLeft > 0.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[rightKneeIndex].y) < 0.2f
                            && Mathf.Abs(jointsPos[rightHandIndex].z - jointsPos[rightKneeIndex].z) > 0.1f
                            && Mathf.Abs(jointsPos[leftHandIndex].z - jointsPos[rightKneeIndex].z) > 0.1f
                            && Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[rightHandIndex].x) > 0.1f
                            && Mathf.Abs(jointsPos[leftHandIndex].z - jointsPos[rightHandIndex].z) > 0.1f;

                        Debug.LogError(isInPose);
                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        break;
                }
                break;
                //还没测好，跪下拍肩
            case Gestures.TouchShoulder:

                switch (gestureData.state)
                {
                    case 0:
                        if (
                            jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f
                            && Mathf.Abs(jointsPos[headIndex].y - gestureTop) < 0.1f
                            && Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightKneeIndex].x) < 0.2f
                            && jointsPos[rightHandIndex].y - jointsPos[rightKneeIndex].x < -0.2f)
                        {
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                            gestureData.progress = 0.5f;
                        }
                        else
                        {
                            //Debug.LogError("sssssssssssssssssssssssssssssssssssssssssssssss");
                        }

                        break;
                    case 1:
                        bool isInPose = jointsTracked[leftKneeIndex] && jointsTracked[rightKneeIndex] && jointsTracked[leftHipIndex] && jointsTracked[rightHipIndex] && jointsTracked[spineBaseIndex] && jointsTracked[rightHandIndex]
                            && jointsPos[leftKneeIndex].x - jointsPos[leftHipIndex].x > -0.2f
                            && jointsPos[rightKneeIndex].x - jointsPos[rightHipIndex].x > -0.2f
                            && bandCenter < 0.7f
                            && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[leftKneeIndex].y < bandCenter / 2f
                            && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y > 0 && jointsPos[spineBaseIndex].y - jointsPos[rightKneeIndex].y < bandCenter / 2f
                            && Mathf.Abs(jointsPos[headIndex].y - gestureTop) < 0.1f
                            && Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightKneeIndex].x) < 0.2f
                            && jointsPos[rightHandIndex].y - jointsPos[rightKneeIndex].x < -0.2f;

                        Debug.LogError(isInPose);
                        if (isInPose)
                        {
                            Vector3 jointPos = jointsPos[gestureData.joint];
                            CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                        }
                        break;
                }
                break;
        }
    }

}
