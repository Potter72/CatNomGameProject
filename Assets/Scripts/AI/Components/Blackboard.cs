using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    private static Blackboard _instance;
    public static Blackboard Instance
    {
        get
        {
            if (!_instance)
            {
                Blackboard[] blackboards = GameObject.FindObjectsOfType<Blackboard>();
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
                DontDestroyOnLoad(CoroutineRunner.instance.gameObject);
            }

            return _instance;
        }

        set => _instance = value as Blackboard;
    }
}
