
using UnityEngine;

public class FollwMouseMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 8.0f;
    public float distanceFromCamera = 10f;

    public bool ignoreTimeScale;

    private void Start()
    {
        
    }

    // ...

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = distanceFromCamera;

        Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);

        float deltaTime = !ignoreTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
        Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * deltaTime));
        
        transform.position = position;
    }


}
