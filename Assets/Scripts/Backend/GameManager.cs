using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(DebugTracker))]
[RequireComponent(typeof(ItemList))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Ball _player;
    [SerializeField] private ItemList _itemList;
    [SerializeField] private Plate _plate;
    [SerializeField] private CatGod _catGod;

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

        Application.targetFrameRate = 60;
    }

    public ItemList GetItemList()
    {
        return _itemList;
    }
}
