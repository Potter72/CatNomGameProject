using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRendererPlace : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    private void FixedUpdate()
    {
        transform.position = playerTransform.position;
        transform.rotation = playerTransform.rotation;
    }
}
