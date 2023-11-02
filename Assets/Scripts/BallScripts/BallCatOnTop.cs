using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallCatOnTop : MonoBehaviour
{
    [SerializeField] Transform catTransform;
    [SerializeField] BallPickupFood ballPickupFood;
    [SerializeField] BallController ballController;
    [SerializeField] float displaceAdjustment = 0.3f;
    [SerializeField] float displaceAdjustment2 = 0.3f;

    [SerializeField] float baseSineAmplitude = 0.005f;
    [SerializeField] float maxSineAmplitude = 0.02f;
    private float sineAmplitude = 0.5f;
    [SerializeField] float sineSpeed = 2;

    [SerializeField] Rigidbody playerRb;
    private void Start()
    {
        if (playerRb == null) { playerRb = GetComponent<Rigidbody>(); }
    }

    private void FixedUpdate()
    {
        Vector3 startPos = transform.position + Vector3.up * transform.localScale.y / 2;
        float verticalDistance = (startPos.y - (transform.position.y - transform.localScale.y / 2)) / displaceAdjustment;  //gets the vertical distance of the food compared to the player

        //some noise for when moving
        float sine1 = Mathf.Sin(Time.time * sineSpeed * 0.5f) * 1;
        float sine2 = Mathf.Sin(Time.time * sineSpeed * 1f) * 0.5f;
        float sine3 = Mathf.Sin(Time.time * sineSpeed * 0.25f) * 0.75f;

        sineAmplitude = Mathf.Clamp((baseSineAmplitude * playerRb.velocity.magnitude), 0, maxSineAmplitude);
        float finalSine = (sine1 + sine2 + sine3) * sineAmplitude;
        
        if (ballController.movementVector != Vector3.zero)
        {        
            //rotating the player
            catTransform.rotation = Quaternion.LookRotation(ballController.movementVector);
        }

        catTransform.position = startPos + Vector3.down * verticalDistance * Mathf.Pow(ballPickupFood.ballDisplacement,displaceAdjustment2);
    }
}
