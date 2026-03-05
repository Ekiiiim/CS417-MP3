using UnityEngine;

public class GeneratorContribution : MonoBehaviour
{
    public enum GeneratorType { Crop, Electricity }
    public GeneratorType type = GeneratorType.Crop;

    [Tooltip("How much this generator adds to the rate while it exists.")]
    public int rateAdded = 1;

    private bool applied;

    private void OnEnable()
    {
        Apply();
    }

    private void OnDisable()
    {
        Remove();
    }

    private void Apply()
    {
        if (applied) return;
        applied = true;

        if (type == GeneratorType.Crop)
            ResourceManager.Instance.IncreaseCropGrowthRate(rateAdded);
        else
            ResourceManager.Instance.IncreaseElectricityGrowthRate(rateAdded);
    }

    private void Remove()
    {
        if (!applied) return;
        applied = false;

        if (type == GeneratorType.Crop)
            ResourceManager.Instance.IncreaseCropGrowthRate(-rateAdded);
        else
            ResourceManager.Instance.IncreaseElectricityGrowthRate(-rateAdded);
    }
}