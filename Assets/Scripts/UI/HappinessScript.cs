using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappinessScript : MonoBehaviour
{
    [SerializeField] private Image originalImage;
    [SerializeField] private Sprite happyFace;
    [SerializeField] private Sprite mediumFace;
    [SerializeField] private Sprite sadFace;
    [SerializeField] private float startingTime;

    private float currentTime;

    void Start ()
    {
        currentTime = startingTime;
        originalImage.sprite = happyFace;
    }

    void Update ()
    {
        currentTime -= 1 * Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            Debug.Log("lol");
        }
    }

}
