using UnityEngine;
using UnityEngine.InputSystem;

public class CanvasFaceCamera : MonoBehaviour
{
    private Transform mainCameraTransform;
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(mainCameraTransform);
        transform.Rotate(0, 180, 0);
    }
}
