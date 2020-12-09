using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public bool IsPaused = false;
    public GameObject optionscreen;
    private void Update()
    {
        if (IsPaused)
        {
            Time.timeScale = 0;
            optionscreen.SetActive(true);
        }
        if (!IsPaused)
        {
            Time.timeScale = 1;
            optionscreen.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = true;
            
        }
    }

    public void Resume() 
    {
        IsPaused = false;
    }
}
