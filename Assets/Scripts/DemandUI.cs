using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemandUI : MonoBehaviour
{
    [SerializeField] private List<Image> _imagePrefabs;
    [SerializeField] private Image _image;
    [SerializeField] private Transform _catGod;
    [SerializeField] private List<Vector2> _dimensions;
    [SerializeField] private List<Vector2> _slots;

    private List<Image> _demandImages;
    private List<TextMeshProUGUI> _textBoxes;
    private List<int> _amount;
    
    private float _uiHalfWidth;
    private float _uiHalfHeight;

    private Vector2 _screenSize;
    
    private Camera _camera;

    private bool _outside = false;

    private void Awake()
    {
        _camera = Camera.main;

        _screenSize = _camera.ViewportToScreenPoint(new Vector3(1, 1, 1));
        _demandImages = new List<Image>();
        _textBoxes = new List<TextMeshProUGUI>();
    }
    
    private void FixedUpdate()
    {
        SetBubblePosition();
    }

    private void SetBubblePosition()
    {
        Vector3 screenPoint = _camera.WorldToScreenPoint(_catGod.position);

        if (screenPoint.x < _uiHalfWidth || screenPoint.x > _screenSize.x - _uiHalfWidth || screenPoint.y < _uiHalfHeight || screenPoint.y > _screenSize.y - _uiHalfHeight)
        {
            if (!_outside)
            {
                _outside = true;
                _image.color = Color.grey;
            }
        }
        else
        {
            if (_outside)
            {
                _outside = false;
                _image.color = Color.white;
            }
        }
        
        screenPoint.x = Mathf.Clamp(screenPoint.x, _uiHalfWidth, _screenSize.x - _uiHalfWidth);
        screenPoint.y = Mathf.Clamp(screenPoint.y, _uiHalfHeight, _screenSize.y - _uiHalfHeight);

        _image.rectTransform.position = screenPoint;
    }

    public void SetDemand(List<Item.ItemType> types, List<int> amount)
    {
        SetDimensions(types.Count);
        _amount = amount;
        Debug.Log(_amount[0]);
        
        for (int i = 0; i < types.Count; i++)
        {
            Image newDemand = GetImage(types[i]);
            TextMeshProUGUI newText = newDemand.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            
            newDemand.rectTransform.localPosition = _slots[i];
            
            newText.text = $"x{amount[i]}";
            _demandImages.Add(newDemand);
            _textBoxes.Add(newText);
        }
    }

    public void ReduceByOne(int index)
    {
        _textBoxes[index].text = $"x{_amount[index]}";
    }

    public void RemoveType(int index)
    {
        Destroy(_demandImages[index].gameObject);
        _demandImages.RemoveAt(index);
        _textBoxes.RemoveAt(index);
    }

    private Image GetImage(Item.ItemType type)
    {
        Image i;
        
        switch (type)
        {
            case Item.ItemType.Carrot:
                i = Instantiate(_imagePrefabs[0], transform);
                break;
            case Item.ItemType.Ham:
                i = Instantiate(_imagePrefabs[1], transform);
                break;
            case Item.ItemType.RedShroom:
                i = Instantiate(_imagePrefabs[2], transform);
                break;
            case Item.ItemType.YellowShroom:
                i = Instantiate(_imagePrefabs[3], transform);
                break;
            case Item.ItemType.Lettuce:
                i = Instantiate(_imagePrefabs[4], transform);
                break;
            default:
                i = Instantiate(_imagePrefabs[0], transform);
                break;
        }

        return i;
    }
    
    private Color GetColor(Item.ItemType type)
    {
        Color c = Color.white;

        switch (type)
        {
            case Item.ItemType.YellowShroom:
                c = Color.yellow;
                break;
            case Item.ItemType.Carrot:
                c = new Color(1f, 0.5f, 0f);
                break;
            case Item.ItemType.Ham:
                c = Color.magenta;
                break;
            case Item.ItemType.RedShroom:
                c = Color.red;
                break;
            case Item.ItemType.Lettuce:
                c = Color.green;
                break;
        }

        return c;
    }

    private void SetDimensions(int count)
    {
        switch (count)
        {
            // case 1:
            //     _image.rectTransform.sizeDelta = _dimensions[0];
            //     break;
            // case 2:
            //     _image.rectTransform.sizeDelta = _dimensions[1];
            //     break;
            // case 3:
            //     _image.rectTransform.sizeDelta = _dimensions[2];
            //     break;
            // case 4:
            //     _image.rectTransform.sizeDelta = _dimensions[2];
            //     break;
            // case 5:
            //     _image.rectTransform.sizeDelta = _dimensions[3];
            //     break;
            default:
                _image.rectTransform.sizeDelta = _dimensions[3];
                break;
        }

        _uiHalfWidth = _image.rectTransform.sizeDelta.x / 2;
        _uiHalfHeight = _image.rectTransform.sizeDelta.y / 2;
    }
}
