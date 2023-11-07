using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontChanger2 : MonoBehaviour
{
    [SerializeField] private List<GameObject> normalTexts;

    [SerializeField] private List<GameObject> dyslexicTexts;
    [SerializeField] private bool isOn = false;

    void Update ()
    {
        if (!isOn)
        {
            ChangeToNormal();
        }
        else if (isOn)
        {
            ChangeToDyslexic();
        }
    }

    public void ChangeFontLol ()
    {
        isOn = !isOn;
    }

    public void ChangeToNormal ()
    {
        foreach (var normObj in normalTexts)
        {
            normObj.SetActive(true);
        }
        foreach (var dysmObj in dyslexicTexts)
        {
            dysmObj.SetActive(false);
        }
    }

    public void ChangeToDyslexic ()
    {
        foreach (var dysObj in dyslexicTexts)
        {
            dysObj.SetActive(true);
        }
        foreach (var norObj in normalTexts)
        {
            norObj.SetActive(false);
        }
    }
}
