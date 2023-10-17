using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private List<GameObject> _itemSlots = new List<GameObject>();
    [SerializeField] private CatGod _catGod;

    private List<Item> _items = new List<Item>();
    private int _itemCount = 0;

    public void AddItem(Item item)
    {
        item.transform.position = _itemSlots[_itemCount].transform.position;
        _items.Add(item);

        _itemCount++;
        if(_itemCount == _itemSlots.Count)
        {
            _itemCount = 0;
        }
    }

    // Attempts to feed the cat god when all the
    // items have been sent from the ball
    // See GoToPlate in Item
    public void FeedGod()
    {
        _catGod.Feed(_items);
    }

    // Cat god noms the items
    public void RemoveAllItems()
    {
        while(_items.Count > 0)
        {
            Destroy(_items[0].gameObject);
            _items.RemoveAt(0);
        }
    }
}
