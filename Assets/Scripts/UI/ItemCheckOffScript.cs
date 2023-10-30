using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCheckOffScript : MonoBehaviour
{   
    [SerializeField] private GameObject changingImage;

    [SerializeField] private GameObject originalImage;

    [SerializeField] private bool despawn = false;

    [SerializeField] private int despawnTimer = 3;
    [SerializeField] private int respawnTimer = 3;

    private bool respawn = false;

    Changeimage changeImage;

    void Start ()
    {
        changeImage = originalImage.GetComponent<Changeimage>();
    }

    void Update ()
    {
        if (despawn == true)
        {
            StartCoroutine(despawner());
        }
        if (respawn == true)
        {
            StartCoroutine(respawner());
        }
    }
    IEnumerator despawner()
    {
        yield return new WaitForSeconds(despawnTimer);
        changingImage.SetActive(false);
        originalImage.SetActive(false);
        despawn = false;
        respawn = true;
    }
    IEnumerator respawner()
    {
        yield return new WaitForSeconds(respawnTimer);
        changingImage.SetActive(true);
        originalImage.SetActive(true);
        respawn = false;
        changeImage.FoodSpriteRandomize();
    }


}
