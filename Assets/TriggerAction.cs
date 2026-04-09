using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Events;
using TMPro;

public class TriggerAction : MonoBehaviour
{
    public enum ResourceType { Crop, Electricity, None }

    [Header("Cost")]
    public ResourceType resource = ResourceType.None;
    public int amount = 0;

    [Header("Input & Feedback")]
    public InputActionReference action;
    public UnityEvent OnActionExecuted;

    [Header("Visual")]
    public GameObject blinkingText;
    public float blinkInterval = 0.5f;

    [Header("Cooldown")]
    public float cooldownDuration = 2f;
    private float cooldownRemaining = 0f;
    private bool isOnCooldown = false;

    [Header("Cooldown UI")]
    public TMP_Text cooldownText;

    [Header("Other")]
    public bool isOneTimeUse = true;

    private bool isPlayerInside = false;
    private Coroutine blinkingCoroutine;

    private void OnEnable() => action.action.performed += HandleInput;

    private void OnDisable() => action.action.performed -= HandleInput;

    private void Update()
    {
        if (isOnCooldown)
        {
            cooldownRemaining -= Time.deltaTime;

            if (cooldownRemaining < 0f)
                cooldownRemaining = 0f;

            if (cooldownText != null)
                cooldownText.text = "Cooldown: " + cooldownRemaining.ToString("0.0") + "s";

            if (cooldownRemaining <= 0f)
            {
                isOnCooldown = false;

                if (cooldownText != null)
                    cooldownText.text = "Ready";
            }
        }
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        if (isOnCooldown) return;

        if (isPlayerInside && SpendResource())
        {
            OnActionExecuted.Invoke();

            isOnCooldown = true;
            cooldownRemaining = cooldownDuration;

            if (cooldownText != null)
                cooldownText.text = "Cooldown: " + cooldownRemaining.ToString("0.0") + "s";

            if (isOneTimeUse) this.gameObject.SetActive(false);
        }
    }

    private bool SpendResource()
    {
        switch (resource)
        {
            case ResourceType.Crop:
                return ResourceManager.Instance.SpendCrops(amount);
            case ResourceType.Electricity:
                return ResourceManager.Instance.SpendElectricity(amount);
            case ResourceType.None:
                return true;
            default:
                return false;
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

            if (blinkingText != null)
                blinkingText.SetActive(false);
        }
    }

    private IEnumerator BlinkText()
    {
        while (isPlayerInside)
        {
            if (blinkingText != null)
                blinkingText.SetActive(!blinkingText.activeSelf);

            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
