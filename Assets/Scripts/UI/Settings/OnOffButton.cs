using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class OnOffButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _onOffText;

    [SerializeField] private TextMeshProUGUI _dyslexicOnOffText;
    [SerializeField] private Color _onColor;
    [SerializeField] private Color _offColor;

    [Space] 
    [SerializeField] private Sprite _buttonOn;
    [SerializeField] private Sprite _buttonOff;
    private Image _onOffButton;

    [SerializeField] private bool _off = false;
    
    private void Awake()
    {
        _onOffButton = GetComponent<Image>();
        ChangeStatus();
    }

    public void ChangeStatus()
    {
        if (_off)
        {
            TurnOn();
        }

        else if (!_off)
        {
            TurnOff();
        }
    }

    private void TurnOn()
    {
        _text.color = _onColor;
        _onOffButton.sprite = _buttonOn;
        _onOffText.text = "On";
        _onOffText.color = _onColor;
        _dyslexicOnOffText.text = "On";
        _dyslexicOnOffText.color = _onColor;
        _off = false;
    }

    private void TurnOff()
    {
        _text.color = _offColor;
        _onOffButton.sprite = _buttonOff;
        _onOffText.text = "Off";
        _onOffText.color = _offColor;
        _dyslexicOnOffText.text = "Off";
        _dyslexicOnOffText.color = _offColor;
        _off = true;
    }
}
