using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameObject _item;
    [SerializeField] private int _spawnAmount = 10;
    [SerializeField] private float _sendDelay = 0.3f;

    [SerializeField] private List<Item> _items = new List<Item>();
    [SerializeField] private Plate _plate;

    private void Awake()
    {
        // Remove when the rest of the game is done
        for(int i = 0; i < _spawnAmount; i++)
        {
            GameObject newItem = Instantiate(_item);
            Vector3 itemPos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 0.5f;
            newItem.transform.position = itemPos + transform.position;
            _items.Add(newItem.GetComponent<Item>());
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SendItemsToPlate();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            // Clears editor console on reset
            Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.LogEntries").GetMethod("Clear").Invoke(new object(), null);
            SceneManager.LoadScene(0);
        }
    }

    // Used for when the ball needs to add an item that's stuck
    private void AddItem(Item item)
    {
        _items.Add(item);
    }

    // Use this when you want to send the food stuck on the ball
    private void SendItemsToPlate()
    {
        StartCoroutine(SendSpacedOut());
    }

    // Sends the food at an interval
    IEnumerator SendSpacedOut()
    {
        while(_items.Count > 1)
        {
            _items[0].SendItem(_plate, this);
            _items.RemoveAt(0);
            yield return new WaitForSeconds(_sendDelay);
        }

        _items[0].SendItem(_plate, this);
        _items[0].SetLastItem();
        _items.RemoveAt(0);
    }
}
