using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    public List<SoundExecuter> sound;
    public AudioClip audioClip;
    public int index;
    public enum soundname
    {
        SoundEffect,Song
    }
    public void SpawnSE()
    {
       Instantiate(new GameObject(audioClip.name));
    }
}

