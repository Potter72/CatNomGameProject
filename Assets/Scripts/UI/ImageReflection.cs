using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageReflection : MonoBehaviour
{
    [SerializeField] private Image changingImage;

    [SerializeField] private Image originalImage;


    void Update ()
    {
        changingImage.sprite = originalImage.sprite;
    }
}
