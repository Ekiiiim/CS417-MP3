using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class WristDisplay : MonoBehaviour
{
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI InfoText;
    public TextMeshProUGUI HintText;
    public GameObject CityWall;
    public InputActionReference toggleCanvasAction;

    public Vector3 targetScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
    public float stiffness = 150f;
    public float damping = 10f;
    private float currentScale, velocity;
    private bool isAnimating = false;
    private bool showTutorial = false;

    void Start()
    {
        toggleCanvasAction.action.performed += ToggleCanvasVisibility;
        transform.localScale = Vector3.zero;
        TriggerPopIn();
    }

    private void OnDestroy()
    {
        toggleCanvasAction.action.performed -= ToggleCanvasVisibility;
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
        InfoText.fontSize = 16f;
        TitleText.text = "Status Board";
        HintText.text = "Press X to close/open";
    }

    void Update()
    {
        if (isAnimating) HandleSpringScale();
        if (showTutorial || !gameObject.activeSelf) return;

        UpdateStatusText();
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
        currentScale = 0f;
        velocity = 0f;
        gameObject.SetActive(true);
        isAnimating = true;
    }

    private void HandleSpringScale()
    {
        float force = (1f - currentScale) * stiffness;
        velocity += force * Time.deltaTime;
        velocity -= velocity * damping * Time.deltaTime;
        currentScale += velocity * Time.deltaTime;
        transform.localScale = targetScale * currentScale;

        if (Mathf.Abs(1f - currentScale) < 0.001f && Mathf.Abs(velocity) < 0.001f)
        {
            transform.localScale = targetScale;
            isAnimating = false;
        }
    }

    public void ShowHouseTutorial()
    {
        showTutorial = true;
        TitleText.text = "Tutorial: Houses";
        InfoText.text = "House residents help you collect crops, but houses require upkeep!\n\nUpgrade houses to exponentially increase residents' efficiency.";
        HintText.text = "Press X to dismiss";
        TriggerPopIn();
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
