using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    private int movementTouchId;
    private int aimingTouchId;

    private Touch movementTouch;
    private Touch aimingtTouch;


    private Vector2 movingStartPos;
    private bool isMoving;
    private Vector2 aimingStartPos;
    private bool isAiming;

    [SerializeField] float movementForce = 1;
    [SerializeField] float movementForceClamp = 3;

    [SerializeField] Rigidbody playerRigidbody;

    public void StartMovement()
    {
        Debug.Log("starting");
        Debug.Log("touches" + Touchscreen.current.touches);
        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {

            if (Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Debug.Log("succes");
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

    public void StartAiming()
    {
        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            if (Touchscreen.current.touches[i].phase.valueSizeInBytes == 1)
            {
                //movementTouch = Touchscreen.current.touches[i];
                aimingTouchId = i;
            }
        }
    }

    public void StopAiming()
    {

    }

    private void FixedUpdate()
    {
        MovementPhysics();
    }

    private void MovementPhysics()
    {
        if (!isMoving) { return; }

        //movement forces
        Vector2 movementReadVector = Touchscreen.current.touches[movementTouchId].startPosition.ReadValue() - Touchscreen.current.touches[movementTouchId].position.ReadValue();

        Vector3 movementVector = new Vector3(movementReadVector.x, 0, movementReadVector.y);
        movementVector = Vector3.ClampMagnitude(movementVector, movementForceClamp);
        //movementVector = Camera.main.transform.forward * movementVector;
        playerRigidbody.AddForce(movementVector * movementForce);
    }
}