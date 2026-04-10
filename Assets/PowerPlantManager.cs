using UnityEngine;
using TMPro;
using System;

public class PowerPlantManager : MonoBehaviour
{
    public GameObject upgradeTrigger;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradeText;

    public GameObject unlockableWall;
    public GameObject canvas;
    public GameObject portals;

    private float basePowerRate = 1f;
    private float multiplierPerUpgrade = 2f;
    private int level = 1;
    private int maxLevel = 5;
    private int currentCost = 1000;
    private int costMultiplierPerUpgrade = 2;
    private float currentContribution = 0;

    public bool isUnlocked = false;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        RemoveContribution();
    }

    public void ActivatePowerPlant()
    {
        ApplyContribution();
        UpdateUI();
        isUnlocked = true;
    }

    public void UpgradePowerPlant()
    {
        if (IsMaxLevel() || !ResourceManager.Instance.SpendCrops(currentCost)) return;

        RemoveContribution();

        level++;

        ApplyContribution();

        currentCost *= costMultiplierPerUpgrade;

        UpdateUI();
    }

    private void ApplyContribution()
    {
        currentContribution = basePowerRate * (float)Math.Pow(multiplierPerUpgrade, level - 1);
        ResourceManager.Instance.IncreaseElectricityGrowthRate(currentContribution);
    }

    private void RemoveContribution()
    {
        if (currentContribution == 0) return;

        ResourceManager.Instance.IncreaseElectricityGrowthRate(-currentContribution);
        currentContribution = 0;
    }

    private void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text = $"Power Plant Lv {level}\n+{currentContribution:F2} electricity/sec";
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
            upgradeText.text = $"Upgrade Power Plant with {currentCost} crops?";
        }
    }

    public bool IsMaxLevel() => level >= maxLevel;

    public int GetLevel() => level;

    public void ApplySavedState(bool unlocked, int savedLevel)
    {
        RemoveContribution();

        isUnlocked = unlocked;
        level = Mathf.Clamp(savedLevel, 1, maxLevel);
        currentCost = 1000 * (int)Math.Pow(costMultiplierPerUpgrade, Mathf.Max(level - 1, 0));

        if (isUnlocked)
        {
            ActivatePowerPlant();
            unlockableWall.SetActive(false);
            portals.SetActive(true);
            canvas.SetActive(true);
            upgradeTrigger.SetActive(true);
        }
    }
}