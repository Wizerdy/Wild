using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Sound_Manager : MonoBehaviour
{
    public List<SoundExecuter>soundData;
    public AudioClip soundtoadd;
    public soundname soundclass;
    public float volume_all;
    public float volume_sfx;
    public float volume_mus;
    //les différents type de son
    public enum soundname
    {
        AMB,RSFX,MUS,NPC,SFX,FT,LP
    }
    public enum soundtype
    {
      ALL,SFX,MUS
    }
    //fait apparaitre un effet sonore dans la scene
    public void SpawnSE()
    { 
        if (soundclass == soundname.AMB ||soundclass == soundname.RSFX)
        {
           StartCoroutine(GenerateSound(soundname.AMB));
        }
        else if (soundclass == soundname.FT)
        {
            StartCoroutine(GenerateSound(soundclass)); 
        }
        else if (soundclass == soundname.LP||soundclass == soundname.MUS)
        {
            StartCoroutine(GenerateSound(soundclass)); 
        }
        else if (soundclass == soundname.NPC)
        {
            StartCoroutine(GenerateSound(soundclass)); 
        }

    }
    //ajoute un son à la base de données sonores
    public void AddSound()
    {
        int error = 0;
        for (int i = 0; i < soundData.Count; i++)
        {
            if (soundtoadd.name == soundData[i].clip.name)
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
        SoundExecuter soundadded = new SoundExecuter(this);
        soundadded.clip = soundtoadd;
        soundadded.soundname = soundclass;
        soundData.Add(soundadded);
       AssetDatabase.CreateAsset(soundadded,"Assets/Resources/Sound/"+soundadded.clip.name+".asset");
        }
    }
    //Controle due niveau sonores





    public void ManageSound(float sn,soundtype soundtype)
    {
        for (int i = 0; i < soundData.Count; i++)
        {
            if (soundData[i].soundname == soundclass)
            {
                soundData[i].soundvolume = sn;
            }
        }
    }

    //Cooldown avant destruction du son


    public IEnumerator GenerateSound(soundname soundtype)
    {
        int ran;
        ran = Random.Range(0, soundData.Count);
        while (true)
        {
            ran = Random.Range(0, soundData.Count);
            if (soundData[ran].soundname == soundtype)
            {
                break;
            }
        }

        var obj = new GameObject();
        obj.AddComponent<Sound>();
        obj.AddComponent<SphereCollider>();
        var SC = obj.GetComponent<SphereCollider>();

        if (soundtype != soundname.FT)
        {
            DestroyImmediate(SC);
        }

 
   var SO = obj.GetComponent<Sound>();
       SO.SE = soundData[ran];
       obj.name = SO.SE.name;
   var Source = obj.AddComponent<AudioSource>();
       SO.SE.PlaySound(Source, SC,this);
       yield return new WaitForSeconds(SO.SE.clip.length);
        DestroyImmediate(obj);
        Debug.Log("Done");
    }
    public IEnumerator GenerateSound(soundname soundtype, Vector3 vector)
    {
        int ran;
        ran = Random.Range(0, soundData.Count);
        while (true)
        {
            ran = Random.Range(0, soundData.Count);
            if (soundData[ran].soundname == soundtype)
            {
                break;
            }
        }
        var obj = new GameObject();
        obj.AddComponent<Sound>();
        obj.AddComponent<SphereCollider>();
        var SC = obj.GetComponent<SphereCollider>();
        obj.transform.position = vector;
        if (soundtype != soundname.FT)
        {
            DestroyImmediate(SC);
        }
        var SO = obj.GetComponent<Sound>();
        SO.SE = soundData[ran];
        obj.name = SO.SE.name;
        var Source = obj.AddComponent<AudioSource>();
        SO.SE.PlaySound(Source, SC, this);
        yield return new WaitForSeconds(SO.SE.clip.length);
        DestroyImmediate(obj);
        Debug.Log("Done");
       
    }


    public void Changevolume_All(float volume) {volume_all = volume;}
    public void Changevolume_Sfx(float volume) {volume_sfx = volume;}
    public void Changevolume_Mus(float volume) {volume_mus = volume;}


    private void Update()
    {
        ManageSound(volume_all, soundtype.ALL); ManageSound(volume_sfx, soundtype.SFX);ManageSound(volume_mus, soundtype.MUS);
    }
    private void Start()
    {
        SpawnSE();
    }


    public IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("wait sucessfully");
    }

}

