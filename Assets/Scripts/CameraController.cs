using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    public float smoothSpeed;
    public GameObject cameraPivot;
    private float distanceToPlayer;

    public bool canRotateCamera = true;

    //camera shake
    public float shakeDecreaseSpeed = 1;
    public float shakeSpeed = 14;
    public float trauma;        //trauma is the current camera shake value and continously lowered
    public float maxOffset = 1000;
    public float maxAngle = 9000;

    private float offsetX;
    private float offsetY;
    private float offsetZ;
    private float offsetAngle;
    private Vector3 shakeOffset;

    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";

    private void Start()
    {
        //offset = transform.localPosition;

    }

    private void Update()
    {
        //Lowers trauma over time
        trauma = Mathf.Clamp((trauma - 1f * Time.deltaTime * shakeDecreaseSpeed), 0, 1);

        //Generates the camera shake offsets if trauma is above 0
        if (trauma > 0)
        {
            offsetAngle = maxAngle * trauma * (Mathf.PerlinNoise(Random.Range(0, 10), Time.time * shakeSpeed) * 2 - 1);
            offsetX = maxOffset * trauma * (Mathf.PerlinNoise(Random.Range(0, 10), Time.time * shakeSpeed) * 2 - 1);
            offsetY = maxOffset * trauma * (Mathf.PerlinNoise(Random.Range(0, 10), Time.time * shakeSpeed) * 2 - 1);
            offsetZ = maxOffset * trauma * (Mathf.PerlinNoise(Random.Range(0, 10), Time.time * shakeSpeed) * 2 - 1);
        }
        else
        {
            offsetAngle = 0;
            offsetX = 0;
            offsetY = 0;
            offsetZ = 0;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            trauma += 1;
        }
    }

    void FixedUpdate()
    {
        distanceToPlayer = (cameraPivot.transform.position - target.transform.position).magnitude;
        cameraPivot.transform.position = Vector3.Lerp(cameraPivot.transform.position, target.transform.position, Time.deltaTime * smoothSpeed * distanceToPlayer);

        //Adds the camera Shake
        transform.localPosition = offset;
        transform.LookAt(cameraPivot.transform.position);
        shakeOffset = new Vector3(offsetX, offsetY, offsetZ);
        transform.position += shakeOffset;
        if (trauma <= 0)
        {

        }
        
        
        
        /*
		if (trauma > 0)
		{
			transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + offsetAngle);
		}
		else { transform.rotation = Quaternion.Euler(0, 0, 0); }
		*/
    }
}
