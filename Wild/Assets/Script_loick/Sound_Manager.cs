using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    public List<SoundExecuter> sound;
    public AudioSource soundtoadd;
    public int index;
    public soundname soundclass;
    public enum soundname
    {
        AMB,RSFX,MUS,NPC,SFX,FT,LP
    }
    public void SpawnSE()
    {
       Instantiate(new GameObject(sound[index].source.name));
    }
    public void AddSound()
    {
        int error = 0;
        for (int i = 0; i < sound.Count; i++)
        {
            if (soundtoadd.clip == null || soundtoadd.name == sound[i].source.name)
            {
                error++;
            }

        }
            if (error>0)
            {
                Debug.LogError("Error you can add "+ error +" item");
            }
        else
        {
        SoundExecuter soundadded = new SoundExecuter();
        soundadded.source = soundtoadd;
        soundadded.soundtype = soundclass;
        sound.Add(soundadded);
        }
    }
}

