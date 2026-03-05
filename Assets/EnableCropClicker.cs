using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class EnableCropClicker : MonoBehaviour
{
    public InputActionReference clickAction;
    public GameObject blinkingText;
    public float blinkInterval = 0.5f;

    private bool isPlayerInside = false;
    private Coroutine blinkingCoroutine;

    void OnEnable()
    {
        clickAction.action.Enable();
        clickAction.action.performed += onClickCrop;
    }
    void OnDisable()
    {
        clickAction.action.performed -= onClickCrop;
        clickAction.action.Disable();
    }
    private void onClickCrop(InputAction.CallbackContext context)
    {
        if (isPlayerInside)
        {
            ResourceManager.Instance.IncreaseCropCount(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            blinkingCoroutine = StartCoroutine(BlinkText());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (blinkingCoroutine != null)
            {
                StopCoroutine(blinkingCoroutine);
                blinkingCoroutine = null;
            }
            blinkingText.SetActive(false);
        }
    }
    private IEnumerator BlinkText()
    {
        while (isPlayerInside)
        {
            blinkingText.SetActive(!blinkingText.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
