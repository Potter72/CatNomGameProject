using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameData
{

}

public interface IDataPersistence
{
    void Load(GameData data);
    void Save(GameData data);

}
public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    public GameData GameData { get => _gameData; }

    private GameData _gameData;
    [Header("File Storage Config")]
    [SerializeField] private string _fileName;

    private FileDataHandler _fileDataHandler;
    private List<IDataPersistence> _dataPersistenceObjects;


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
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
        _gameData = _fileDataHandler.Load();
        if (_gameData == null)
        {
            Debug.Log("No save data found in start.");
            return;
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IDataPersistence[] objects = (IDataPersistence[])FindObjectsByType(typeof(IDataPersistence), FindObjectsSortMode.None);
        return new List<IDataPersistence>(objects);
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }
    public void LoadGame()
    {
        _gameData = _fileDataHandler.Load();

        if (this._gameData == null)
        {
            Debug.Log("No save data found. Making new file.");
            NewGame();
            return;
        }

        foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.Load(_gameData);
        }

    }
    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.Save(_gameData);
        }
        _fileDataHandler.Save(_gameData);
    }

}
