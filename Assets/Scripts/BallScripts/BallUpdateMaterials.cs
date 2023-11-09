using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallUpdateMaterials : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    private void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }
    private void FixedUpdate()
    {
        Shader.SetGlobalVector("_PlayerCutoutPos", mainCamera.WorldToViewportPoint(transform.position));
        Shader.SetGlobalVector("_PlayerWorldPos", transform.position);
    }
}