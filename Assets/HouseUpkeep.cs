using UnityEngine;

public class HouseUpkeep : MonoBehaviour
{
    [SerializeField] private int upkeepPerSecond = 1;

    private bool registered = false;

    private void OnEnable()
    {
        Register();
    }

    private void OnDisable()
    {
        Unregister();
    }

    public void Register()
    {
        if (registered) return;
        registered = true;
        ResourceManager.Instance.AddHouseUpkeep(upkeepPerSecond);
    }

    public void Unregister()
    {
        if (!registered) return;
        registered = false;
        ResourceManager.Instance.RemoveHouseUpkeep(upkeepPerSecond);
    }

    // Optional if upgrades increase upkeep too
    public void SetUpkeep(int newUpkeep)
    {
        if (registered)
        {
            ResourceManager.Instance.RemoveHouseUpkeep(upkeepPerSecond);
            upkeepPerSecond = newUpkeep;
            ResourceManager.Instance.AddHouseUpkeep(upkeepPerSecond);
        }
        else
        {
            upkeepPerSecond = newUpkeep;
        }
    }
}