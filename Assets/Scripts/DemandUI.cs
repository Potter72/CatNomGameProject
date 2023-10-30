using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemandUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Transform _catGod;
    [SerializeField] private Camera _camera;

    private bool _outside = false;
    
    private void FixedUpdate()
    {
        SetBubblePosition();
    }

    private void SetBubblePosition()
    {
        Vector3 viewportPos = _camera.WorldToViewportPoint(_catGod.position);

        if (viewportPos.x < 0.05f || viewportPos.x > 0.95f || viewportPos.y < 0.1f || viewportPos.y > 0.9f)
        {
            if (!_outside)
            {
                _outside = true;
                _image.color = Color.red;
            }
        }
        else
        {
            if (_outside)
            {
                _outside = false;
                _image.color = Color.blue;
            }
        }
        
        viewportPos.x = Mathf.Clamp(viewportPos.x, 0.05f, 0.95f);
        viewportPos.y = Mathf.Clamp(viewportPos.y, 0.1f, 0.9f);

        _image.rectTransform.position = _camera.ViewportToScreenPoint(viewportPos);
    }

    public void SetDemand(List<Item.ItemType> types, List<int> amount)
    {
        for (int i = 0; i < types.Count; i++)
        {
            
        }
    }

    private Color GetColor(Item.ItemType type)
    {
        Color c = Color.white;

        switch (type)
        {
            case Item.ItemType.Corn:
                c = Color.yellow;
                break;
            case Item.ItemType.Fish:
                c = Color.blue;
                break;
            case Item.ItemType.Ham:
                c = Color.magenta;
                break;
            case Item.ItemType.Potato:
                c = Color.red;
                break;
        }

        return c;
    }
}
