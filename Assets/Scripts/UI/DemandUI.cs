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
    [SerializeField] private Image _completeImage;
    [SerializeField] private Transform _catGod;
    [SerializeField] private List<Vector2> _dimensions;
    [SerializeField] private List<Vector2> _slots;
    [SerializeField] private List<Image> _staticSlots;

    private List<Image> _demandImages;
    private List<TextMeshProUGUI> _textBoxes;
    private List<Animator> _animators;
    private List<int> _amount;
    
    private float _uiHalfWidth;
    private float _uiHalfHeight;

    private Vector2 _screenSize;
    
    private Camera _camera;

    private int _currentCount;
    private bool _outside = false;

    private void Awake()
    {
        _camera = Camera.main;

        _screenSize = _camera.ViewportToScreenPoint(new Vector3(1, 1, 1));
        _demandImages = new List<Image>();
        _textBoxes = new List<TextMeshProUGUI>();
        _animators = new List<Animator>();
    }
    
    private void FixedUpdate()
    {
        SetBubblePosition();
    }

    public void SetDemand(List<Item.ItemType> types, List<int> amount)
    {
        SetDimensions(types.Count);
        _amount = amount;
        
        for (int i = 0; i < types.Count; i++)
        {
            Image newDemand = GetImage(types[i]);
            TextMeshProUGUI newText = newDemand.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            newText.text = $"x{amount[i]}";
            _demandImages.Add(newDemand);
            _textBoxes.Add(newText);
            _animators.Add(newText.GetComponent<Animator>());
        }
        
        if (!_outside)
        {
            SetToDynamicUI();
        }
            
        else if (_outside)
        {
            SetToStaticUI();
        }
    }
    
    public void ReduceByOne(int index)
    {
        _textBoxes[index].text = $"x{_amount[index]}";
        _animators[index].SetTrigger("Bounce");
    }

    public void SetTypeComplete(int index)
    {
        Instantiate(_completeImage, _demandImages[index].transform, false);
    }
    
    public void RemoveAllTypes()
    {
        for (int i = 0; i < _demandImages.Count; i++)
        {
            Destroy(_demandImages[i].gameObject);
        }
        
        _demandImages.Clear();
        _textBoxes.Clear();
        _animators.Clear();
    }
    
    private Image GetImage(Item.ItemType type)
    {
        Image i;
        Debug.Log(type);
        switch (type)
        {
            case Item.ItemType.Mushroom:
                i = Instantiate(_imagePrefabs[0], transform);
                break;
            case Item.ItemType.Turkey:
                i = Instantiate(_imagePrefabs[1], transform);
                break;
            case Item.ItemType.Pie:
                i = Instantiate(_imagePrefabs[2], transform);
                break;
            case Item.ItemType.Squash:
                i = Instantiate(_imagePrefabs[3], transform);
                break;
            case Item.ItemType.MoonCake:
                i = Instantiate(_imagePrefabs[4], transform);
                break;
            case Item.ItemType.Cupcake:
                i = Instantiate(_imagePrefabs[5], transform);
                break;
            case Item.ItemType.Apple:
                i = Instantiate(_imagePrefabs[6], transform);
                break;
            case Item.ItemType.Potato:
                i = Instantiate(_imagePrefabs[7], transform);
                break;
            case Item.ItemType.Sandwich:
                i = Instantiate(_imagePrefabs[8], transform);
                break;
            case Item.ItemType.Ham:
                i = Instantiate(_imagePrefabs[9], transform);
                break;
            default:
                i = Instantiate(_imagePrefabs[0], transform);
                break;
        }

        return i;
    }
    
    // private Color GetColor(Item.ItemType type)
    // {
    //     Color c = Color.white;
    //
    //     switch (type)
    //     {
    //         case Item.ItemType.YellowShroom:
    //             c = Color.yellow;
    //             break;
    //         case Item.ItemType.Carrot:
    //             c = new Color(1f, 0.5f, 0f);
    //             break;
    //         case Item.ItemType.Ham:
    //             c = Color.magenta;
    //             break;
    //         case Item.ItemType.RedShroom:
    //             c = Color.red;
    //             break;
    //         case Item.ItemType.Lettuce:
    //             c = Color.green;
    //             break;
    //     }
    //
    //     return c;
    // }

    private void SetDimensions(int count)
    {
        _currentCount = count;
        
        switch (count)
        {
            case 1:
                _image.rectTransform.sizeDelta = _dimensions[0];
                break;
            case 3:
                _image.rectTransform.sizeDelta = _dimensions[1];
                break;
            default:
                _image.rectTransform.sizeDelta = _dimensions[0];
                break;
        }

        _uiHalfWidth = _image.rectTransform.sizeDelta.x / 2;
        _uiHalfHeight = _image.rectTransform.sizeDelta.y / 2;
    }
    
    private void SetBubblePosition()
    {
        Vector3 screenPoint = _camera.WorldToScreenPoint(_catGod.position);

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
        
        // screenPoint.x = Mathf.Clamp(screenPoint.x, _uiHalfWidth, _screenSize.x - _uiHalfWidth);
        // screenPoint.y = Mathf.Clamp(screenPoint.y, _uiHalfHeight, _screenSize.y - _uiHalfHeight);

        _image.rectTransform.position = screenPoint;
    }

    private void SetToDynamicUI()
    {
        _image.color = new Color(1, 1, 1, 1);
        
        for (int i = 0; i < _staticSlots.Count; i++)
        {
            _staticSlots[i].color = new Color(1, 1, 1, 0);
        }
        
        for (int i = 0; i < _demandImages.Count; i++)
        {
            _demandImages[i].rectTransform.SetParent(transform);
            _demandImages[i].rectTransform.localPosition = _slots[i];
        }
    }

    private void SetToStaticUI()
    {
        _image.color = new Color(1, 1, 1, 0);
        
        if (_currentCount == 1)
        {
            _staticSlots[0].color = new Color(1, 1, 1, 1);
            _demandImages[0].rectTransform.position = _staticSlots[0].rectTransform.position;
            _demandImages[0].rectTransform.SetParent(_staticSlots[0].rectTransform);
            return;
        }

        for (int i = 0; i < _staticSlots.Count; i++)
        {
            _staticSlots[i].color = new Color(1, 1, 1, 1);
            _demandImages[i].rectTransform.position = _staticSlots[i].rectTransform.position;
            _demandImages[i].rectTransform.SetParent(_staticSlots[i].rectTransform);
        }
    }
}
