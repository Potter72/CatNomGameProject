using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomItemSpawner : MonoBehaviour
{
    [Serializable]
    public class SpawnAreaList
    {
        public List<SpawnArea> SpawnAreaPoints;
    }
    
    [Serializable]
    public class SpawnArea
    {
        public Transform Point;
        public float Radius;
    }

    [SerializeField] private List<SpawnAreaList> _spawnAreas;
    [SerializeField] private int _group;
    [SerializeField] private Color _gizmoColor;

    [SerializeField] private Transform _parent;
    [SerializeField] private List<GameObject> _itemPrefabs;
    [SerializeField] private List<int> _itemVarietyLimit;
    
    [SerializeField] private float _startDelay;
    [SerializeField] private float _spawnDelay;

    [SerializeField] private int _droughtLimit = 5;

    private List<GameObject> _demandedPrefabs;
    private List<Item.ItemType> _demandedTypes;
    private int _demandDrought;
    
    private int _level = 0;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        
        foreach (SpawnArea s in _spawnAreas[_group].SpawnAreaPoints)
        {
            Gizmos.DrawSphere(s.Point.position, s.Radius);
        }
    }

    private void Awake()
    {
        _demandedPrefabs = new List<GameObject>();
    }

    public void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnItem), _startDelay, _spawnDelay);
    }
    
    private void SpawnItem()
    {
        GameObject itemPrefab;
    
        itemPrefab = _demandDrought >= _droughtLimit ? _demandedPrefabs[Random.Range(0, _demandedPrefabs.Count)] : 
                                                        _itemPrefabs[Random.Range(0, _itemVarietyLimit[_level])];
        
        Item itemComponent = itemPrefab.GetComponent<Item>();
        CheckIfDemanded(itemComponent.FoodType);
        
        SpawnArea sa = _spawnAreas[_level].SpawnAreaPoints[Random.Range(0, _spawnAreas[_level].SpawnAreaPoints.Count)];
    
        Vector3 point = sa.Point.position;
        Vector2 random = new Vector2(point.x, point.z);
        random += Random.insideUnitCircle * sa.Radius;
        point.x = random.x;
        point.z = random.y;
    
        GameObject newItem = Instantiate(itemPrefab, _parent);
        newItem.transform.position = point;
    }

    // Provides the list of items provided by the god to
    // check which items to spawn in case of drought
    public void SetDemands(List<Item.ItemType> demandedTypes)
    {
        _demandedPrefabs.Clear();
        
        _demandedTypes = new List<Item.ItemType>(demandedTypes);

        int demandsCopied = 0;
        
        foreach (GameObject go in _itemPrefabs)
        {
            if (demandsCopied == _demandedTypes.Count) break;
            
            Item.ItemType currentItem = go.GetComponent<Item>().FoodType;
            
            foreach (Item.ItemType t in _demandedTypes)
            {
                if (t != currentItem) continue;
                
                _demandedPrefabs.Add(go);
                demandsCopied++;
                break;
            }
        }
    }
    
    private void CheckIfDemanded(Item.ItemType itemType)
    {
        foreach (Item.ItemType t in _demandedTypes)
        {
            if (t == itemType)
            {
                _demandDrought = 0;

                return;
            }
        }

        _demandDrought++;
        return;
    }

    public void IncreaseLevel()
    {
        _level++;
    }
}
