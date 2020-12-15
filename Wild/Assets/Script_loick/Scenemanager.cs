using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenemanager : MonoBehaviour
{
    public string scenename;
    public Image image;
    public void LoadScene(string scene)
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        StartCoroutine(LoadGame(scene));
    }

    IEnumerator LoadGame(string scenename)
    {
        AsyncOperation Operation = SceneManager.LoadSceneAsync(scenename);
        while (!Operation.isDone)
        {
            float progress = Mathf.Clamp01(Operation.progress / 0.9f);
            image.fillAmount = progress;
            yield return null;
        }
    }
    public void LoadScene()
    {
        LoadScene(scenename);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
