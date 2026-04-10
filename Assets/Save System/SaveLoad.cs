
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public long savedUnixTime;

    public float cropCount;
    public float electricityCount;
    public float happiness;
    public float powerUpTimer;

    public List<bool> houseUnlocks;
    public List<int> houseLevels;
    public List<bool> powerPlantUnlocks;
    public List<int> powerPlantLevels;
    public bool coffeeShopUnlocked;
    public bool superMarketUnlocked;
    public bool stadiumUnlocked;

    public SaveData(ResourceManager resourceManager, HouseManager[] houses, PowerPlantManager[] powerPlants, GameObject coffeeShop, GameObject superMarket, GameObject stadium)
    {
        savedUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        cropCount = resourceManager.cropCount;
        electricityCount = resourceManager.electricityCount;
        happiness = resourceManager.happiness;
        powerUpTimer = resourceManager.powerUpTimer;

        houseUnlocks = new List<bool>();
        houseLevels = new List<int>();
        powerPlantUnlocks = new List<bool>();
        powerPlantLevels = new List<int>();

        foreach (HouseManager house in houses)
        {
            houseUnlocks.Add(house.isUnlocked);
            houseLevels.Add(house.GetLevel());
        }

        foreach (PowerPlantManager powerPlant in powerPlants)
        {
            powerPlantUnlocks.Add(powerPlant.isUnlocked);
            powerPlantLevels.Add(powerPlant.GetLevel());
        }

        coffeeShopUnlocked = coffeeShop != null && coffeeShop.activeSelf;
        superMarketUnlocked = superMarket != null && superMarket.activeSelf;
        stadiumUnlocked = stadium != null && stadium.activeSelf;
    }
}

public class SaveLoad : MonoBehaviour
{

    public ResourceManager resourceManager;
    public HouseManager[] houseList;
    public PowerPlantManager[] powerPlantList;

    [Header("Additional Buildings")]
    public GameObject unlockableCoffeeShop;
    public GameObject coffeeShopUnlockTrigger;
    public GameObject unlockableSuperMarket;
    public GameObject superMarketUnlockTrigger;
    public GameObject unlockableStadium;
    public GameObject stadiumUnlockTrigger;

    [Header("Welcome Back")]
    public WristDisplay wristDisplay;
    public UnityEvent onWelcomeBack;

    [Header("Save Options")]
    public bool autoSave = true;
    public bool debugMode = false;
    public string fileName = "Sample.dat";

    private string SavePath => Path.Combine(Application.persistentDataPath, fileName);
    private bool skipAutoSave;

    private void Start()
    {
        if (autoSave)
        {
            Load();
        }
    }

    private void OnDisable()
    {
        if (autoSave && !skipAutoSave)
        {
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        if (autoSave && !skipAutoSave)
        {
            Save();
        }
    }

    private string BuildJson()
    {
        SaveData saveData = new SaveData(resourceManager, houseList, powerPlantList, unlockableCoffeeShop, unlockableSuperMarket, unlockableStadium);
        return JsonUtility.ToJson(saveData);
    }

    private void LoadFromJson(string jsonString)
    {
        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonString);
        if (saveData == null)
        {
            if (debugMode)
            {
                Debug.LogWarning("SaveLoad: Invalid save file content.");
            }
            return;
        }

        resourceManager.cropCount = saveData.cropCount;
        resourceManager.electricityCount = saveData.electricityCount;
        resourceManager.happiness = saveData.happiness;
        resourceManager.powerUpTimer = Mathf.Max(0f, saveData.powerUpTimer);

        resourceManager.cropGrowthRate = 0f;
        resourceManager.electricityGrowthRate = 0f;
        resourceManager.houseUpkeepPerSecond = 0f;

        int houseCount = Mathf.Min(houseList.Length, saveData.houseUnlocks.Count, saveData.houseLevels.Count);
        for (int i = 0; i < houseCount; i++)
        {
            houseList[i].ApplySavedState(saveData.houseUnlocks[i], saveData.houseLevels[i]);
        }

        int powerPlantCount = Mathf.Min(powerPlantList.Length, saveData.powerPlantUnlocks.Count, saveData.powerPlantLevels.Count);
        for (int i = 0; i < powerPlantCount; i++)
        {
            powerPlantList[i].ApplySavedState(saveData.powerPlantUnlocks[i], saveData.powerPlantLevels[i]);
        }

        ApplyBuildingUnlockState(unlockableCoffeeShop, coffeeShopUnlockTrigger, saveData.coffeeShopUnlocked);
        ApplyBuildingUnlockState(unlockableSuperMarket, superMarketUnlockTrigger, saveData.superMarketUnlocked);
        ApplyBuildingUnlockState(unlockableStadium, stadiumUnlockTrigger, saveData.stadiumUnlocked);

        ApplyOfflineEulerProgress(saveData.savedUnixTime);
    }

    private void ApplyBuildingUnlockState(GameObject building, GameObject unlockTrigger, bool unlocked)
    {
        if (building != null)
        {
            building.SetActive(unlocked);
        }

        if (unlockTrigger != null)
        {
            unlockTrigger.SetActive(!unlocked);
        }
    }

    private void ApplyOfflineEulerProgress(long savedUnixTime)
    {
        long nowUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        float elapsedSeconds = Mathf.Max(0f, nowUnixTime - savedUnixTime);
        if (elapsedSeconds <= 0f) return;

        float effectivePowerUp = resourceManager.powerUpTimer > 0f ? resourceManager.powerUpMultiplier : 1f;

        float cropRatePerSecond = effectivePowerUp * resourceManager.cropGrowthRate * resourceManager.happiness - resourceManager.houseUpkeepPerSecond;
        float electricityRatePerSecond = Mathf.Floor(resourceManager.electricityGrowthRate * resourceManager.happiness);

        float cropGain = cropRatePerSecond * elapsedSeconds;
        float electricityGain = electricityRatePerSecond * elapsedSeconds;

        resourceManager.cropCount = Mathf.Max(0f, resourceManager.cropCount + cropGain);
        resourceManager.electricityCount = Mathf.Max(0f, resourceManager.electricityCount + electricityGain);
        resourceManager.powerUpTimer = Mathf.Max(0f, resourceManager.powerUpTimer - elapsedSeconds);

        if (elapsedSeconds > 0f)
        {
            if (wristDisplay != null)
            {
                wristDisplay.ShowWelcomeBack(elapsedSeconds, cropGain, electricityGain);
            }
            onWelcomeBack?.Invoke();
        }
    }

    public void Save()
    {
        skipAutoSave = false;

        if (debugMode)
            Debug.Log("Save path: " + SavePath);

        using (FileStream fs = new FileStream(SavePath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(BuildJson());
            }
        }
    }

    public void Load()
    {
        if (debugMode)
            Debug.Log("Loading...");

        if (!File.Exists(SavePath))
        {
            if (debugMode)
                Debug.Log("SaveLoad: No save file found. Starting fresh.");
            return;
        }

        using (FileStream fs = new FileStream(SavePath, FileMode.Open))
        {
            using (StreamReader reader = new StreamReader(fs))
            {
                string loadedString = reader.ReadToEnd();
                if (debugMode)
                    Debug.Log("Loaded: " + loadedString);
                LoadFromJson(loadedString);
            }
        }

        if (debugMode)
        {
            Debug.Log("Loaded");
        }
    }

    public void ClearSavedData()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            if (debugMode)
            {
                Debug.Log("SaveLoad: Save data cleared.");
            }
        }
        else if (debugMode)
        {
            Debug.Log("SaveLoad: No save file to clear.");
        }

        skipAutoSave = true;

        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }
}
