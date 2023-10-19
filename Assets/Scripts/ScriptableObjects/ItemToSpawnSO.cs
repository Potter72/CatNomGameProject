using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace ProjectCatRoll.Elias
{
    [CreateAssetMenu(fileName = "ItemToSpawnSO", menuName = "ScriptableObjects/ItemToSpawnSO", order = 1)]
    public class ItemToSpawnSO : ScriptableObject
    {
        public float SpawnRate;
        public GameObject Prefab;
    }
}
