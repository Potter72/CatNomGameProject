using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCatRoll.Elias
{
    public class PrefabSpawner : MonoBehaviour
    {
        [SerializeField]
        private ItemToSpawnSO[] _itemsToSpawn;

        [SerializeField]
        private float _radius; 


        void Start() { 
            SpawnItems(10);
        }

        public void SpawnItems(int numberOfItems)
        {
            for (int i = 0; i < numberOfItems; i++)
            {
                float randomValue = Random.Range(0f, 1f);
                float numForAdding = 0;
                float total = 0; //total of all spawn rates

                for (int j = 0; j < _itemsToSpawn.Length; j++)
                {
                    if (_itemsToSpawn[j] == null)
                    {
                        break;
                    }
                    total += _itemsToSpawn[j].SpawnRate; //add spawn rate of current item to total
                }
                for (int j = 0; j < numberOfItems; j++)
                {
                    // if (_itemsToSpawn[j%_itemsToSpawn.Length] == null) //commenting this out because i think it should work anyways
                    // {
                    //     continue;
                    // }

                    if (_itemsToSpawn[j%_itemsToSpawn.Length].SpawnRate / total + numForAdding >= randomValue)
                    {
                        
                        //create transform to spawn at
                        Vector3 spawnPos = new Vector3(
                            Random.Range(-_radius, _radius),
                            0,
                            Random.Range(-_radius, _radius)
                        );

                        Instantiate(
                            _itemsToSpawn[j%_itemsToSpawn.Length].Prefab,
                            spawnPos,
                            Quaternion.identity
                        );
                        break;
                    }
                    else
                    {
                        numForAdding += _itemsToSpawn[j%_itemsToSpawn.Length].SpawnRate / total;
                    }
                }
            }
        }
    }
}
