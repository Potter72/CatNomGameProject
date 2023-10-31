using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using UnityEngine;
using TMPro;

public class CatGod : MonoBehaviour
{
    [SerializeField] private Transform _mouth;
    [SerializeField] private TextMeshProUGUI _text;
    private Plate _plate;
    private List<Item.ItemType> _demand = new List<Item.ItemType>();
    
    private Vector3 _startPos;
    private float _size;


    void Awake()
    {
        _size = transform.localScale.x;
        DemandMoreFood();
        _startPos = transform.position;
        _plate = GameManager.Instance.GetPlate();
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

        _text.text = $"I demand {wordOfGod}";

        //if(DebugTracker.Instance.DebugOn)
        //{
        //    Debug.Log($"I demand {wordOfGod}");
        //}
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

        StartCoroutine(LerpToSize(_size + 0.2f, 0.2f));
        yield return new WaitForSeconds(0.5f);

        while (timer < 1f)
        {
            timer += 0.02f;
            transform.position = Vector3.Lerp(_plate.transform.position, _startPos, timer);

            yield return new WaitForSeconds(0.02f);
        }

        DemandMoreFood();
    }

    IEnumerator LerpToSize(float size, float duration)
    {
        float t = 0;
        while(t <1)
        {
            _size = Mathf.Lerp(_size, size, t);
            Debug.Log(_size + " " + size + " " + t);
            transform.localScale = new Vector3(_size, _size, _size);
            t += Time.deltaTime / duration;
            yield return null;
        }
        GameManager.Instance.SaveGodSize(_size);
    }
}
