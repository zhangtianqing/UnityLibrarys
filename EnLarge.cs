using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 多点触控放大缩小
/// AssetStore可用插件: lean touch
/// </summary>
public class EnLarge : MonoBehaviour
{

    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2) 
    public ParticleSystem[] particle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount < 1)
        {
            for (int i = 0; i < 4; i++)
            {
                var main = particle[i].main;
                main.loop = false;
                particle[i].Stop();
            }
        }
        if (Input.GetMouseButton(0))
        {

            

            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var main = particle[i].main;
                        main.loop = true;
                        particle[i].Play();
                    }
                }
            }

            if (1 == Input.touchCount)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 deltaPos = touch.deltaPosition;
                transform.Rotate(Vector3.down * deltaPos.x * Time.deltaTime * 100, Space.World);
                transform.Rotate(Vector3.right * deltaPos.y * Time.deltaTime * 100, Space.World);
            }
            if (Input.touchCount == 2)
            {
                //多点触摸, 放大缩小  
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                //第2点刚开始接触屏幕, 只记录，不做处理  
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }

                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = newDistance - oldDistance;

                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset / 100f;
                Vector3 localScale = transform.localScale;
                Vector3 scale = new Vector3(localScale.x + scaleFactor,
                                            localScale.y + scaleFactor,
                                            localScale.z + scaleFactor);

                //最小缩放到 0.3 倍  
                if (scale.x > 5f && scale.y > 5f && scale.z > 5f)
                {
                    transform.localScale = scale;
                }

                //记住最新的触摸点，下次使用  
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
        }
    }


}

    

