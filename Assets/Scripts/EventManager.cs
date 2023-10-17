using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;

namespace ProjectCatRoll.Events
{
    public static partial class EventManager
    {
        public static event UnityAction<int> OnItemSpawnRequest;

        public static void RequestItemSpawn(int numberOfItems)
        {
            OnItemSpawnRequest?.Invoke(numberOfItems);
        }
    }
}
