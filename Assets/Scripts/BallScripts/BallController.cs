using ProjectCatRoll.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class BallController : MonoBehaviour
{
    private int movementTouchId;

    public bool isMoving;
    [SerializeField] BallVFXController vfxController;
    [SerializeField] Animator playerAnimator;

    [SerializeField] float movementForceMultiplier = 4;
    [SerializeField] float maxHorizontalMoveSpeed = 3;
    [SerializeField] float acceleration = 1;

    [SerializeField] Rigidbody playerRigidbody;

    [SerializeField] float bounceValueToAdd = 0.6f;
    [SerializeField] float bounceDecreaseSpeed = 0.1f;
    [SerializeField] float minBounceValue = 0.1f;
    [SerializeField] float bounceWobble = 0;
    [SerializeField] float fallDamageWobble = 1;
    [SerializeField] AnimationCurve fallDamageWobbleCurve;
    [SerializeField] float fallDamageWobbleTime = 0.2f;
    [SerializeField] float fallDamageCameraChake = 0.3f;

    private float heighestPoint;
    [SerializeField] float fallDamageCutoff = 1;


    [SerializeField] GameObject moveJoystick;
    [SerializeField] float moveJoystickWidth = 13f;

    public Vector3 movementVector;  //is public because used for rotating the player
    [SerializeField] float standStillSpeed = 0.5f;

    [Header("Material References")]
    [SerializeField] Material ballMat;


    [Header("SOUNDS")]
    [SerializeField] AudioClip rollClip;
    private bool isPlayingRollClip;

    public bool isGrounded;
    bool isInCutscene;

    private Vector3 oldPos; //for rotating the player

    private void OnEnable()
    {
        EventManager.OnCutscenePlay += OnCutscenePlayed;
        EventManager.OnCutsceneStop += OnCutsceneStopped;
    }

    private void OnDisable()
    {
        EventManager.OnCutscenePlay -= OnCutscenePlayed;
        EventManager.OnCutsceneStop -= OnCutsceneStopped;
    }

    private void OnCutsceneStopped(object sender, EventArgs e)
    {
        playerRigidbody.isKinematic = false;
        isInCutscene = false;
    }

    private void OnCutscenePlayed(object sender, EventArgs e)
    {
        playerRigidbody.isKinematic = true;
        isInCutscene = true;
    }

    public void StartMovement()
    {
        if(isMoving) { return; }
        //Debug.Log("touching: " + Touchscreen.current.touches.Count);
        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            if (Touchscreen.current.touches[i].isInProgress)
            {
                //Debug.Log("inProgress");
                //movementTouch = Touchscreen.current.touches[i];
                movementTouchId = i;
                isMoving = true;
            }
        }
    }

    public void StopMovement()
    {
        isMoving = false;
        moveJoystick.SetActive(false);
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
        }

    }

    private void FixedUpdate()
    {
        bounceWobble -= bounceDecreaseSpeed * bounceWobble;
        bounceWobble = Mathf.Clamp(bounceWobble, minBounceValue, 1);

        ballMat.SetFloat("_BounceAmount", bounceWobble);

        if (!isGrounded)
        {
            if(heighestPoint < transform.position.y) heighestPoint = transform.position.y;
        }
        else
        {
            heighestPoint = transform.position.y;
        }


        MovementPhysics();

    }

    private void MovementPhysics()
    {
        if (!isMoving || isInCutscene) //movement stuff for when not inputing
        {
            moveJoystick.SetActive(false);
            playerAnimator.SetBool("Running", false);

            Vector3 moveDirection = playerRigidbody.velocity;
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);

            if (moveDirection.magnitude > standStillSpeed)
            {
                if (!isPlayingRollClip)
                {
                    isPlayingRollClip = true;
                    //AudioManager.Instance.PlaySound(rollClip);
                    //Invoke("RollClipLoop", rollClip.length);
                }

                playerAnimator.SetBool("Walking", true);
                moveJoystick.transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.right) * Quaternion.Euler(0, 90, 0);
            }
            else
            {
                playerAnimator.SetBool("Walking", false);
            }
            
            return; 
        }

        playerAnimator.SetBool("Running", true);

        Vector2 currentVelocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.z);

        Vector2 startPos = Touchscreen.current.touches[movementTouchId].startPosition.ReadValue();
        Vector2 currentPos = Touchscreen.current.touches[movementTouchId].position.ReadValue();

        //movement forces
        Vector2 movementReadVector = startPos - currentPos;

        Vector3 forward = Camera.main.transform.forward; 
        forward = new Vector3(forward.x, 0, forward.z).normalized;
        Vector3 right = Camera.main.transform.right;
        right = new Vector3(right.x, 0, right.z).normalized;

        movementVector = forward * -movementReadVector.y + right * -movementReadVector.x;
        movementVector = Vector3.ClampMagnitude(movementVector / 1000 * movementForceMultiplier, maxHorizontalMoveSpeed);
        //movementVector = Camera.main.transform.forward * movementVector;

        if(currentVelocity.magnitude < maxHorizontalMoveSpeed)
        {
            playerRigidbody.AddForce(movementVector * acceleration);
        }

        DoDrawJoystick(startPos, currentPos, movementReadVector);
    }

    private void RollClipLoop()
    {
        isPlayingRollClip = false;
    }

    private void DoDrawJoystick(Vector2 startPos, Vector2 currentPos, Vector2 movementReadVector)
    {
        moveJoystick.SetActive(true);
        moveJoystick.transform.position = Vector3.Lerp(startPos, currentPos, 0.5f);
        //moveJoystick.transform.localScale = new Vector3(moveJoystick.transform.localScale.x, movementReadVector.magnitude, moveJoystick.transform.localScale.z);
        
        if(movementReadVector != Vector2.zero)
        {
            moveJoystick.transform.rotation = Quaternion.LookRotation(movementReadVector,Vector3.right) * Quaternion.Euler(0,90,0);
        }
        moveJoystick.GetComponent<RectTransform>().sizeDelta = new Vector2(movementReadVector.magnitude / 3f, moveJoystickWidth + 1);
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(heighestPoint - transform.position.y > fallDamageCutoff) 
        {
            StartCoroutine(LerpBallWobble(fallDamageWobble * Mathf.Pow((heighestPoint - transform.position.y),0.2f) - 0.3f, fallDamageWobbleTime));
            vfxController.PlayFallDamageEffect();   //plays the fall damage grass vfx
            heighestPoint = transform.position.y;
        }
        isGrounded = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    //adds some wobble to the shader for the ball over time
    IEnumerator LerpBallWobble(float value, float time)
    {
        //Camera.main.GetComponent<CameraController>().trauma += fallDamageCameraChake;
        float oldDecreaseSpeed = bounceDecreaseSpeed;
        bounceDecreaseSpeed = 0;
        float counter = 0;
        float lerpValue = 1 / time;
        while(time > counter)
        {
            //bounceWobble = Mathf.Lerp(bounceWobble, value, counter);
            bounceWobble = fallDamageWobbleCurve.Evaluate(counter * lerpValue);

            yield return 0;
            counter += Time.deltaTime;
        }
        //yield return new WaitForSeconds(1);
        //bounceWobble = value;
        bounceDecreaseSpeed = oldDecreaseSpeed;
    }
}