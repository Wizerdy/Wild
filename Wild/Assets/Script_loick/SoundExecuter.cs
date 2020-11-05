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
        switch (soundtype)
        {
            case Sound_Manager.soundname.AMB:
                break;
            case Sound_Manager.soundname.RSFX:
                break;
            case Sound_Manager.soundname.MUS:
                break;
            case Sound_Manager.soundname.NPC:
                break;
            case Sound_Manager.soundname.SFX:
                break;
            case Sound_Manager.soundname.FT:
                break;
            case Sound_Manager.soundname.LP:
                source.loop = true;
                source.Play();
                break;
            default:
                break;
        }
    }

}