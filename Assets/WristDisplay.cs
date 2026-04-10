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
    public InputActionReference clearSaveAction;
    public SaveLoad saveLoad;

    public Vector3 targetScale = new Vector3(0.0005f, 0.0005f, 0.0005f);

    private Color bronzeColor = new Color(0.8f, 0.5f, 0.2f);
    private Color silverColor = new Color(0.83f, 0.83f, 0.83f);
    private Color goldColor = new Color(1f, 0.84f, 0f);

    private bool showTutorial = false;
    private bool showingWelcomeBack = false;

    private bool trophyCrops1kUnlocked;
    private bool trophyCrops10kUnlocked;
    private bool trophyElectricity1kUnlocked;

    private SpringPopInAnimator popInAnimator;

    void Start()
    {
        toggleCanvasAction.action.performed += ToggleCanvasVisibility;
        if (clearSaveAction != null)
        {
            clearSaveAction.action.performed += HandleClearSaveInput;
        }

        if (saveLoad == null)
        {
            saveLoad = FindFirstObjectByType<SaveLoad>();
        }

        popInAnimator = GetComponent<SpringPopInAnimator>();
        if (popInAnimator == null)
        {
            popInAnimator = gameObject.AddComponent<SpringPopInAnimator>();
        }
        popInAnimator.targetScale = targetScale;

        transform.localScale = Vector3.zero;
        TriggerPopIn();
    }

    private void OnDestroy()
    {
        toggleCanvasAction.action.performed -= ToggleCanvasVisibility;
        if (clearSaveAction != null)
        {
            clearSaveAction.action.performed -= HandleClearSaveInput;
        }
    }

    private void HandleClearSaveInput(InputAction.CallbackContext context)
    {
        if (!showTutorial || !showingWelcomeBack) return;
        if (saveLoad == null) return;

        saveLoad.ClearSavedData();
    }

    private void ToggleCanvasVisibility(InputAction.CallbackContext context)
    {
        if (showTutorial)
        {
            ResetToStatusBoard();
        }
        else
        {
            if (!gameObject.activeSelf) TriggerPopIn();
            else gameObject.SetActive(false);
        }
    }

    private void ResetToStatusBoard()
    {
        showTutorial = false;
        showingWelcomeBack = false;
        InfoText.fontSize = 16f;
        TitleText.text = "Status Board";
        HintText.text = "Press X to close/open";
        TitleText.color = Color.black;
        InfoText.color = Color.black;
    }

    void Update()
    {
        if (showTutorial || !gameObject.activeSelf) return;

        CheckAchievementMilestones();
        if (showTutorial) return;
        UpdateStatusText();
    }

    private void CheckAchievementMilestones()
    {
        if (ResourceManager.Instance == null) return;

        if (!trophyCrops1kUnlocked && ResourceManager.Instance.cropCount >= 500f)
        {
            trophyCrops1kUnlocked = true;
            ShowAchievement("Bronze Harvest Trophy", "\nReached 500 crops.", bronzeColor);
            return;
        }

        if (!trophyCrops10kUnlocked && ResourceManager.Instance.cropCount >= 2000f)
        {
            trophyCrops10kUnlocked = true;
            ShowAchievement("Silver Harvest Trophy", "Reached 2,000 crops.", silverColor);
            return;
        }

        if (!trophyElectricity1kUnlocked && ResourceManager.Instance.electricityCount >= 1000f)
        {
            trophyElectricity1kUnlocked = true;
            ShowAchievement("Power Pioneer Trophy", "Reached 1,000 electricity.", goldColor);
        }
    }

    private void ShowAchievement(string trophyTitle, string trophyDescription, Color flashColor)
    {
        showTutorial = true;
        showingWelcomeBack = false;
        TitleText.text = $"{trophyTitle}";
        InfoText.fontSize = 17f;
        InfoText.text = trophyDescription;
        HintText.text = "Press X to dismiss";
        TitleText.color = flashColor;
        InfoText.color = flashColor;
        TriggerPopIn();
    }

    private void UpdateStatusText()
    {
        float cropCount = ResourceManager.Instance.cropCount;
        float cropGrowthRate = ResourceManager.Instance.cropGrowthRate;
        float totalUpkeep = ResourceManager.Instance.houseUpkeepPerSecond;
        float cropBonusMulti = ResourceManager.Instance.curPowerUpMulti;

        string infoString = $"Crops: {cropCount:N0}   (+{cropBonusMulti * (cropGrowthRate - totalUpkeep):F2}/sec)\n";
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

    private void TriggerPopIn()
    {
        gameObject.SetActive(true);
        if (popInAnimator != null)
        {
            popInAnimator.TriggerPopIn();
        }
    }

    public void ShowHouseTutorial()
    {
        showTutorial = true;
        showingWelcomeBack = false;
        TitleText.text = "Tutorial: Houses";
        InfoText.text = "House residents help you collect crops, but houses require upkeep!\n\nUpgrade houses to exponentially increase residents' efficiency.";
        HintText.text = "Press X to dismiss";
        TitleText.color = Color.black;
        InfoText.color = Color.black;
        TriggerPopIn();
    }

    public void ShowCityTutorial()
    {
        showTutorial = true;
        showingWelcomeBack = false;
        TitleText.text = "Tutorial: City";
        InfoText.fontSize = 12f;
        InfoText.text = "Welcome to the city! Now you can access the power plant. It generates electricity, which can be used to build facilities that boost your residents' happiness.\n\nThe happiness index is multiplied to the generation rates of all resources.";
        HintText.text = "Press X to dismiss";
        TitleText.color = Color.black;
        InfoText.color = Color.black;
        TriggerPopIn();
    }

    public void ShowWelcomeBack(float awaySeconds, float cropGain, float electricityGain)
    {
        showTutorial = true;
        showingWelcomeBack = true;
        TitleText.text = "Welcome Back!";
        InfoText.fontSize = 15f;

        TimeSpan away = TimeSpan.FromSeconds(awaySeconds);
        string awayText = $"Away: {away.Hours}h {away.Minutes}m {away.Seconds}s";

        string gainsText = $"Offline gains:\n+{Mathf.Max(0f, cropGain):N0} crops";
        if (!CityWall.activeSelf || electricityGain > 0f)
        {
            gainsText += $"\n+{Mathf.Max(0f, electricityGain):N0} electricity";
        }

        InfoText.text = $"{awayText}\n\n{gainsText}";
        HintText.text = clearSaveAction != null ? "Press X to dismiss, or\nPress Y to start fresh" : "Press X to dismiss";

        Color welcomeColor = new Color(0.12f, 0.57f, 0.95f);
        TitleText.color = welcomeColor;
        InfoText.color = welcomeColor;

        TriggerPopIn();
    }
}
