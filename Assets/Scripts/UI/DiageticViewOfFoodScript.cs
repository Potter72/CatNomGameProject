using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiageticViewOfFoodScript : MonoBehaviour
{
    [SerializeField] private Renderer dCanvas;

    [SerializeField] private GameObject nDCanvas;

    void Update ()
    {
        if (dCanvas.isVisible)
        {
            nDCanvas.SetActive(false);
        }
        else
        {
            nDCanvas.SetActive(true);
        }
    }


}
