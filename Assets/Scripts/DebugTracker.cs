using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTracker : MonoBehaviour
{
    public static DebugTracker Instance {  get; private set; }

    // Public variable to enable and disable Debug.Log() if needed
    public bool DebugOn = true;

    private void Awake()
    {
        gameObject.hideFlags = HideFlags.HideInHierarchy;

        Instance = this;
    }
}
