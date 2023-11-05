using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Image _pie;
    [SerializeField] private Image _loadingBack;
    [SerializeField] private Image _loadingFront;
    [SerializeField] private Animator _fader;

    private AsyncOperation _asyncLoad;
    
    public void StartLoading()
    {
        StartCoroutine(FadeLoadingScreen());
    }

    IEnumerator FadeLoadingScreen()
    {
        float timer = 0f;
        Color b = _background.color;
        Color p = _pie.color;
        Color lb = _loadingBack.color;
        Color lf = _loadingFront.color;
        
        while (timer < 1f)
        {
            timer += 0.1f;
            b.a = timer;
            p.a = timer;
            lb.a = timer;
            lf.a = timer;

            _background.color = b;
            _pie.color = p;
            _loadingBack.color = lb;
            _loadingFront.color = lf;

            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        _asyncLoad = SceneManager.LoadSceneAsync(1);
        _asyncLoad.allowSceneActivation = false;
        
        while (!_asyncLoad.isDone)
        {
            _loadingFront.fillAmount = _asyncLoad.progress;
            yield return new WaitForSeconds(0.01f);
            
            if (_asyncLoad.progress >= 0.9f)
            {
                _loadingFront.fillAmount = _asyncLoad.progress;
                StartCoroutine(FadeToBlack());
            }
        }
    }

    IEnumerator FadeToBlack()
    {
        float timer = 0f;

        _fader.SetTrigger("Fade");

        yield return new WaitForSeconds(1f);
        
        _asyncLoad.allowSceneActivation = true;
    }

    
}
