using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _support;

    public void OpenSettings()
    {
        _settings.SetActive(true);
        _support.SetActive(false);
    }

    public void OpenSupport()
    {
        _settings.SetActive(false);
        _support.SetActive(true);
    }
}
