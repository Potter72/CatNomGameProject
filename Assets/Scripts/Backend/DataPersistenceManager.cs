using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using System.Linq;

public class GameData
{
    //unsure what to put in here
    public Vector3 PlayerPosition;
}

public class SettingsData
{
    //floats for volume
    public float MasterVolume;
    public float MusicVolume;
    public float EffectVolume;
    public float AmbienceVolume;
    public float UIVolume;

    //resolution
    public int ResolutionWidth;
    public int ResolutionHeight;
    public bool Fullscreen;

}

public interface IDataPersistenceSettingsData
{
    public void LoadSettings(SettingsData data);
    public void SaveSettings(SettingsData data);
}

public interface IDataPersistenceGameData
{
    public void LoadGame(GameData data);
    public void SaveGame(GameData data);

}
public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    public GameData GameData { get => _gameData; }
    private GameData _gameData;


    public SettingsData SettingsData { get => _settingsData; }
    private SettingsData _settingsData;


    [Header("File Name")]
    [SerializeField] private string _fileName;

    private FileDataHandler _fileDataHandler;
    private List<IDataPersistenceGameData> _gameDataPersistenceObjects;
    private List<IDataPersistenceSettingsData> _settingsDataPersistenceObjects;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        this._fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
        _gameDataPersistenceObjects = FindAllGameDataPersistenceObjects();
        _settingsDataPersistenceObjects = FindAllSettingsDataPersistenceObjects();
    }

    private List<IDataPersistenceGameData> FindAllGameDataPersistenceObjects()
    {
        IEnumerable<IDataPersistenceGameData> objects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistenceGameData>();
        return new List<IDataPersistenceGameData>(objects);
    }

    private List<IDataPersistenceSettingsData> FindAllSettingsDataPersistenceObjects()
    {
        IDataPersistenceSettingsData[] objects = (IDataPersistenceSettingsData[])FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None).OfType<IDataPersistenceSettingsData>();
        return new List<IDataPersistenceSettingsData>(objects);
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }
    public void LoadGame()
    {
        _gameData = _fileDataHandler.LoadGame();

        if (this._gameData == null)
        {
            Debug.Log("No save data found. Making new file.");
            NewGame();
            return;
        }

        foreach (IDataPersistenceGameData dataPersistenceObject in _gameDataPersistenceObjects)
        {
            dataPersistenceObject.LoadGame(_gameData);
        }

    }
    public void SaveGame()
    {
        foreach (IDataPersistenceGameData dataPersistenceObject in _gameDataPersistenceObjects)
        {
            dataPersistenceObject.SaveGame(_gameData);
        }
        _fileDataHandler.SaveGame(_gameData);
    }

    public void LoadSettings()
    {
        _settingsData = _fileDataHandler.LoadSettings();

        if (this._settingsData == null)
        {
            Debug.Log("No settings data found. Making new file.");
            _settingsData = new SettingsData();
            return;
        }

        foreach (IDataPersistenceSettingsData dataPersistenceObject in _settingsDataPersistenceObjects)
        {
            dataPersistenceObject.LoadSettings(_settingsData);
        }
    }

    //voids for settings
    public void SaveSettings()
    {
        foreach (IDataPersistenceSettingsData dataPersistenceObject in _settingsDataPersistenceObjects)
        {
            dataPersistenceObject.SaveSettings(_settingsData);
        }
        _fileDataHandler.SaveSettings(_settingsData);
    }

}
