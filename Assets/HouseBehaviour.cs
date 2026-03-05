using UnityEngine;
using TMPro;

public class HouseGenerator : MonoBehaviour
{
    [Header("Generation (what 1 house contributes)")]
    [SerializeField] private int baseCropRate = 1;
    [SerializeField] private int cropRatePerUpgrade = 1;

    [Header("Upgrade")]
    [SerializeField] private int level = 0;
    [SerializeField] private int maxLevel = 10;

    [Header("Optional Debug UI")]
    [SerializeField] private TextMeshPro levelText;

    private bool isPlaced = false;
    private int currentContribution = 0;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        RemoveContribution();
    }
    public void PlaceHouse()
    {
        if (isPlaced) return;
        isPlaced = true;

        ApplyContribution();
        UpdateUI();
    }

    public void UpgradeHouse()
    {
        if (!isPlaced)
        {
            Debug.LogWarning("Tried upgrading house before it was placed. Calling PlaceHouse() first.");
            PlaceHouse();
        }

        if (level >= maxLevel)
        {
            Debug.Log("House max level.");
            return;
        }

        RemoveContribution();

        level++;

        ApplyContribution();
        UpdateUI();

        Debug.Log($"House upgraded to level {level}. Contribution now {currentContribution} crop/sec");
    }

    private void ApplyContribution()
    {
        currentContribution = baseCropRate + level * cropRatePerUpgrade;
        ResourceManager.Instance.IncreaseCropGrowthRate(currentContribution);
    }

    private void RemoveContribution()
    {
        if (!isPlaced) return;
        if (currentContribution == 0) return;

        ResourceManager.Instance.IncreaseCropGrowthRate(-currentContribution);
        currentContribution = 0;
    }

    private void UpdateUI()
    {
        if (levelText != null)
            levelText.text = $"House Lv {level}\n+{currentContribution}/s";
    }

    public int GetLevel() => level;
}