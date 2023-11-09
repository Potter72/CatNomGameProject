using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class BallWetController : MonoBehaviour
{
    [SerializeField] bool dripWhenWet = false;
    [SerializeField] Material material;
    [SerializeField] float drySpeed;
    [SerializeField] VisualEffect dripVFX;
    [SerializeField] BallPickupFood ballPickuper;
    [SerializeField] BallMagnet ballFoodMagnet;
    [SerializeField] Ball ballItemController;
    private float ballMagnetStartForce;
    [SerializeField] float dripSpeed = 24;
    [SerializeField] bool isFullWet = false;

    //removing food
    [SerializeField] float removeForce = 2;
    [SerializeField] bool isInWater;
    private void Start()
    {
        if (material == null) material = GetComponent<Renderer>().material;
        if (dripVFX == null) dripVFX = GameObject.Find("PlayerWaterDropVFX").GetComponent<VisualEffect>();
        if(!dripWhenWet) { dripVFX.SetFloat("SpawnRate", 0); }
        if (ballFoodMagnet) ballMagnetStartForce = ballFoodMagnet.pullForce;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            Debug.Log("inWater");
            isInWater = true;

            float wetSpeed = other.GetComponent<WaterPuddle>().WetSpeed;

            if (wetSpeed > 0 && material.GetFloat("_WaterAmount") < 1)
            {
                float oldWetness = material.GetFloat("_WaterAmount");
                oldWetness += wetSpeed;
                material.SetFloat("_WaterAmount", oldWetness);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            isInWater = false;
        }
    }
    private void FixedUpdate()
    {
        if (dripWhenWet)
        {
            if (material.GetFloat("_WaterAmount") > 0)
            {
                dripVFX.SetFloat("SpawnRate", dripSpeed * material.GetFloat("_WaterAmount"));
            }
            else
            {
                dripVFX.SetFloat("SpawnRate", 0);
            }
        }

        if (!isInWater && isFullWet)
        {
            if(material.GetFloat("_WaterAmount") > 0)
            {
                float oldWetness = material.GetFloat("_WaterAmount");
                oldWetness -= drySpeed;
                material.SetFloat("_WaterAmount", oldWetness);
            }
            else //starting pickup again
            {
                material.SetFloat("_WaterAmount", 0);
                Debug.Log("canpickup");
                ballPickuper.canPickUp = true;
                ballFoodMagnet.pullForce = ballMagnetStartForce;
                isFullWet = false;
            }
        }
  


        //removing food whenn full wet
        if (isInWater)
        {
            if (material.GetFloat("_WaterAmount") >= 1)
            {
                isFullWet = true;
                ballPickuper.canPickUp = false;
                for (int i = 0; i < transform.childCount; i++)
                {

                    GameObject foodObject = transform.GetChild(i).GetChild(0).gameObject;
                    foodObject.transform.Find("FoodDecal").gameObject.SetActive(true);
                    ballPickuper.food.Remove(foodObject);
                    foodObject.transform.parent = null;
                    foodObject.AddComponent<Rigidbody>();
                    foodObject.GetComponent<Collider>().enabled = true;
                    foodObject.GetComponentInChildren<Collider>().enabled = true;
                    foodObject.GetComponent<Rigidbody>().AddForce((foodObject.transform.position - transform.position) * removeForce, ForceMode.Impulse);

                    ballFoodMagnet.pullForce = 0;

                    Destroy(transform.GetChild(i).gameObject);

                    
                }

                ballItemController.ClearAllItems();
            }
        }
       
    }
}