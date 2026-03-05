using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActionAsset inputActions;

    private void Awake()
    {
        inputActions?.Enable();
    }

    private void OnDestroy()
    {
        inputActions?.Disable();
    }
}
