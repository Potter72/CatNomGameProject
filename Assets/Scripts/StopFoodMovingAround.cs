using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFoodMovingAround : MonoBehaviour
{
    [SerializeField] Rigidbody foodRB;
    [SerializeField] Transform itemContainerParent;

    [SerializeField] bool isGrounded = false;

    private void Awake()
    {
        itemContainerParent = transform.parent;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(transform.parent == itemContainerParent) 
        {
            isGrounded = true;
            foodRB.isKinematic = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (transform.parent == itemContainerParent)
        {
            isGrounded = false;
        }
    }
}
