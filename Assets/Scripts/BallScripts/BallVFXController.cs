using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BallVFXController : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] BallController playerController;
    [SerializeField] VisualEffect rollGrass;
    [SerializeField] float grassSpawnMultiplier = 2;
    [SerializeField] float grassSpawnMax = 1400;

    private void FixedUpdate()
    {
        if (playerTransform) transform.position = playerTransform.position + Vector3.down * playerTransform.localScale.y / 2;
        if (playerController.isGrounded)
        {
            float horizontalVelocity = new Vector2(playerTransform.GetComponent<Rigidbody>().velocity.x, playerTransform.GetComponent<Rigidbody>().velocity.z).magnitude * grassSpawnMultiplier;
            horizontalVelocity = Mathf.Clamp(horizontalVelocity, 0, grassSpawnMax);
            rollGrass.SetFloat("SpawnRate", horizontalVelocity);
        }
        else
        {
            rollGrass.SetFloat("SpawnRate", 0);
        }
    }

    public void PlayFallDamageEffect()
    {
        rollGrass.SendEvent("OnFallDamage");
    }
}
