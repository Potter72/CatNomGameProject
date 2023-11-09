using System.Runtime.CompilerServices;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using UnityEngine;
using System;
using TMPro;

public class CatGod : MonoBehaviour
{
    [Serializable]
    public class LevelInfo
    {
        public int ClearsRequired;
        public int TypeVariety;
        public int DemandVariety;
        public int MinFoodVariety;
        public int MaxFoodVariety;
    }
    
    [SerializeField] private DemandUI _demandUI;
    [SerializeField] private Transform _mouth;

    [SerializeField] private List<LevelInfo> _levels;
    private int _currentLevel = 0;
    
    private Plate _plate;
    private List<Item.ItemType> _types = new List<Item.ItemType>();
    private List<Item.ItemType> _demand = new List<Item.ItemType>();
    private List<int> _amount = new List<int>();
    
    private Vector3 _startPos;
    private float _size;


    [Header("EatingAnimation")]
    [SerializeField] Animator catGodAnimator;
    [SerializeField] float eatAnimWaitTime = 1;
    [SerializeField] GameObject succObject;


    void Awake()
    {
        _size = transform.localScale.x;
        _startPos = transform.position;
        _plate = GameManager.Instance.GetPlate();
        
        for (int i = 0; i < Enum.GetNames(typeof(Item.ItemType)).Length; i++)
        {
            _types.Add((Item.ItemType)i);
        }
        
        DemandMoreFood();
    }

    private void DemandMoreFood()
    {
        List<Item.ItemType> remainingTypes = new(_types);
        List<Item.ItemType> demandedTypes = new();
        List<int> amountOfItems = new();

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
        _amount = amountOfItems;
        
        _demandUI.SetDemand(demandedTypes, amountOfItems);
    }
    
    public void UpdateFood(Item.ItemType foodType)
    {
        if (!_demand.Contains(foodType)) return;
        
        int index = _demand.IndexOf(foodType);

        if (_amount[index] == 0) return;
        
        _amount[index]--;
        _demandUI.ReduceByOne(index);

        if (_amount[index] > 0) return;
        
        // _demand.RemoveAt(index);
        // _amount.RemoveAt(index);
        _demandUI.SetTypeComplete(index);

        if (_demand.Count > 0) return;
        
        // FinishRequest();
    }

    #region Finish
    public void FinishRequest()
    {
        if (--_levels[_currentLevel].ClearsRequired == 0)
        {            
            _currentLevel++;
            // Play cutscene/animation
            CutsceneManager.Instance.PlayNext();
        }

        if (_currentLevel >= _levels.Count)
        {
            Debug.Log("<color=yellow>You Win!</color>");
            return;
        }
        
        _demandUI.RemoveAllTypes();
        DemandMoreFood();
    }
    
    public void Feed(List<Item> items)
    {

        foreach (int amount in _amount)
        {
            if (amount > 0) return;
        }

        foreach (Item i in items)
        {
            Vector3 rp = _plate.transform.position + new Vector3(0, 1, 0) * 8f + Random.insideUnitSphere * 3f;
            i.SendToMouth(_mouth, rp);
        }

        //starting food animation stuff
        if (catGodAnimator != null) 
        { 
            Invoke("StartEatAnimation", eatAnimWaitTime); 
            Invoke("StartEatSucc", eatAnimWaitTime - 0.05f); 

        }

        _demand.Clear();
        items.Clear();
        StartCoroutine(Wait());
    }
    private void StartEatAnimation()
    {
        catGodAnimator.SetTrigger("Eat");
    }
    private void StartEatSucc()
    {
        Invoke("StopEatSucc", 2f);
        succObject.SetActive(true);
    }
    private void StopEatSucc()
    {
        succObject.SetActive(false);
    }

    #endregion

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

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        
        FinishRequest();
    }
    
    // IEnumerator Consume()
    // {
    //     float timer = 0f;
    //
    //     while(timer < 1f)
    //     {
    //         timer += 0.02f;
    //         transform.position = Vector3.Lerp(_startPos, _plate.transform.position, timer);
    //
    //         yield return new WaitForSeconds(0.02f);
    //     }
    //
    //     timer = 0f;
    //     _plate.RemoveAllItems();
    //
    //     if(DebugTracker.Instance.DebugOn)
    //     {
    //         Debug.Log("NOM NOM NOM NOM NOM");
    //     }
    //
    //     for (int i = 0; i < 5; i++)
    //     {
    //         _size += _demand.Count / 5;
    //         Debug.Log(_size);
    //         yield return new WaitForSeconds(0.1f);
    //     }
    //
    //     while (timer < 1f)
    //     {
    //         timer += 0.02f;
    //         transform.position = Vector3.Lerp(_plate.transform.position, _startPos, timer);
    //
    //         yield return new WaitForSeconds(0.02f);
    //     }
    //
    //     DemandMoreFood();
    // }
    
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
    

}
