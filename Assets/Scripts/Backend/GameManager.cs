using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using ProjectCatRoll.Events;

public class CatGodSizeEventArgs : EventArgs
{
    public float Size;

    public CatGodSizeEventArgs(float size)
    {
        Size = size;
    }
}

[RequireComponent(typeof(DebugTracker))]
[RequireComponent(typeof(ItemList))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    
    [SerializeField] private Ball _player;
    [SerializeField] private ItemList _itemList;
    [SerializeField] private Plate _plate;
    [SerializeField] private CatGod _catGod;
    
    public float CatGodSize { get; private set; }

    private DebugTracker _debugTracker;
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        //Application.targetFrameRate = 60;

        _player = GameObject.FindFirstObjectByType<Ball>();
        _itemList = GetComponent<ItemList>();
        _plate = GameObject.FindFirstObjectByType<Plate>();
        _catGod = GameObject.FindFirstObjectByType<CatGod>();
        _debugTracker = GetComponent<DebugTracker>();
    }

    public void Log(string s)
    {
        if (_debugTracker.DebugOn)
        {
            Debug.Log(s);
        }
    }
    public Ball GetPlayer()
    {
        return _player;
    }
    
    public Plate GetPlate()
    {
        return _plate;
    }

    public void SaveGodSize(float size)
    {
        CatGodSize = size;
        EventManager.SendCatGodSizeChange(size);
    }

    public ItemList GetItemList()
    {
        return _itemList;
    }

    public CatGod GetCatGod()
    {
        return _catGod;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
}
