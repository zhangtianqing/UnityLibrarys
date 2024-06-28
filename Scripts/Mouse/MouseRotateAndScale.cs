using UnityEngine;
using System.Collections;

public class MouseRotateAndScale : MonoBehaviour
{
    GameObject car;
    //public GameObject light;
    Vector2 p1, p2;//������¼����λ�ã��Ա������ת����
                   // ����м��
    int MouseWheelSensitivity = 5;
    int MouseZoomMin = 18;
    int MouseZoomMax = 90;
    float normalDistance = 60;
    // Use this for initialization
    void Start()
    {
        car = GameObject.Find("benchi");

    }

    // Update is called once per frame
    void Update()
    {
        //��ת
        if (Input.GetMouseButtonDown(0))
        {
            p1 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);//����������ʱ��¼���λ��p1

        }
        if (Input.GetMouseButton(0))
        {
            p2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);//�������϶�ʱ��¼���λ��p2
            if (transform.position.y >= 20 && transform.position.y <= 130)  //������20-130֮��
            {
                float dx = p2.x - p1.x;

                float dy = (float)0.6 * (p2.y - p1.y);
                //��������ƶ�
                transform.Translate(-dy * Vector3.up * Time.deltaTime);
                transform.RotateAround(car.transform.position, Vector3.up, dx * Time.deltaTime);
            }
            else if (transform.position.y < 20 && p2.y < p1.y)
            {
                float dx = p2.x - p1.x;

                float dy = (float)0.6 * (p2.y - p1.y);
                //��������ƶ�
                transform.Translate(-dy * Vector3.up * Time.deltaTime);
                transform.RotateAround(car.transform.position, Vector3.up, dx * Time.deltaTime);
            }
            else if (transform.position.y > 130 && p2.y > p1.y)
            {
                float dx = p2.x - p1.x;

                float dy = (float)0.6 * (p2.y - p1.y);
                //��������ƶ�
                transform.Translate(-dy * Vector3.up * Time.deltaTime);
                transform.RotateAround(car.transform.position, Vector3.up, dx * Time.deltaTime);
            }
            //���濪ʼ��ת������ˮƽ�����Ͻ�����ת



            //else if (transform.position.y < 20)
            //{
            //    transform.position.y = 20.1f;
            //}
            //else if (transform.position.y > 130)
            //{
            //    transform.position.y = 129.9f;
            //}
            //light.transform.RotateAround(car.transform.position, Vector3.up, dx * Time.deltaTime);
        }


        //�����ֿ��Ƴ�����С
        // �����ס����
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Debug.Log(1);
            Debug.Log(Input.GetAxis("Mouse ScrollWheel"));

            if (normalDistance >= MouseZoomMin && normalDistance <= MouseZoomMax)
            {
                normalDistance -= Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity;
            }

            if (normalDistance < MouseZoomMin)
            {
                normalDistance = MouseZoomMin;
            }
            if (normalDistance > MouseZoomMax)
            {
                normalDistance = MouseZoomMax;
            }
            // transform.Translate(transform.forward * normalDistance);
            transform.GetComponent<Camera>().fieldOfView = normalDistance;


        }

        //���
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Debug.Log(-1);
            if (normalDistance >= MouseZoomMin && normalDistance <= MouseZoomMax)
            {
                normalDistance -= Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity;
            }

            if (normalDistance < MouseZoomMin)
            {
                normalDistance = MouseZoomMin;
            }
            if (normalDistance > MouseZoomMax)
            {
                normalDistance = MouseZoomMax;
            }
            // transform.Translate(-transform.forward * normalDistance);
            transform.GetComponent<Camera>().fieldOfView = normalDistance;
        }
    }
}