using System.IO;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
public static class EditorScreenshotExtension
{
#if UNITY_EDITOR
    //ctrl + shift + y ��ͼ
    [MenuItem("Screenshot/Take Screenshot %#y")]
    private static async void Screenshot()
    {
        string folderPath = Directory.GetCurrentDirectory() + "\\Screenshots";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var timestamp = System.DateTime.Now;
        var stampString = string.Format("_{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}", timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, timestamp.Second);
        ScreenCapture.CaptureScreenshot(Path.Combine(folderPath, stampString + ".png"));
        
        Debug.Log("��ͼ��......");
        //�ȴ�5��
        await Task.Delay(5000);
        System.Diagnostics.Process.Start("explorer.exe", folderPath);
        Debug.Log("��ͼ" + stampString + ".png");
    }
#endif
}