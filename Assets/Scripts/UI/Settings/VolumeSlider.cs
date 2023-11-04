using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Image _handleIcon;
    [SerializeField] private Sprite _sliderOn;
    [SerializeField] private Sprite _sliderOff;
    
    [Space]
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _onColor;
    [SerializeField] private Color _offColor;
    
    [Space]
    [SerializeField] private Image _volumeIcon;
    [SerializeField] private Sprite _buttonOn;
    [SerializeField] private Sprite _buttonOff;
    
    private Slider _slider;
    private float _lastValue;
    private bool _muted = false;
    
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _lastValue = _slider.value;
    }

    public void CheckValue()
    {
        if (_slider.value <= 0f)
        {
            Mute();
        }
        
        else if (_slider.value > 0f)
        {
            if (_muted)
            {
                Unmute();
            }

            _lastValue = _slider.value;
        }
    }
    
    public void VolumeButtonPress()
    {
        if (_muted)
        {
            ButtonUnmute();
        }
        else if (!_muted)
        {
            Mute();
        }
    } 
    
    private void Mute()
    {
        _slider.value = 0f;
        _handleIcon.sprite = _sliderOff;
        _text.color = _offColor;
        _volumeIcon.sprite = _buttonOff;
        _muted = true;
    }

    private void Unmute()
    {
        _handleIcon.sprite = _sliderOn;
        _text.color = _onColor;
        _volumeIcon.sprite = _buttonOn;
        _muted = false;
    }

    private void ButtonUnmute()
    {
        _slider.value = _lastValue;
        Unmute();
    }
}
