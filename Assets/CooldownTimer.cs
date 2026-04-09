using UnityEngine;
using TMPro;

public class CooldownTimer : MonoBehaviour
{
    public GameObject cropClicker;

    [Header("Cooldown")]
    public float cooldownDuration = 1f;
    private float cooldownRemaining = 0f;
    private bool isOnCooldown = false;

    [Header("Cooldown UI")]
    public TextMeshProUGUI cooldownText;

    [Header("Pop-In Animation")]
    private SpringPopInAnimator popInAnimator;
    private TriggerAction cropClickerAction;
    private AudioSource cooldownAudioSource;

    void Start()
    {
        popInAnimator = GetComponent<SpringPopInAnimator>();
        if (popInAnimator == null)
        {
            popInAnimator = gameObject.AddComponent<SpringPopInAnimator>();
        }
        popInAnimator.targetScale = cooldownText.transform.localScale;

        if (cooldownText != null)
        {
            cooldownAudioSource = cooldownText.GetComponent<AudioSource>();
        }

        if (cropClicker != null)
        {
            cropClickerAction = cropClicker.GetComponent<TriggerAction>();
        }

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownRemaining -= Time.deltaTime;

            if (cooldownRemaining < 0f)
                cooldownRemaining = 0f;

            if (cooldownText != null)
                cooldownText.text = "Cooldown: " + cooldownRemaining.ToString("0.00") + "s";

            if (cooldownRemaining <= 0f)
            {
                isOnCooldown = false;

                if (cooldownText != null)
                    cooldownText.text = "";

                SetCropClickerEnabled(true);

                if (cooldownAudioSource != null)
                {
                    cooldownAudioSource.Play();
                }

                if (popInAnimator != null)
                {
                    popInAnimator.TriggerPopIn();
                }
            }
        }
    }

    public void StartCooldown()
    {
        if (isOnCooldown) return;

        isOnCooldown = true;
        cooldownRemaining = cooldownDuration;

        SetCropClickerEnabled(false);

        if (popInAnimator != null)
        {
            popInAnimator.TriggerPopIn();
        }

        if (cooldownText != null)
        {
            cooldownText.text = "Cooldown: " + cooldownRemaining.ToString("0.00") + "s";
        }
    }

    private void SetCropClickerEnabled(bool isEnabled)
    {
        if (cropClickerAction != null)
        {
            cropClickerAction.enabled = isEnabled;
        }
    }
}
