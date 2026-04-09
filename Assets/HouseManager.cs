using UnityEngine;
using TMPro;
using System;

public class HouseManager : MonoBehaviour
{
    [Header("Generation (what 1 house contributes)")]
    [SerializeField] private float baseCropRate = 2f;
    [SerializeField] private float multiplierPerUpgrade = 2f;

    [Header("Upgrade")]
    [SerializeField] private int level = 1;
    [SerializeField] private int maxLevel = 3;
    [SerializeField] private int baseCost = 20;
    [SerializeField] private int costMultiplierPerUpgrade = 2;
    [SerializeField] private GameObject[] houseVisuals;
    [SerializeField] private GameObject upgradeTrigger;

    [Header("Optional Debug UI")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI upgradeText;

    private float currentContribution = 0;
    private float currentUpkeep = 0;
    private int currentCost;

    public bool isUnlocked;

    private void OnEnable()
    {
        currentCost = baseCost * costMultiplierPerUpgrade;
        UpdateUI();
    }

    private void OnDisable()
    {
        if (currentContribution != 0)
        {
            isUnlocked = true;
        }
        //     RemoveContribution();
    }

    public void ActivateHouse()
    {
        ApplyContribution();
        ApplyUpkeep();
        UpdateUI();
        isUnlocked = true;
        houseVisuals[level - 1].SetActive(true);
    }

    public void UpgradeHouse()
    {
        if (IsMaxLevel())
        {
            Debug.Log("House max level.");
            return;
        }

        if (!ResourceManager.Instance.SpendCrops(currentCost)) return;

        RemoveContribution();
        RemoveUpkeep();

        if (houseVisuals != null && houseVisuals.Length > 0)
        {
            houseVisuals[level - 1].SetActive(false);
            houseVisuals[level].SetActive(true);
        }

        level++;

        ApplyContribution();
        ApplyUpkeep();

        currentCost *= costMultiplierPerUpgrade;

        UpdateUI();

        Debug.Log($"House upgraded to level {level}. Contribution now {currentContribution} crop/sec");
    }

    private void ApplyContribution()
    {
        currentContribution = baseCropRate * (float)Math.Pow(multiplierPerUpgrade, level - 1);
        ResourceManager.Instance.IncreaseCropGrowthRate(currentContribution);
    }

    private void RemoveContribution()
    {
        if (currentContribution == 0) return;

        ResourceManager.Instance.IncreaseCropGrowthRate(-currentContribution);
        currentContribution = 0;
    }

    private void ApplyUpkeep()
    {
        currentUpkeep = level * 1f;
        ResourceManager.Instance.IncreaseHouseUpkeep(currentUpkeep);
    }

    private void RemoveUpkeep()
    {
        if (currentUpkeep == 0) return;

        ResourceManager.Instance.IncreaseHouseUpkeep(-currentUpkeep);
        currentUpkeep = 0;
    }

    private void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text = $"House Lv {level}\n+{currentContribution:F2} crops/sec\nUpkeep: {currentUpkeep:F2} crops/sec";
        }
        if (IsMaxLevel())
        {
            if (upgradeTrigger != null)
            {
                upgradeTrigger.SetActive(false);
            }
        }
        else if (upgradeText != null)
        {
            upgradeText.text = $"Upgrade a House with {currentCost} crops?";
        }
    }

    public int GetLevel() => level;

    public bool IsMaxLevel() => level >= maxLevel;
}