using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Changeimage : MonoBehaviour
{
    
    [Header("Food Items to Cycle Through")]
    public List<Sprite> foodSprite = new List<Sprite>();

    public Image foodImage;

    private int currentfood = 0;

    public void FoodSpriteRandomize ()
    {
        currentfood = Random.Range(0, foodSprite.Count);

        foodImage.sprite = foodSprite[currentfood];
    }
}
