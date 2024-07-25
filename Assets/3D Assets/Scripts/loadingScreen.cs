using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class loadingScreen : MonoBehaviour
{
    public GameObject screenloaderpanel;
    public Slider slider;
    public TextMeshProUGUI progresstext;
   
    
    private void Start()
    {
        
    }
    public void loadingScreener(int sceneindex)
    {
        StartCoroutine(screenLoader(sceneindex)); 
       
    }

    IEnumerator screenLoader(int sceneindex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneindex);
        while(operation.progress < 0.9f)
        {
           screenloaderpanel.SetActive(true);
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progresstext.text = progress * 100f + " %";

            yield return null;
        }
    }

    
}
