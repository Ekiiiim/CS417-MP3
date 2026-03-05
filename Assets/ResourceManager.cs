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

    private float timer = 0f;

    private void Awake() => Instance = this;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            cropCount += Mathf.FloorToInt(cropGrowthRate * happiness);
            electricityCount += Mathf.FloorToInt(electricityGrowthRate * happiness);
            timer -= 1f;
        }
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