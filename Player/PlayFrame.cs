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

    [Header("播放方式(循环、单次)")]//默认单次
    public State1 condition = State1.once;

    [Header("自动播放")]//默认不自动播放
    public bool PlayAwake = false;

    [Header("未播放时是否隐藏")]//默认不隐藏
    public bool UnPlayDisplay = false;

    [Header("每秒播放的帧数(整数)")]
    public float frame_number = 30;


    //回调事件
    public UnityEvent onCompleteEvent;
    [Header("Texture资源序号-从0开始")]
    public int TexCurrenIndex=0;

    private Texture[] texture_arr;
    //播放状态(默认、播放中、暂停)
    private State playState;
    private RawImage manimg;


    private int index;
    private float tim;
    private float waittim;
    private bool isplay;

    private void Awake()
    {
        Application.targetFrameRate = 25;
    }

    private void Start()
    {
        manimg = GetComponent<RawImage>();
        tim = 0;
        index = 0;
        waittim = 1 / frame_number;
        playState = State.idle;
        isplay = false;

        //path:Resources/Texture/index 
        texture_arr = Resources.LoadAll<Texture>("Texture/" + TexCurrenIndex);

        if (manimg == null)
        {
            Debug.LogWarning("Image为空，请添加Image组件！！！");
            return;
        }
        if (texture_arr.Length < 1)
        {
            Debug.LogWarning("sprite数组为0，请给sprite数组添加元素！！！");
        }
        manimg.texture = texture_arr[0];
        if (PlayAwake)
        {
            Play();
        }
    }

    private void Update()
    {
        #region 编辑器测试
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            Play();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Replay();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Stop();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
#endif
        #endregion
        UpMove();
    }


    #region 改进-播放显示，不播放隐藏
    /// <summary>
    /// 改进-播放显示，不播放隐藏
    /// </summary>
    private void ToVisible() {
        if (isplay) {
            manimg.color = new Color(255, 255, 255,255);
        }
    }
    private void ToDisVisible() {
        if (UnPlayDisplay) {
            manimg.color = new Color(255, 255, 255, 0);
        }

    }
    #endregion

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
                    if (index >= texture_arr.Length)
                    {
                        index = 0;
                        manimg.texture = texture_arr[index];
                        isplay = false;
                        playState = State.idle;
                        ToDisVisible();
                        //此处可添加结束回调函数
                        if (onCompleteEvent != null)
                        {
                            onCompleteEvent.Invoke();
                            return;
                        }
                    }
                    manimg.texture = texture_arr[index];
                }
            }
        }
        #region 循环播放
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
                    if (index >= texture_arr.Length)
                    {
                        index = 0;
                        //此处可添加结束回调函数
                    }
                    manimg.texture = texture_arr[index];
                }
            }
        }
        #endregion
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
        ToDisVisible();
        manimg.texture = texture_arr[index];
    }

    /// <summary>
    /// 重播
    /// </summary>
    public void Replay()
    {
        ToVisible();
        isplay = true;
        playState = State.playing;
        index = 0;
        tim = 0;
    }
}