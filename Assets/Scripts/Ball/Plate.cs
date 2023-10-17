using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private List<GameObject> _itemSlots = new List<GameObject>();
    private int _itemCount = 0;

    public void AddItem(GameObject item)
    {
        item.transform.position = _itemSlots[_itemCount].transform.position;
        _itemCount++;
        if(_itemCount == _itemSlots.Count)
        {
            _itemCount = 0;
        }
    }

    public void FeedGod()
    {
        Debug.Log("NOM NOM NOM NOM NOM");

        //while (_itemSlots.Count > 0)
        //{
        //    Destroy(_itemSlots[0]);
        //    _itemSlots.RemoveAt(0);
        //    _itemCount = 0;
        //}
    }
}
