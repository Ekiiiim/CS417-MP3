using UnityEngine;
using TMPro;

public class GeneratorDeployer : MonoBehaviour
{
    [Header("What to deploy")]
    public GameObject generatorPrefab;

    [Header("Where to deploy")]
    public Transform spawnPoint;

    [Header("Limit (optional)")]
    public bool oneTime = false;

    [Header("Cooldown")]
    public float cooldownDuration = 2f;
    private float cooldownRemaining = 0f;
    private bool isOnCooldown = false;

    [Header("Cooldown UI")]
    public TMP_Text cooldownText;

    private bool deployed = false;

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

    public void Deploy()
    {
        if (oneTime && deployed) return;
        if (isOnCooldown) return;

        if (generatorPrefab == null)
        {
            Debug.LogError("GeneratorDeployer: generatorPrefab is not assigned.");
            return;
        }

        deployed = true;

        Transform t = (spawnPoint != null) ? spawnPoint : transform;
        Instantiate(generatorPrefab, t.position, t.rotation);

        isOnCooldown = true;
        cooldownRemaining = cooldownDuration;

        if (cooldownText != null)
            cooldownText.text = "Cooldown: " + cooldownRemaining.ToString("0.0") + "s";
    }
}
