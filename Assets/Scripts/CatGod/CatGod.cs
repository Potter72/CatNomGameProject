using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class CatGod : MonoBehaviour
{
    [Serializable]
    public class LevelInfo
    {
        public int ClearsRequired;
        public int TypeVariety;
        public int MinFoodVariety;
        public int MaxFoodVariety;
    }
    
    [SerializeField] private DemandUI _demandUI;
    [SerializeField] private Transform _mouth;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private int _minDemand = 20;
    [SerializeField] private int _maxDemand = 50;

    [SerializeField] private List<LevelInfo> _levels;
    private int _currentLevel = 3;
    
    private Plate _plate;
    private List<Item.ItemType> _demand = new List<Item.ItemType>();
    private List<Item.ItemType> _types = new List<Item.ItemType>();
    
    private Vector3 _startPos;

    private float _size;

    void Start()
    {
        _startPos = transform.position;
        _plate = GameManager.Instance.GetPlate();
        
        for (int i = 0; i < Enum.GetNames(typeof(Item.ItemType)).Length; i++)
        {
            _types.Add((Item.ItemType)i);
        }
        
        DemandMoreFood3();
    }

    private void DemandMoreFood3()
    {
        List<Item.ItemType> remainingTypes = _types;
        List<Item.ItemType> demandedTypes = new List<Item.ItemType>();
        List<int> amountOfItems = new List<int>();

        for (int i = 0; i < _levels[_currentLevel].TypeVariety; i++)
        {
            Item.ItemType newDemand = remainingTypes[Random.Range(0, remainingTypes.Count)];
            remainingTypes.Remove(newDemand);
            demandedTypes.Add(newDemand);
        }

        for (int i = 0; i < demandedTypes.Count; i++)
        {
            int min = _levels[_currentLevel].MinFoodVariety;
            int max = _levels[_currentLevel].MaxFoodVariety + 1;
            amountOfItems.Add(Random.Range(min, max));
        }

        _demand = demandedTypes;
        
        _demandUI.SetDemand(demandedTypes, amountOfItems);
    }
    
    // private void DemandMoreFood2()
    // {
    //     List<Item.ItemType> remainingTypes = _types;
    //     List<Item.ItemType> demandedTypes = new List<Item.ItemType>();
    //     List<int> amountOfItems = new List<int>();
    //
    //     int amountOfTypes = Random.Range(1 ,remainingTypes.Count);
    //     int itemsLeft = Random.Range(_minDemand, _maxDemand);
    //     
    //     for (int i = 0; i < amountOfTypes; i++)
    //     {
    //         Item.ItemType newDemand = remainingTypes[Random.Range(0, remainingTypes.Count)];
    //         remainingTypes.Remove(newDemand);
    //         demandedTypes.Add(newDemand);
    //     }
    //     
    //     demandedTypes.Sort();
    //
    //     for (int i = 0; i < demandedTypes.Count; i++)
    //     {
    //         if (i == demandedTypes.Count - 1)
    //         {
    //             amountOfItems.Add(itemsLeft);
    //             break;
    //         }
    //
    //         int takenAmount = Random.Range(1, itemsLeft - (demandedTypes.Count - i - 1));
    //         itemsLeft -= takenAmount;
    //         amountOfItems.Add(takenAmount);
    //     }
    //     
    //     _demandUI.SetDemand(demandedTypes, amountOfItems);
    // }
    //
    // private void DemandMoreFood()
    // {
    //     _demand.Clear();
    //
    //     int demandAmount = Random.Range(1, 6);
    //
    //     for(int i = 0; i < demandAmount; i++)
    //     {
    //         Item.ItemType newDemand = (Item.ItemType)Random.Range(0, 4);
    //         _demand.Add(newDemand);
    //     }
    //
    //     string wordOfGod = "";
    //
    //     foreach(Item.ItemType demand in _demand)
    //     {
    //         wordOfGod += $"{demand.ToString()} ";
    //     }
    //
    //     _text.text = $"I demand {wordOfGod}";
    //
    //     //if(DebugTracker.Instance.DebugOn)
    //     //{
    //     //    Debug.Log($"I demand {wordOfGod}");
    //     //}
    // }

    public void UpdateFood(Item.ItemType foodType)
    {
        if (_demand.Contains(foodType))
        {
            _demandUI.ReduceByOne(_demand.IndexOf(foodType));
        }
    }
    
    public void Feed(List<Item> items)
    {
        if(InspectFood(items))
        {
            if(DebugTracker.Instance.DebugOn)
            {
                //Debug.Log("AAAA");
            }

            foreach (Item i in items)
            {
                Vector3 rp = _plate.transform.position + new Vector3(0, 1, 0) * 8f + Random.insideUnitSphere * 3f;
                i.SendToMouth(_mouth, rp);
            }
            
            // Old feeding routine;
            // StartCoroutine(Consume());
        }
    }

    private bool InspectFood(List<Item> items)
    {
        List<Item.ItemType> tempList = _demand;

        foreach(Item item in items)
        {
            if(!tempList.Contains(item.FoodType))
            {
                continue;
            }

            int index = tempList.IndexOf(item.FoodType);
            tempList.RemoveAt(index);

            if(tempList.Count == 0)
            {
                return true;
            }
        }

        return false;
    }

    IEnumerator Consume()
    {
        float timer = 0f;

        while(timer < 1f)
        {
            timer += 0.02f;
            transform.position = Vector3.Lerp(_startPos, _plate.transform.position, timer);

            yield return new WaitForSeconds(0.02f);
        }

        timer = 0f;
        _plate.RemoveAllItems();

        if(DebugTracker.Instance.DebugOn)
        {
            Debug.Log("NOM NOM NOM NOM NOM");
        }

        for (int i = 0; i < 5; i++)
        {
            _size += _demand.Count / 5;
            Debug.Log(_size);
            yield return new WaitForSeconds(0.1f);
        }

        while (timer < 1f)
        {
            timer += 0.02f;
            transform.position = Vector3.Lerp(_plate.transform.position, _startPos, timer);

            yield return new WaitForSeconds(0.02f);
        }

        DemandMoreFood3();
    }
}
