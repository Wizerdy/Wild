using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundExecuter
{
    public Sound_Manager.soundname soundtype;
    public AudioSource source;
    public void PlaySound()
    {
        if (soundtype == Sound_Manager.soundname.Song)
        {
            source.loop = true;
            source.Play();
        }
        else
        {
            source.loop = false;
            source.Play();
        }
    }

}