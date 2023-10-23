using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changeimage : MonoBehaviour
{
    [SerializeField] private int items;
    [SerializeField] private GameObject item;
    public void GenerateItem()
    {
        for (int x = 0; x < items; x++)
        {
            var spawnedItem = Instantiate(item, new Vector3(5, 0), Quaternion.identity);
        }
    }
}
