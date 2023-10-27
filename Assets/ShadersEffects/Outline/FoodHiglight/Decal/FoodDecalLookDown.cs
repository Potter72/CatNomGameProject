using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDecalLookDown : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
