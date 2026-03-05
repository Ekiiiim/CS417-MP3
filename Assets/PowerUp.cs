using UnityEngine;

public class PowerUpPurchase : MonoBehaviour
{
    [Header("Choose one")]
    public bool affectsCrops = true;
    public bool affectsElectricity = false;

    [Header("Multiplier Factor")]
    [Tooltip("1.25 = +25% rate contribution")]
    public float multiplierFactor = 1.25f;

    public void BuyPowerUp()
    {
        if (affectsCrops)
            ResourceManager.Instance.MultiplyCropMultiplier(multiplierFactor);

        if (affectsElectricity)
            ResourceManager.Instance.MultiplyElectricityMultiplier(multiplierFactor);

        
    }
}