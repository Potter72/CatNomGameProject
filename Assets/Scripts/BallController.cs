using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    private int movementTouchId;

    private bool isMoving;

    [SerializeField] float movementForce = 1;
    [SerializeField] float movementForceClamp = 3;

    [SerializeField] Rigidbody playerRigidbody;

    [SerializeField] float bounceValueToAdd = 0.6f;
    [SerializeField] float bounceDecreaseSpeed = 0.1f;
    [SerializeField] float minBounceValue = 0.1f;
    [SerializeField] float bounceWobble = 0;
    [SerializeField] float fallDamageWobble = 1;

    private float startHeight;
    [SerializeField] float fallDamageCutoff = 1;
    [SerializeField] Material ballMat;

    public void StartMovement()
    {
        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                //movementTouch = Touchscreen.current.touches[i];
                movementTouchId = i;
                isMoving = true;
            }
        }
    }

    public void StopMovement()
    {
        isMoving = false;
        movementTouchId = 1000;
    }

    public void StopAiming()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bounceWobble += bounceValueToAdd;
            Debug.Log("bruh?");
        }
    }

    private void FixedUpdate()
    {
        bounceWobble -= bounceDecreaseSpeed * bounceWobble;
        bounceWobble = Mathf.Clamp(bounceWobble, minBounceValue, 1);

        ballMat.SetFloat("_BounceAmount", bounceWobble);

        MovementPhysics();
    }

    private void MovementPhysics()
    {
        if (!isMoving) { return; }

        //movement forces
        Vector2 movementReadVector = Touchscreen.current.touches[movementTouchId].startPosition.ReadValue() - Touchscreen.current.touches[movementTouchId].position.ReadValue();

        Vector3 forward = Camera.main.transform.forward;
        forward = new Vector3(forward.x, 0, forward.z).normalized;
        Vector3 right = Camera.main.transform.right;
        right = new Vector3(right.x, 0, right.z).normalized;

        Vector3 movementVector = forward * -movementReadVector.y + right * -movementReadVector.x;
        movementVector = Vector3.ClampMagnitude(movementVector, movementForceClamp);
        //movementVector = Camera.main.transform.forward * movementVector;
        playerRigidbody.AddForce(movementVector * movementForce);

        Debug.DrawRay(movementVector, transform.position,Color.red,2);
    }

    private void OnCollisionExit(Collision collision)
    {
        startHeight = transform.position.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(startHeight - transform.position.y > fallDamageCutoff) 
        {
            bounceWobble += (startHeight - transform.position.y) * fallDamageWobble;
            startHeight = transform.position.y;
        }
    }
}