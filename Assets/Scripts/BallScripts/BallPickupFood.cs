using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;

public class BallPickupFood : MonoBehaviour
{
    public List<GameObject> food = new List<GameObject>();
    public List<Vector3> startLocalPos = new List<Vector3>();
    public float ballDisplacement;
    public float displaceAdjustement = 0.5f;
    [SerializeField] Material ballMat;

    private Ball ball;

    [SerializeField] bool addFoodBump = true;
    [SerializeField] SphereCollider bumbCollider;
    [SerializeField] float bumpDepth = 0.5f;
    [SerializeField] float bumpRadius = 1f;
    public bool canPickUp = true;
    
    private void Awake()
    {
        ball = GetComponent<Ball>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food") && canPickUp)
        {
            GameObject parent = other.transform.parent.gameObject;
            parent.GetComponent<Collider>().enabled = false;
            other.GetComponent<Collider>().enabled = false;
            //disables collider for children
            //Debug.Log(parent);

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                if (parent.transform.GetChild(i).GetComponent<Collider>())
                {
                    parent.transform.GetChild(i).GetComponent<Collider>().enabled = false;
                }

                if (parent.transform.GetChild(i).GetComponent<DecalProjector>())
                {
                    parent.transform.GetChild(i).GetComponent<DecalProjector>().gameObject.SetActive(false);
                }
                
            }

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
                GameObject foodBump = Instantiate(foodBumpToSpawn, transform.position, Quaternion.identity);
                foodBump.transform.position -= vectorToCenter;
                foodBump.layer = 10; //makes the bumps not collide with the food
                SphereCollider bumpSphere = foodBump.AddComponent<SphereCollider>();
                bumpSphere.radius = bumpRadius;
                foodBump.transform.parent = foodAnchor.transform;
            }

            food.Add(parent);

            if(parent.GetComponent<BombFood>() != null) { parent.GetComponent<BombFood>().StartCoroutine(parent.GetComponent<BombFood>().ExplodeCountdown()); }

            // Please change this later
            ball.AddItem(other.transform.parent.GetComponent<Item>());
        }

        if (other.gameObject.CompareTag("Feeder"))
        {
            food.Clear();
            ball.SendItemsToPlate();
        }
    }

    public void RemoveFood(Item item)
    {
        food.Remove(item.gameObject);
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
