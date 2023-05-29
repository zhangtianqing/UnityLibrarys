
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TxtTools : MonoBehaviour
{

    [Header("打字间隔")] public float typeTimer = 0.2f;
    [Header("打字的内容")] public string words;
    private TextMeshProUGUI txtFile;
    [Header("是否开始打字")] public bool isStartTyping = false;//是否开始打字
    private float timer;
    private int currentIndex = 0;

    public void isStart(string text)
    {
        txtFile = GetComponent<TextMeshProUGUI>();
        //返回两个或多个值中最大的值
        typeTimer = Mathf.Max(0.02f, typeTimer);

        words = text;
        Invoke("Play", 0.8f);
    }

    void Play()
    {
        isStartTyping = true;
    }

    private void Update()
    {
        OnStartTyping();
    }

    private void OnStartTyping()
    {

        if (isStartTyping)
        {
            timer += Time.deltaTime;
            if (timer >= typeTimer)
            {
                timer = 0;
                currentIndex++;
                txtFile.text = words.Substring(0, currentIndex);
                if (currentIndex >= words.Length)
                {
                    OnFinshTyping();
                }
            }
        }
    }

    private void OnFinshTyping()
    {
        isStartTyping = false;
        timer = 0;
        currentIndex = 0;
        txtFile.text = words;
    }
}
