using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootPiston : MonoBehaviour
{
    [SerializeField] float shootHeight = 3;
    [SerializeField] float shootSpeed = 1;
    [SerializeField] float shootWaitTime = 0.05f;
    [SerializeField] Rigidbody pistonRb;
    [SerializeField] float resetSpeed = 2;
    [SerializeField] float resetWaitTime = 1;
    [SerializeField] float shootCooldown = 6;
    private void Start()
    {
        StartCoroutine(ShootPistonCooldown());
    }

    IEnumerator ShootPistonCooldown()
    {
        while (true) 
        {
            yield return new WaitForSeconds(shootCooldown);
            StartCoroutine(ShootPistonCo());
        }
    }
    IEnumerator ShootPistonCo()
    {
        Vector3 startPos = transform.position;
        while((startPos - transform.position).magnitude < shootHeight)
        {
            //transform.position = Vector3.Lerp(startPos, transform.position + transform.up, counter);
            pistonRb.MovePosition(transform.position + transform.up * shootSpeed);
            yield return new WaitForSeconds(shootWaitTime);
        }
        StartCoroutine(ResetPiston(startPos));
    }

    IEnumerator ResetPiston(Vector3 originalPos)
    {
        yield return new WaitForSeconds(resetWaitTime);
        float counter = 0;
        Vector3 startPos = transform.position;

        while (counter < 1) 
        {
            for (int i = 0; i < 1; i++)
            {
                Debug.Log("weoah");
                transform.position = Vector3.Lerp(startPos, originalPos, counter);
                counter += Time.deltaTime * resetSpeed;
                yield return 0;
            }
        }
        transform.position = originalPos;
    }
}
