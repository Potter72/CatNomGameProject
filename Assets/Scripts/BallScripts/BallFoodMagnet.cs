using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMagnet : MonoBehaviour
{
    [SerializeField] Transform ball;
    [SerializeField] float pullForce;

    private void FixedUpdate()
    {
        transform.position = ball.position;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Vector3 magnetForceVector = transform.position - other.transform.position;
            other.GetComponentInParent<Rigidbody>().AddForce(magnetForceVector * pullForce);
        }
    }
}
