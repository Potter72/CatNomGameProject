using System.Collections.Generic;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
public class SettingsScript : MonoBehaviour, IDataPersistenceSettingsData
{
    public AudioMixer audioMixer;

    public TMPro.TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    bool _isFullscreen;

    void Start()
    {

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        AudioManager.Instance.ChangeMasterVolume(volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        _isFullscreen = isFullscreen;
    }

    public void SaveSettings(SettingsData data)
    {
        data.MasterVolume = AudioManager.Instance.MasterVolume;
        data.Fullscreen = _isFullscreen;
    }
    public void LoadSettings(SettingsData data)
    {
        AudioManager.Instance.ChangeMasterVolume(data.MasterVolume);
    }
}
