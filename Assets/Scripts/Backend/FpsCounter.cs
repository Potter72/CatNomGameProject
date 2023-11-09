using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField] float fpsUpdateTimer = 0.05f;
    // Start is called before the first frame update

    private void Start()
    {
        //if (Application.isMobilePlatform) Application.targetFrameRate = -1;
        Application.targetFrameRate = -1;
        StartCoroutine(UpdateFPSText());
    }
    IEnumerator UpdateFPSText()
    {
        while (fpsText)
        {
            fpsText.SetText("FPS: " + (1f / Time.unscaledDeltaTime));
            yield return new WaitForSecondsRealtime(fpsUpdateTimer);
        }
    }
}
