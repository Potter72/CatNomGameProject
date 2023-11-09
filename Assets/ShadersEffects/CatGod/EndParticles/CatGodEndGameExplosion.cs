using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
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

    //credits
    [SerializeField] GameObject endCreditImage;
    [SerializeField] GameObject endCreditText;
    

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
        yield return new WaitForSeconds(4);
        catExplodeVFXGO.SetActive(false);

        
        //end game credits   
        endCreditImage.SetActive(true);
        endCreditText.SetActive(true);

        UnityEngine.UI.Image endImage = endCreditImage.GetComponent<UnityEngine.UI.Image>();
        TextMeshProUGUI endText = endCreditText.GetComponent<TextMeshProUGUI>();
        float creditTime = 2;
        float counter2 = 0;
        float startTime2 = Time.time;

        Color startColorImage = endImage.color;
        Color endColorImage = new Color(0.02f, 0.02f, 0.02f, 0.93f);
        Color startColorText = endText.color;
        Color endColorText = new Color(0.97f, 0.97f, 0.97f, 1);

        while (Time.time - startTime2 < creditTime)
        {
            endImage.color = Color.Lerp(startColorImage, endColorImage, counter2 / creditTime);
            endText.color = Color.Lerp(startColorText, endColorText, counter2 / creditTime);
            counter2 += Time.deltaTime;
            yield return 0;
        }
    }
}
