using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomItemSpawner : MonoBehaviour
{
    [Serializable]
    public class SpawnArea
    {
        public Transform Point;
        public float Radius;
    }

    [SerializeField] private List<SpawnArea> _spawnAreas;
    [SerializeField] private Color _gizmoColor;

    [SerializeField] private Transform _parent;
    [SerializeField] private List<GameObject> _itemPrefabs;
    
    [SerializeField] private float _startDelay;
    [SerializeField] private float _spawnDelay;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        
        foreach (SpawnArea s in _spawnAreas)
        {
            Gizmos.DrawSphere(s.Point.position, s.Radius);
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnItem), _startDelay, _spawnDelay);
    }
    
    private void Update()
    {
        
    }

    private void SpawnItem()
    {
        GameObject item = _itemPrefabs[Random.Range(0, _itemPrefabs.Count)];
        
        SpawnArea sa = _spawnAreas[Random.Range(0, _spawnAreas.Count)];

        Vector3 point = sa.Point.position;
        Vector2 random = new Vector2(point.x, point.z);
        random += Random.insideUnitCircle * sa.Radius;
        point.x = random.x;
        point.z = random.y;

        GameObject newItem = Instantiate(item, _parent);
        newItem.transform.position = point;
    }
}
