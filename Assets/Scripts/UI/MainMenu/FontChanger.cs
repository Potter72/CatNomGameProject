using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FontChanger : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset _dyslexiaFont;
    
    private TMP_FontAsset[] _fonts;
    private TextMeshProUGUI[] _text;

    private bool _dyslexic = false;
    
    private void Awake()
    {
        StoreFonts();
    }

    private void StoreFonts()
    {        
        _text =
            GameObject.FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        _fonts = new TMP_FontAsset[_text.Length];
        
        for (int i = 0; i < _text.Length; i++)
        {
            _fonts[i] = _text[i].font;
        }
    }

    public void ChangeFont()
    {
        if (_dyslexic)
        {
            ChangeToRegularFont();
        }
        
        else if (!_dyslexic)
        {
            ChangeToDyslexiaFont();
        }
    }
    
    public void ChangeToDyslexiaFont()
    {
        for (int i = 0; i < _text.Length; i++)
        {
            _text[i].font = _dyslexiaFont;
        }

        _dyslexic = true;
    }

    public void ChangeToRegularFont()
    {
        for (int i = 0; i < _text.Length; i++)
        {
            _text[i].font = _fonts[i];
        }

        _dyslexic = false;
    }
}
