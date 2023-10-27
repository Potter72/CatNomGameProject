using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FoodDecalLookDown : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float downRayLenght = 0.5f;
    [SerializeField] DecalProjector projector;
    [SerializeField] Transform linkedFood;
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
        RaycastHit hit;
        Physics.Raycast(linkedFood.position, Vector3.down, out hit, downRayLenght, layerMask,QueryTriggerInteraction.Ignore);

        if(hit.collider != null)
        {
            projector.enabled = true;
            transform.position = hit.point + Vector3.up * 0.03f;
        }
        else
        {
            projector.enabled = false;
        }
        
        
    }
}
