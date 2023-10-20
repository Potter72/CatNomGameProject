using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BallVFXController : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] VisualEffect rollGrass;

    private void FixedUpdate()
    {
        if(playerTransform) transform.position = playerTransform.position - Vector3.down * playerTransform.localScale.y / 2;

    }
}
