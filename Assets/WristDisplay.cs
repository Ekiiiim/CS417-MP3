using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class WristDisplay : MonoBehaviour
{
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI InfoText;
    public TextMeshProUGUI HintText;
    public GameObject CityWall;
    public InputActionReference toggleCanvasAction;

    private bool showTutorial = false;

    void Start()
    {
        toggleCanvasAction.action.performed += ToggleCanvasVisibility;
    }

    private void OnDestroy()
    {
        toggleCanvasAction.action.performed -= ToggleCanvasVisibility;
    }

    private void ToggleCanvasVisibility(InputAction.CallbackContext context)
    {
        if (showTutorial) {
            showTutorial = false;
            InfoText.fontSize = 16f;
            TitleText.text = "Status Board";
            HintText.text = "Press X to close/open";
        } else {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    void Update()
    {
        if (showTutorial) return;

        float cropCount = ResourceManager.Instance.cropCount;
        float cropGrowthRate = ResourceManager.Instance.cropGrowthRate;
        float totalUpkeep = ResourceManager.Instance.houseUpkeepPerSecond;
        float cropBonusMulti = ResourceManager.Instance.curPowerUpMulti;

        string infoString = $"Crops: {cropCount:N0}   (+{cropBonusMulti*(cropGrowthRate - totalUpkeep):F2}/sec)\n";
        if (!CityWall.activeSelf)
        {
            float electricityCount = ResourceManager.Instance.electricityCount;
            float electricityGrowthRate = ResourceManager.Instance.electricityGrowthRate;
            infoString += $"Electricity: {electricityCount:N0}   (+{electricityGrowthRate:F2}/sec)\n";
            infoString += $"\nHappiness: x{ResourceManager.Instance.happiness:F2}";
        }
        if (ResourceManager.Instance.powerUpTimer > 0f)
        {
            infoString += $"\n!!!CROP GROWTH BONUS!!! (ends in {Math.Ceiling(ResourceManager.Instance.powerUpTimer)})";
        }
        InfoText.text = infoString;
    }

    public void ShowHouseTutorial()
    {
        showTutorial = true;
        TitleText.text = "Tutorial: Houses";
        InfoText.text = "House residents help you collect crops, but houses require upkeep!\n\nUpgrade houses to exponentially increase residents' efficiency.";
        HintText.text = "Press X to dismiss";
    }

    public void ShowCityTutorial()
    {
        showTutorial = true;
        TitleText.text = "Tutorial: City";
        InfoText.fontSize = 12f;
        InfoText.text = "Welcome to the city! Now you can access the power plant. It generates electricity, which can be used to build facilities that boost your residents' happiness.\n\nThe happiness index is multiplied to the generation rates of all resources.";
        HintText.text = "Press X to dismiss";
    }
}
