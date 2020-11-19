using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundExecuter : ScriptableObject
{
    public Sound_Manager.soundname soundname;
    public Sound_Manager.soundtype soundtype;
    public AudioClip clip;
    public float soundvolume;
    public AudioSource source;
    private Sound_Manager SE;

    public SoundExecuter(Sound_Manager sound_Manager)
    {
        SE = sound_Manager;
    }

    public void PlaySound(AudioSource audiosource,SphereCollider sphereCollider,Sound_Manager sound_Manager)
    {
        source = audiosource;
        source.volume = soundvolume;
        source.clip = clip;
        switch (soundname)
        {
            case Sound_Manager.soundname.AMB:
                source.Play();
                break;
            case Sound_Manager.soundname.RSFX:
                source.Play();
                break;
            case Sound_Manager.soundname.MUS:
                source.loop = true;
                source.Play();
                break;
            case Sound_Manager.soundname.NPC:
                source.Play();
                break;
            case Sound_Manager.soundname.SFX:
                source.Play();
                break;
            case Sound_Manager.soundname.FT:
                source.Play();
                sound_Manager.StartCoroutine(GrowSphere(10,sphereCollider));
                break;
            case Sound_Manager.soundname.LP:
                source.loop = true;
                source.Play();
                break;
            default:
                break;
        }
        
        
    }
    public IEnumerator GrowSphere(int radius,SphereCollider Collision)
    {
        for (int i = 0; i < radius; i++)
        {
            Collision.radius = i;
            yield return new WaitForSeconds(0.001f);
        }
    }

}