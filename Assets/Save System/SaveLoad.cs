
using System.Collections.Generic;
using System.IO;
using System.Resources;
using UnityEngine;

[System.Serializable]
public class Serializable
{
    public float cropCount;
    public float electricityCount;
    public float happiness;
    public float houseUpkeepPerSecond;
    public List<bool> houseUnlocks;
    public List<int> houseLevels;
    public List<bool> powerPlantUnlocks;
    public List<int> powerPlantLevels;

    public Serializable(float _cropCount, float _electricityCount, float _happiness, float _houseUpkeepPerSecond, HouseManager[] _houseList, PowerPlantManager[] _powerPlantList)//, Vector3[] _audioNotePositions, string[] _audioNoteNames)
    {
        cropCount = _cropCount;
        electricityCount = _electricityCount;
        happiness = _happiness;
        houseUpkeepPerSecond = _houseUpkeepPerSecond;
        houseUnlocks = new List<bool>();
        houseLevels = new List<int>();
        powerPlantUnlocks = new List<bool>();
        powerPlantLevels = new List<int>();
        foreach (HouseManager house in _houseList)
        {
            houseUnlocks.Add(house.isUnlocked);
            houseLevels.Add(house.GetLevel());
        }
        foreach (PowerPlantManager powerPlant in _powerPlantList)
        {
            powerPlantUnlocks.Add(powerPlant.isUnlocked);
            powerPlantLevels.Add(powerPlant.GetLevel());
        }
    }
}

public class SaveLoad : MonoBehaviour {

    public ResourceManager resourceManager;
    public HouseManager[] houseList;
    public PowerPlantManager[] powerPlantList;
    //public Serializable serializable;

    //public AudioNoteManager audioNoteManager;
    //note list count
    //positions of notes
    //name of files
    public bool autoSave = true;
    public bool debugMode = false;
    public string fileName = "Sample.dat";

    private void Awake()
    {
        if (autoSave)
        {
            Load();
        }
    }

    // void OnGUI()
    // {
    //     if(GUI.Button(new Rect(10, 10, 100, 50), "Save"))
    //     {
    //         Save();
    //     }

    //     if (GUI.Button(new Rect(10, 70, 100, 50), "Load"))
    //     {
    //         Load();
    //     }

    //     if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 50), "NextPart"))
    //     {
    //         objectManager.EnableChildAt((objectManager.enabledObjectIndex + 1) % objectManager.partList.Count);
    //     }

    //     if (GUI.Button(new Rect(Screen.width - 110, 70, 100, 50), "NextColor"))
    //     {
    //         materialManager.EnableMaterialAt((materialManager.enabledColorIndex + 1) % materialManager.materials.Count);
    //     }

    // }

    private void OnDisable()
    {
        if(autoSave){
            Save();
        }
    }

    private string PrepJson()
    {
        Serializable serializable = new Serializable(resourceManager.cropCount, resourceManager.electricityCount, resourceManager.happiness, resourceManager.houseUpkeepPerSecond, houseList, powerPlantList);

        return JsonUtility.ToJson(serializable);
    }

    private void LoadJson(string _jsonString)
    {
        Serializable serializable = JsonUtility.FromJson<Serializable>(_jsonString);
        resourceManager.cropCount = serializable.cropCount;
        resourceManager.electricityCount = serializable.electricityCount;
        // resourceManager.happiness = serializable.happiness;
        // resourceManager.houseUpkeepPerSecond = serializable.houseUpkeepPerSecond;
        for (int i = 0; i < houseList.Length; i++)
        {
            if (serializable.houseUnlocks[i])
            {
                houseList[i].ActivateHouse();
                for (int j = 0; j < serializable.houseLevels[i] - 1; j--)
                {
                    houseList[i].UpgradeHouse();
                }
            }
        }
        //foreach(Vector3 pos in serializable.audioNotePositions)
        //{
        //    Debug.Log("AudioNote Position: " + pos);
        //}
        //audioNoteManager.SetAudioNotesFromLoad(serializable.audioNotePositions, serializable.audioNoteNames);
    }

    public void Save()
    {
        string path = Path.Combine(Application.dataPath, fileName);

        if (debugMode)
            Debug.Log("Path: " + path);



        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(PrepJson()); 
                //foreach(GameObject go in objectsToSerialize)
                //{
                    //string currentSerializedData = JsonUtility.ToJson(s);
                  //  writer.WriteLine(convertObjectToJson(go));
                //}
            }
        }
    }

    public void Load()
    {
        if (debugMode)
            Debug.Log("Loading...");
        
        string path = Path.Combine(Application.dataPath, fileName);
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            using (StreamReader reader = new StreamReader(fs))
            {
                string loadedString = reader.ReadToEnd();
                if (debugMode)
                    Debug.Log("Loaded: " + loadedString);
                LoadJson(loadedString);
                //bool hasFinished = false;
                
                //while (!hasFinished)
                //{
                    
                    //Debug.Log("peek" + reader.Peek());
                    //string currRead = reader.ReadLine();

                    //serializable.Add(JsonUtility.FromJson<Serializable>(currRead));
                    
                    //if (reader.Peek() == -1)
                    //{
                    //    hasFinished = true;
                    //}
                //}
            }
        }

        if (debugMode) {
            // foreach(Serializable s in serializableList)
            //{
            //    Debug.Log(s.LogString());
            //}
            Debug.Log("Loaded");
        }
    }
}
