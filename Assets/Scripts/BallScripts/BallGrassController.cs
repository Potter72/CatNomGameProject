using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGrassUpdated : MonoBehaviour
{
    [SerializeField] Material interactableGrassMaterial;

    private void FixedUpdate()
    {
        interactableGrassMaterial.SetVector("_PlayerPosition", transform.position);
    }
}