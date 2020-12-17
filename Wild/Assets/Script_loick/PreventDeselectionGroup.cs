using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class PreventDeselectionGroup : MonoBehaviour
{
    public EventSystem evt;
    public string Scenename;
    public GameObject @object;
    public VideoPlayer videoPlayer;
    private void Start()
    {   
        evt = EventSystem.current;
    }

    public GameObject sel;

    private void Update()
    {
        if (Time.time> 1 && videoPlayer != null && !videoPlayer.isPlaying)
        {
            StartCoroutine(LoadGame(Scenename));
            @object.SetActive(true);
            gameObject.SetActive(false);
        }
        if (evt.currentSelectedGameObject != null && evt.currentSelectedGameObject)
            sel = evt.currentSelectedGameObject;
        else if (sel != null && evt.currentSelectedGameObject == null)
            evt.SetSelectedGameObject(sel);
         if (evt.currentSelectedGameObject.active == false)
        {
            evt.SetSelectedGameObject(evt.firstSelectedGameObject);
        }
      
    }
    public void Select()
    {
            sel = evt.firstSelectedGameObject;
    }
    IEnumerator LoadGame(string scenename)
    {
        AsyncOperation Operation = SceneManager.LoadSceneAsync(scenename);
        while (!Operation.isDone)
        {
            yield return null;
        }
    }
}
