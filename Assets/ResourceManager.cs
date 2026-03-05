using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("Crops")]
    public int cropCount = 0;
    public int cropGrowthRate = 0;

    [Header("Electricity")]
    public int electricityCount = 0;
    public int electricityGrowthRate = 0;

    [Header("Happiness")]
    public float happiness = 1f;

    [Header("Multipliers (Power-ups)")]
    public float cropMultiplier = 1f;
    public float electricityMultiplier = 1f;

    [Header("Unlocks")]
    public bool electricityUnlocked = false;

    [Header("Upkeep (Houses consume food)")]
    public int houseUpkeepPerSecond = 0;

    private float timer = 0f;

    private void Awake() => Instance = this;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            //cropCount += Mathf.FloorToInt(cropGrowthRate * happiness);
            //electricityCount += Mathf.FloorToInt(electricityGrowthRate * happiness);
            // After producing crops:
            cropCount += Mathf.FloorToInt(cropGrowthRate * cropMultiplier * happiness);

            // Houses consume crops over time:
            int upkeepCost = Mathf.FloorToInt(houseUpkeepPerSecond * happiness);
            cropCount -= upkeepCost;

            // Don’t go negative:
            if (cropCount < 0) cropCount = 0;

            if (electricityUnlocked)
            {
                electricityCount += Mathf.FloorToInt(electricityGrowthRate * electricityMultiplier * happiness);
            }
            timer -= 1f;
            
        }
    }
    public void AddHouseUpkeep(int amountPerSecond)
    {
        houseUpkeepPerSecond += amountPerSecond;
    }

    public void RemoveHouseUpkeep(int amountPerSecond)
    {
        houseUpkeepPerSecond -= amountPerSecond;
        if (houseUpkeepPerSecond < 0) houseUpkeepPerSecond = 0;
    }
    public void IncreaseCropCount(int amount)
    {
        cropCount += amount;
    }

    public void IncreaseHappiness(float amount)
    {
        happiness += amount;
    }

    public void IncreaseCropGrowthRate(int amount)
    {
        cropGrowthRate += amount;
    }

    public void IncreaseElectricityGrowthRate(int amount)
    {
        electricityGrowthRate += amount;
    }

    public bool SpendCrops(int amount)
    {
        if (cropCount >= amount)
        {
            cropCount -= amount;
            return true;
        }
        return false;
    }
    public void MultiplyCropMultiplier(float factor)
    {
        cropMultiplier *= factor;
    }

    public void MultiplyElectricityMultiplier(float factor)
    {
        electricityMultiplier *= factor;
    }

    public void UnlockElectricity()
    {
        electricityUnlocked = true;
    }
    public bool SpendElectricity(int amount)
    {
        if (electricityCount >= amount)
        {
            electricityCount -= amount;
            return true;
        }
        return false;
    }
}