using UnityEngine;
using UnityEngine.InputSystem;

public class CanvasFaceCamera : MonoBehaviour
{
    public InputActionReference toggleCanvasAction;
    private Transform mainCameraTransform;
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
        toggleCanvasAction.action.Enable();
        toggleCanvasAction.action.performed += ToggleCanvasVisibility;
    }

    void LateUpdate()
    {
        transform.LookAt(mainCameraTransform);
        transform.Rotate(0, 180, 0);
    }

    private void ToggleCanvasVisibility(InputAction.CallbackContext context)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnDestroy()
    {
        toggleCanvasAction.action.performed -= ToggleCanvasVisibility;
    }
}
