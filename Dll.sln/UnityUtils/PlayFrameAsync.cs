using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayFrame : MonoBehaviour
{
    public enum State
    {
        idle,
        playing,
        pause
    }

    public enum State1
    {
        once,
        loop
    }

    [Header("序列帧文件夹名称")]
    public string frame_Floder_Name;

    [Header("序列帧文件名（带_）")]
    public string frame_Name;

    [Header("序列帧开始帧值")]
    public int frame_Start_Index;

    [Header("序列帧结束帧值")]
    public int frame_End_Index;

    private int frame_Lenth;

    [Header("播放方式(循环、单次)")]//默认单次
    public State1 condition = State1.once;

    [Header("自动播放")]//默认不自动播放
    public bool PlayAwake = false;

    //播放状态(默认、播放中、暂停)
    private State playState;

    private RawImage manimg;

    [Header("每秒播放的帧数(整数)")]
    public float frame_number = 30;

    ////[Header("sprite数组路径")]
    //public string texturePath = "";

    //public Texture[] texture_arr;

    //回调事件
    public UnityEvent onCompleteEvent;

    private int index;
    private float tim;
    private float waittim;
    private bool isplay;

    private void OnEnable()
    {
        manimg = GetComponent<RawImage>();

        frame_Lenth = frame_End_Index - frame_Start_Index + 1;

        tim = 0;
        index = 0;
        waittim = 1 / frame_number;
        playState = State.idle;
        isplay = false;

        ToDisvisible();
        //texture_arr = Resources.LoadAll<Texture>(texturePath);//"Texture/1106景观位置导图cs-道"

        if (manimg == null)
        {
            Debug.LogWarning("Image为空，请添加Image组件！！！");
            return;
        }
        if (frame_Lenth < 1)
        {
            Debug.LogWarning("sprite数组为0，请给sprite数组添加元素！！！");
        }
        manimg.texture = ReadFrame(0);
        if (PlayAwake)
        {
            Play();
        }
    }

    private void Update()
    {
        //#region 测试

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Play();
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Replay();
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Stop();
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Pause();
        //}

        //#endregion 测试

        UpMove();
    }

    #region 改进-播放显示，不播放隐藏

    /// <summary>
    /// 改进-播放显示，不播放隐藏
    /// </summary>
    private void ToVisible()
    {
        manimg.color = new Color(255, 255, 255, 255);
    }

    private void ToDisvisible()
    {
        //manimg.color = new Color(255, 255, 255, 0);
    }

    #endregion 改进-播放显示，不播放隐藏

    private void UpMove()
    {
        //单播
        if (condition == State1.once)
        {
            if (playState == State.idle && isplay)
            {
                playState = State.playing;
                index = 0;
                tim = 0;
            }
            if (playState == State.pause && isplay)
            {
                playState = State.playing;
                tim = 0;
            }
            if (playState == State.playing && isplay)
            {
                tim += Time.deltaTime;
                if (tim >= waittim)
                {
                    tim = 0;
                    index++;
                    if (index >= frame_Lenth)
                    {
                        index = 0;
                        manimg.texture = ReadFrame(frame_Lenth - 1);
                        isplay = false;
                        playState = State.idle;
                        ToDisvisible();
                        //此处可添加结束回调函数
                        if (onCompleteEvent != null)
                        {
                            onCompleteEvent.Invoke();
                            return;
                        }
                    }
                    manimg.texture = ReadFrame(index);
                }
            }
        }
        //循环播放
        if (condition == State1.loop)
        {
            if (playState == State.idle && isplay)
            {
                playState = State.playing;
                index = 0;
                tim = 0;
            }
            if (playState == State.pause && isplay)
            {
                playState = State.playing;
                tim = 0;
            }
            if (playState == State.playing && isplay)
            {
                tim += Time.deltaTime;
                if (tim >= waittim)
                {
                    tim = 0;
                    index++;
                    if (index >= frame_Lenth)
                    {
                        index = 0;
                        //此处可添加结束回调函数
                    }
                    manimg.texture = ReadFrame(index);
                }
            }
        }
    }

    /// <summary>
    /// 播放
    /// </summary>
    public void Play()
    {
        ToVisible();
        isplay = true;
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        isplay = false;
        playState = State.pause;
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        isplay = false;
        playState = State.idle;
        index = 0;
        tim = 0;
        if (manimg == null)
        {
            Debug.LogWarning("Image为空，请赋值");
            return;
        }
        manimg.texture = ReadFrame(index);
    }

    /// <summary>
    /// 重播
    /// </summary>
    public void Replay()
    {
        isplay = true;
        playState = State.playing;
        index = 0;
        tim = 0;
    }

    private Texture2D ReadFrame(int _index)
    {
        return Resources.Load<Texture2D>("Frame/" + frame_Floder_Name + "/" + frame_Name + (frame_Start_Index + _index).ToString().PadLeft(5, '0'));
    }
}