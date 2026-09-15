using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;


[Serializable]
public class GameLogData
{
    public float total_playtime_duration;
}


[Serializable]
public class GameLogDataList
{
    public List<GameLogData> gameLogs = new List<GameLogData>();
}


public class Logger : MonoBehaviour
{
    [SerializeField]
    private float total_playtime_duration = 0f;


    void Update()
    {
        total_playtime_duration += Time.deltaTime;
    }


    void OnDisable()
    {
        string directory_path = Path.Combine(Application.dataPath, "Logs");
        string file_path = Path.Combine(directory_path, "GameLog.json");
        Debug.Log($"Saving to: {file_path}");

        if (!Directory.Exists(directory_path))                                      // ensure the directory exists
        {
            Directory.CreateDirectory(directory_path);
        }


        GameLogDataList gameLog_datalist;
        try
        {
            string json_file = File.ReadAllText(file_path);
            gameLog_datalist = JsonUtility.FromJson<GameLogDataList>(json_file);    // read existing JSON file and create a new one if it doesn't exist
        }
        catch (FileNotFoundException)
        {
            Debug.LogWarning("File not found, creating a new list");
            gameLog_datalist = new GameLogDataList();
        }


        GameLogData gameLog_data = new GameLogData                                   // add new data to the list called 
        {
            total_playtime_duration = total_playtime_duration,
        };

        gameLog_datalist.gameLogs.Add(gameLog_data);


        try                                                                         
        {
            string json = JsonUtility.ToJson(gameLog_datalist);                     // write the update list back to JSON file (overwrite)
            File.WriteAllText(file_path, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error writing to file: {e.Message}");
        }
    }
}
