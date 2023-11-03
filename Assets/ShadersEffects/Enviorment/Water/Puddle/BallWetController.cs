using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BallWetController : MonoBehaviour
{
    [SerializeField] bool dripWhenWet = false;
    [SerializeField] Material material;
    [SerializeField] float drySpeed;
    [SerializeField] VisualEffect dripVFX;
    [SerializeField] float dripSpeed = 24;
    private bool isInWater;
    private void Start()
    {
        if (material == null) material = GetComponent<Renderer>().material;
        if (dripVFX == null) dripVFX = GameObject.Find("PlayerWaterDropVFX").GetComponent<VisualEffect>();
        if(!dripWhenWet) { dripVFX.SetFloat("SpawnRate", 0); }
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

        if (!isInWater && material.GetFloat("_WaterAmount") > 0) 
        {
            float oldWetness = material.GetFloat("_WaterAmount");
            oldWetness -= drySpeed;
            material.SetFloat("_WaterAmount", oldWetness);
        }
        else if(!isInWater)
        {
            material.SetFloat("_WaterAmount", 0);
        }
    }
}
