using UnityEngine;
using TMPro;

public class WristDisplay : MonoBehaviour
{
    public TextMeshProUGUI StatusInfoText;
    public GameObject ElectricityGenerator;

  void Update()
    {
        float cropCount = ResourceManager.Instance.cropCount;
        float cropGrowthRate = ResourceManager.Instance.cropGrowthRate;

        string infoString = $"Crops: {cropCount}   (+{cropGrowthRate:N0}/sec)\n";
        if (ElectricityGenerator.activeSelf)
        {
            float electricityCount = ResourceManager.Instance.electricityCount;
            float electricityGrowthRate = ResourceManager.Instance.electricityGrowthRate;
            infoString += $"Electricity: {electricityCount}   (+{electricityGrowthRate:N0}/sec)\n";
            infoString += $"\nHappiness: x{ResourceManager.Instance.happiness:F2}";
        }
        StatusInfoText.text = infoString;
    }
}
