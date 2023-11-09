using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CatGodEndGameExplosion : MonoBehaviour
{
    [SerializeField] AnimationCurve sizeAnimCurve;
    [SerializeField] AnimationCurve warbleAnimCurve;
    [SerializeField] float explodeChargeTime = 5;
    [SerializeField] float trailsSpawnRate = 18;
    [SerializeField] GameObject catExplodeVFXGO;
    [SerializeField] VisualEffect catExplodeVFX;
    [SerializeField] GameObject catGodRendererGO;
    [SerializeField] Material catGodMaterial;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.O))  //for debugging
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        StartCoroutine(ExplodeCatGod());
    }

    IEnumerator ExplodeCatGod()
    {
        catExplodeVFX.SetFloat("Delay Time", explodeChargeTime);
        catExplodeVFX.SetFloat("TrailsSpawnRate", trailsSpawnRate);
        catExplodeVFXGO.SetActive(true);
        
        float startTime = Time.time;
        float counter = 0;
        Vector3 startScale = catGodRendererGO.transform.localScale;
        while (Time.time - startTime < explodeChargeTime) 
        {
            catGodRendererGO.transform.localScale = startScale * sizeAnimCurve.Evaluate((counter / explodeChargeTime));
            catGodMaterial.SetFloat("_Warble_Amplitude", warbleAnimCurve.Evaluate(counter / explodeChargeTime));
            counter += Time.deltaTime;
            yield return 0;

        }

        catExplodeVFX.SetFloat("TrailsSpawnRate", 0);
        catGodRendererGO.SetActive(false);
        catGodMaterial.SetFloat("_Warble_Amplitude", 0);
        yield return new WaitForSeconds(5);
        catExplodeVFXGO.SetActive(false);
    }
}
