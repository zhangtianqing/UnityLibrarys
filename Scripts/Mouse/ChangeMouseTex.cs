using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMouseTex : MonoBehaviour
{

    /// <summary>
    /// ������
    /// </summary>
    public Texture CursorTexture;

    private void Awake()
    {
        Cursor.visible = false;
    }
    void OnGUI()
    {
        Vector3 mousePos = Input.mousePosition;
        //��ΪGUI����ϵԭ�������Ͻǣ�����Ļ����ϵԭ���������½ǣ�����Ҫת��
        GUI.DrawTexture(new Rect(mousePos.x - CursorTexture.width / 2, Screen.height - mousePos.y - CursorTexture.height / 2, CursorTexture.width, CursorTexture.height), CursorTexture);
    }
}
