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
    private float duration;
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
          GenerateSound(soundname.AMB);
        }
        else if (soundclass == soundname.FT)
        {
          GenerateSound(soundclass); 
        }
        else if (soundclass == soundname.LP||soundclass == soundname.MUS)
        {
          GenerateSound(soundclass); 
        }
        else if (soundclass == soundname.NPC)
        {
          GenerateSound(soundclass); 
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


    public void GenerateSound(soundname soundtype)
    {
       
        int ran;
        GameObject[] soundlist = GameObject.FindGameObjectsWithTag("Sound");
        for (int i = 0; i < soundlist.Length; i++)
        {
            var source = soundlist[i].GetComponent<AudioSource>();
            if (source.isPlaying == false)
            {
                DestroyImmediate(soundlist[i]);
            }
        }
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
        obj.tag = "Sound";
        obj.AddComponent<Sound>();
        obj.AddComponent<CircleCollider2D>();
        var SC = obj.GetComponent<CircleCollider2D>();

        if (soundtype != soundname.FT)
        {
            DestroyImmediate(SC);
        }

 
   var SO = obj.GetComponent<Sound>();
       SO.SE = soundData[ran];
       obj.name = SO.SE.name;
   var Source = obj.AddComponent<AudioSource>();
       SO.SE.PlaySound(Source, SC,this);
        Debug.Log("Done");
    }
    public void GenerateSound(soundname soundtype, Vector3 vector)
    {
        int ran;
        ran = Random.Range(0, soundData.Count);
        GameObject[] soundlist = GameObject.FindGameObjectsWithTag("Sound");
        for (int i = 0; i < soundlist.Length; i++)
        {
            var source = soundlist[i].GetComponent<AudioSource>();
            if (source.isPlaying == false)
            {
                DestroyImmediate(soundlist[i]);
            }
        }

        while (true)
        {
            ran = Random.Range(0, soundData.Count);
            if (soundData[ran].soundname == soundtype)
            {
                break;
            }
        }
        var obj = new GameObject();
        obj.tag = "Sound";
        obj.AddComponent<Sound>();
        obj.AddComponent<CircleCollider2D>();
        var SC = obj.GetComponent<CircleCollider2D>();
        obj.transform.position = vector;
        if (soundtype != soundname.FT)
        {
            DestroyImmediate(SC);
        }
        var SO = obj.GetComponent<Sound>();
        SO.SE = soundData[ran];
        duration = SO.SE.clip.length;
        obj.name = SO.SE.name;
        var Source = obj.AddComponent<AudioSource>();
        SO.SE.PlaySound(Source, SC, this);
        Debug.Log("Done");
       
    }


    public void Changevolume_All(float volume) {volume_all = volume;}
    public void Changevolume_Sfx(float volume) {volume_sfx = volume;}
    public void Changevolume_Mus(float volume) {volume_mus = volume;}


    private void Awake()
    {
        InvokeRepeating("SpawnSE", 0f, 30f);
    }

    private void Update()
    {
        ManageSound(volume_all, soundtype.ALL);
        ManageSound(volume_sfx, soundtype.SFX);
        ManageSound(volume_mus, soundtype.MUS);

    }

}

