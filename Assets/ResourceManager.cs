using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("Crops")]
    public float cropCount = 0f;
    public float cropGrowthRate = 0f;
    public ToggleCropParticle cropParticle;

    [Header("Electricity")]
    public float electricityCount = 0f;
    public float electricityGrowthRate = 0f;

    [Header("Happiness")]
    public float happiness = 1f;

    [Header("Upkeep (Houses consume food)")]
    public float houseUpkeepPerSecond = 0f;

    [Header("Power Up")]
    public float powerUpMultiplier;
    public float powerUpDuration;

    public float powerUpTimer = 0f;
    private float timer = 0f;
    public float curPowerUpMulti = 1f;

    private void Awake() => Instance = this;

    void Update()
    {
        timer += Time.deltaTime;
        powerUpTimer -= Time.deltaTime;
        if (powerUpTimer > 0f)
        {
            curPowerUpMulti = powerUpMultiplier;
        }
        else
        {
            curPowerUpMulti = 1f;
        }

        if (timer >= 1f)
        {
            cropCount += curPowerUpMulti * cropGrowthRate * happiness - houseUpkeepPerSecond;
            if (cropCount < 0) cropCount = 0;
            if (cropParticle != null)
            {
                int cropBurstCount = Mathf.FloorToInt(curPowerUpMulti * cropGrowthRate * happiness - houseUpkeepPerSecond);
                if (cropBurstCount > 0)
                {
                    cropParticle.TriggerParticle(cropBurstCount);
                }
            }

            electricityCount += Mathf.FloorToInt(electricityGrowthRate * happiness);

            timer -= 1f;
        }
    }
    public void IncreaseHouseUpkeep(float amountPerSecond)
    {
        houseUpkeepPerSecond += amountPerSecond;
    }

    public void RemoveHouseUpkeep(float amountPerSecond)
    {
        houseUpkeepPerSecond -= amountPerSecond;
        if (houseUpkeepPerSecond < 0) houseUpkeepPerSecond = 0;
    }

    public void IncreaseCropCount(float amount)
    {
        cropCount += amount;
    }

    public void IncreaseHappiness(float amount)
    {
        happiness += amount;
    }

    public void IncreaseCropGrowthRate(float amount)
    {
        cropGrowthRate += amount;
    }

    public void IncreaseElectricityGrowthRate(float amount)
    {
        electricityGrowthRate += amount;
    }

    public bool SpendCrops(float amount)
    {
        if (cropCount >= amount)
        {
            cropCount -= amount;
            return true;
        }
        return false;
    }

    public bool SpendElectricity(float amount)
    {
        if (electricityCount >= amount)
        {
            electricityCount -= amount;
            return true;
        }
        return false;
    }

    public void ActivatePowerUp()
    {
        powerUpTimer = powerUpDuration;
    }
}