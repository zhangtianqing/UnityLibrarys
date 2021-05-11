
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 不规则2d点击
/// 1.在UI中编辑范围
/// 2.修改点击事件
/// 
/// TODO
/// </summary>
public class Irregular2DClick : MonoBehaviour
{
    private PolygonCollider2D polygonCollider2D;

    public GameObject mousSiuml;

    // Use this for initialization
    void Start()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }


    private void Update()
    {
        //鼠标点击响应
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Test");
            if (polygonCollider2D.OverlapPoint(Input.mousePosition))
            {
                Debug.Log(gameObject.name);
                //todo你的点击事件逻辑
                SendClick();
            }
        }
        //触屏点击响应
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                if (polygonCollider2D.OverlapPoint(Input.touches[0].position))
                {
                    Debug.Log(gameObject.name);
                    //todo你的点击事件逻辑
                    SendClick();
                }
            }
        }
    }
    /// <summary>
    /// TODO
    /// </summary>
    private void SendClick()
    {
        Debug.Log("SendClick");

    }

}