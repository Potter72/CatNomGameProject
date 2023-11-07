using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Mood : MonoBehaviour
{
    [SerializeField] private Transform _bubblePosition;
    [SerializeField] private Image _dynamicMoodUI;
    [SerializeField] private Image _staticMoodUI2;
    [SerializeField] private Image _mood;
    
    private Camera _camera;

    private Vector2 _screenSize;
    private float _uiHalfWidth;
    private float _uiHalfHeight;

    private bool _outside = false;
    
    private void Awake()
    {
        _camera = Camera.main;
        
        _screenSize = _camera.ViewportToScreenPoint(new Vector3(1, 1, 1));
        _uiHalfWidth = _dynamicMoodUI.rectTransform.sizeDelta.x / 2;
        _uiHalfHeight = _staticMoodUI2.rectTransform.sizeDelta.y / 2;
        
        SetToDynamicUI();
    }

    private void Update()
    {
        SetBubblePosition();
    }
    
    private void SetBubblePosition()
    {
        Vector3 screenPoint = _camera.WorldToScreenPoint(_bubblePosition.position);

        if (screenPoint.x < -_uiHalfWidth || screenPoint.x > _screenSize.x + _uiHalfWidth || screenPoint.y < -_uiHalfHeight || screenPoint.y > _screenSize.y + _uiHalfHeight)
        {
            if (!_outside)
            {
                _outside = true;
                SetToStaticUI();
            }
        }
        else
        {
            if (_outside)
            {
                _outside = false;
                SetToDynamicUI();
            }
        }

        _dynamicMoodUI.rectTransform.position = screenPoint;
    }

    private void SetToDynamicUI()
    {
        _mood.rectTransform.SetParent(_dynamicMoodUI.transform);
        _mood.rectTransform.localPosition = Vector3.zero;
        
        _dynamicMoodUI.color = new Color(1, 1, 1, 1);
        _staticMoodUI2.color = new Color(1, 1, 1, 0);
    }
    
    private void SetToStaticUI()
    {
        _mood.rectTransform.SetParent(_staticMoodUI2.transform);
        _mood.rectTransform.localPosition = Vector3.zero;
        
        _dynamicMoodUI.color = new Color(1, 1, 1, 0);
        _staticMoodUI2.color = new Color(1, 1, 1, 1);
    }
}
