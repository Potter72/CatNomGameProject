using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
public static partial class EventManager
{
    public static event UnityAction<int> SpawnItems;
    public static void OnSpawnItems(int numberOfItems)
    {
        SpawnItems?.Invoke(numberOfItems);
    }
}
