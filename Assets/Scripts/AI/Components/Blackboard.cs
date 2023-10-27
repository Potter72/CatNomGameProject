using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    public Ball _player;
    public Transform _playerTransform;
    public ItemList _itemList;
    
    private static Blackboard _instance;

    public static Blackboard Instance
    {
        get
        {
            if (!_instance)
            {
                Blackboard[] blackboards = GameObject.FindObjectsByType<Blackboard>(FindObjectsSortMode.None);
                if (blackboards != null)
                {
                    if (blackboards.Length == 1)
                    {
                        _instance = blackboards[0];
                        return _instance;
                    }
                }

                GameObject go = new GameObject("Blackboard", typeof(Blackboard));
                _instance = go.GetComponent<Blackboard>();
                //DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }

        set => _instance = value as Blackboard;
    }
    
    void Start()
    {
        _player = GameManager.Instance.GetPlayer();
        _playerTransform = _player.transform;
        _itemList = GameManager.Instance.GetItemList();
    }
    
    void Update()
    {
        
    }
}
