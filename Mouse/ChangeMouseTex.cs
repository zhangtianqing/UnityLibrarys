using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMouseTex : MonoBehaviour
{

    /// <summary>
    /// 鼠标材质
    /// </summary>
    public Texture CursorTexture;

    private void Awake()
    {
        Cursor.visible = false;
    }
    void OnGUI()
    {
        Vector3 mousePos = Input.mousePosition;
        //因为GUI坐标系原点是左上角，而屏幕坐标系原点是在左下角，所以要转换
        GUI.DrawTexture(new Rect(mousePos.x - CursorTexture.width / 2, Screen.height - mousePos.y - CursorTexture.height / 2, CursorTexture.width, CursorTexture.height), CursorTexture);
    }
}
