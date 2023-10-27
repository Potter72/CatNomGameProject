using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateItemScript : MonoBehaviour
{
    private Transform item;

    private int itemListIndex = 0;

    void Start ()
    {
        item = ItemListScript.itemList[0];
    }

    public void GenerateItem ()
    {
        
    }
}
