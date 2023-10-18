using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    [SerializeField] private List<Item> _itemList = new List<Item>();

    public Item GetRandomItem()
    {
        int index = Random.Range(0, _itemList.Count);
        return _itemList[index];
    }

    public void RemoveItem(Item item)
    {
        _itemList.Remove(item);
    }
}
