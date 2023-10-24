using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BallPickupFood : MonoBehaviour
{
    public List<GameObject> food = new List<GameObject>();
    public List<Vector3> startLocalPos = new List<Vector3>();
    public float ballDisplacement;
    [SerializeField] float displaceAdjustement = 0.5f;
    [SerializeField] Material ballMat;

    private Ball ball;

    [SerializeField] bool addFoodBump = true;
    [SerializeField] SphereCollider bumbCollider;
    [SerializeField] float bumpDepth = 0.5f;
    [SerializeField] float bumpRadius = 1f;
    
    private void Awake()
    {
        ball = GetComponent<Ball>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            GameObject parent = other.transform.parent.gameObject;
            parent.GetComponent<Collider>().enabled = false;
            other.GetComponent<Collider>().enabled = false;
            Destroy(parent.GetComponent<Rigidbody>());
            GameObject foodAnchorToSpawn = new GameObject("FoodAnchor");
            GameObject foodAnchor = Instantiate(foodAnchorToSpawn);

            foodAnchor.transform.position = parent.transform.position;
            foodAnchor.transform.rotation = parent.transform.rotation;

            foodAnchor.transform.parent = transform;
            parent.transform.parent = foodAnchor.transform;

            if (addFoodBump)
            {
                //creates a bumb
                GameObject foodBumpToSpawn = new GameObject("FoodBump");
                Vector3 vectorToCenter = (transform.position - foodAnchor.transform.position).normalized * bumpDepth;
                GameObject foodBump = Instantiate(foodBumpToSpawn, foodAnchor.transform.position + vectorToCenter, foodAnchor.transform.rotation);
                foodBump.layer = 10; //makes the bumps not collide with the food
                SphereCollider bumpSphere = foodBump.AddComponent<SphereCollider>();
                bumpSphere.radius = bumpRadius;
                foodBump.transform.parent = foodAnchor.transform;
            }

            food.Add(parent);

            // Please change this later
            ball.AddItem(other.transform.parent.GetComponent<Item>());
        }

        if (other.gameObject.CompareTag("Feeder"))
        {
            
            food.Clear();
            ball.SendItemsToPlate();
        }
    }

    private void FixedUpdate()
    {
        ballDisplacement = ballMat.GetFloat("_BounceAmount");
        for (int i = 0; i < food.Count; i++)
        {
            Transform foodAnchor = food[i].transform.parent.transform;

            float verticalDistance = (foodAnchor.position.y - (transform.position.y - transform.localScale.y / 2)) / displaceAdjustement;  //gets the vertical distance of the food compared to the player

            Vector3 endWorldPos = foodAnchor.position + Vector3.down * verticalDistance * ballDisplacement;

            food[i].transform.localPosition = foodAnchor.transform.InverseTransformPoint(endWorldPos);
        }
    }
}
