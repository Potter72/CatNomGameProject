using System.Collections.Generic;
using UnityEngine.InputSystem;
using ProjectCatRoll.Events;
using System.Collections;
using UnityEngine;

namespace ProjectCatRoll.Elias
{
    public class PrefabSpawner : MonoBehaviour
    {
        [SerializeField]
        private ItemToSpawnSO[] _itemsToSpawn;

        [SerializeField]
        private float _radius;

        private List<GameObject> _spawnedItems;

        private void Awake()
        {
            _spawnedItems = new();
        }

        private void Start()
        {
            //SpawnItems(50);
        }

        private void OnEnable()
        {
            EventManager.OnItemSpawnRequest += SpawnItems;
        }

        private void OnDisable()
        {
            EventManager.OnItemSpawnRequest -= SpawnItems;
        }

        public void ClearItems()
        {
            for (int i = _spawnedItems.Count - 1; i >= 0; i--)
            {
                if (_spawnedItems[i] != null)
                    Destroy(_spawnedItems[i]);
            }
            _spawnedItems.Clear();
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
                    //if the random value is less than the spawn rate of the current item, spawn it
                    if (
                        _itemsToSpawn[j % _itemsToSpawn.Length].SpawnRate / total + numForAdding
                        >= randomValue
                    )
                    {
                        //create transform to spawn at
                        Vector3 spawnPos = new(
                            Random.Range(-_radius +this.transform.position.x, _radius + this.transform.position.x),
                            0,
                            Random.Range(-_radius + this.transform.position.z, _radius + this.transform.position.z)
                        );

                        //find y position
                        RaycastHit hit;
                        if (Physics.Raycast(spawnPos + Vector3.up * 100, Vector3.down, out hit))
                        {
                            spawnPos.y = hit.point.y;
                        }

                        Quaternion spawnRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

                        _spawnedItems.Add(Instantiate(
                            _itemsToSpawn[j % _itemsToSpawn.Length].Prefab,
                            spawnPos,
                            spawnRotation
                        ));
                        break;
                    }
                    else
                    {
                        numForAdding += _itemsToSpawn[j % _itemsToSpawn.Length].SpawnRate / total;
                    }
                }
            }
        }
    }
}
