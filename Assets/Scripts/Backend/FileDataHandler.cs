using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System;
public class FileDataHandler
{
    private string _dataDirPath = "";
    private string _dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                var dataToLoad = File.ReadAllBytes(fullPath);
                string dataToLoadString = System.Text.Encoding.UTF8.GetString(dataToLoad);


                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoadString);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading data from {fullPath}: {e.Message}");
            }
        }
        return loadedData;
    }
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonConvert.SerializeObject(data, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.Write(dataToStore);
                writer.Dispose();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data to {fullPath}: {e.Message} , {e.StackTrace}");
        }
    }
}
