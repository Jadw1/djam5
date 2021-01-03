using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    private IEnumerator LoadAsyncOperation()
    {
        var load = SceneManager.LoadSceneAsync("Levels");

        while (load.progress < 1)
        {
            slider.value = load.progress;
            
            yield return new WaitForEndOfFrame();
        }
    }
}