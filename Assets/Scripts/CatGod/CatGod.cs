using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class CatGod : MonoBehaviour
{
    [SerializeField] private Plate _plate;
    private List<Item.ItemType> _demand = new List<Item.ItemType>();

    private Vector3 _startPos;

    void Awake()
    {
        DemandMoreFood();
        _startPos = transform.position;
    }

    private void DemandMoreFood()
    {
        _demand.Clear();

        int demandAmount = Random.Range(1, 6);

        for(int i = 0; i < demandAmount; i++)
        {
            Item.ItemType newDemand = (Item.ItemType)Random.Range(0, 4);
            _demand.Add(newDemand);
        }

        string wordOfGod = "";

        foreach(Item.ItemType demand in _demand)
        {
            wordOfGod += $"{demand.ToString()} ";
        }

        if(DebugTracker.Instance.DebugOn)
        {
            Debug.Log($"I demand {wordOfGod}");
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

            StartCoroutine(Consume());
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

        yield return new WaitForSeconds(0.5f);

        while (timer < 1f)
        {
            timer += 0.02f;
            transform.position = Vector3.Lerp(_plate.transform.position, _startPos, timer);

            yield return new WaitForSeconds(0.02f);
        }

        DemandMoreFood();
    }
}
