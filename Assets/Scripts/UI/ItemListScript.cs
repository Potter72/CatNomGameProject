using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListScript : MonoBehaviour
{  
    public static Transform[] itemList;
    void Awake ()
    {
        itemList = new Transform[transform.childCount];
        for (int i = 0; i < itemList.Length; i++)
        {
            itemList[i] = transform.GetChild(i);
        }
    }
}
