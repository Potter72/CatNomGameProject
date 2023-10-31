using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;

namespace ProjectCatRoll.Events
{
    public static partial class EventManager
    {
        public static event UnityAction<int> OnItemSpawnRequest;
        //maybe add parameter string id to this so you can decide which spawner you want to spawn items from
        public static void RequestItemSpawn(int numberOfItems)
        {
            OnItemSpawnRequest?.Invoke(numberOfItems);
        }


    }
}
