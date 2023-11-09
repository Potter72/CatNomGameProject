using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BombFood : MonoBehaviour
{
    [SerializeField] float explodeTime = 5;
    [SerializeField] GameObject player;
    [SerializeField] BallPickupFood ballPickuper;
    [SerializeField] BallMagnet ballFoodMagnet;
    [SerializeField] float resetTimeAfterExploding = 2;
    [SerializeField] float removeForce = 5;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] GameObject bombPrefab;
    private float ballMagnetStartForce;
    public IEnumerator ExplodeCountdown()
    {
        player = transform.parent.parent.gameObject;
        ballPickuper = player.GetComponent<BallPickupFood>();
        ballFoodMagnet = GameObject.Find("PlayerMagnet").GetComponent<BallMagnet>();

        Color startColor = meshRenderer.material.GetColor("_ToonColor");
        Color endColor = meshRenderer.material.GetColor("_ToonColor") + Color.red * 3;
        float startScale = transform.localScale.x;
        float counter = 0;
        float startTime = Time.time;
        while (counter < explodeTime)
        {
            meshRenderer.material.SetColor("_ToonColor", Color.Lerp(startColor, endColor, (Mathf.Sin(counter * 8) + 1) / 2));
            transform.localScale = Vector3.one * (scaleCurve.Evaluate(counter / explodeTime) + startScale);
            counter = Time.time - startTime;

            yield return new WaitForSeconds(0.02f);
        }

        ballMagnetStartForce = ballFoodMagnet.pullForce;
        ballPickuper.canPickUp = false;
        ballFoodMagnet.pullForce = 0;

        for (int i = 0; i < player.transform.childCount; i++)
        {

            GameObject foodObject = player.transform.GetChild(i).GetChild(0).gameObject;
            GameObject foodAnchor = foodObject.transform.parent.gameObject;
            foodObject.transform.Find("FoodDecal").gameObject.SetActive(true);
            ballPickuper.food.Remove(foodObject);
            foodObject.transform.parent = null;
            foodObject.AddComponent<Rigidbody>();
            foodObject.GetComponent<Collider>().enabled = true;
            foodObject.GetComponentInChildren<Collider>().enabled = true;
            foodObject.GetComponent<Rigidbody>().AddForce((foodObject.transform.position - transform.position) * removeForce, ForceMode.Impulse);

            Destroy(player.transform.GetChild(i).gameObject);
            //Destroy(foodAnchor);
        }
        transform.parent = null;
        meshRenderer.enabled = false;
        player.GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position) * removeForce, ForceMode.Impulse);
        
        GameObject instantiatedBomb = Instantiate(bombPrefab,transform.position, Quaternion.identity);
        

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        //restarting pickup
        yield return new WaitForSeconds(resetTimeAfterExploding);
        ballPickuper.canPickUp = true;
        ballFoodMagnet.pullForce = ballMagnetStartForce;

        //waits to destroy spawned explosion and the now deactivated bomb
        yield return new WaitForSeconds(3);
        Destroy(instantiatedBomb);
        Destroy(gameObject);
    }
}
