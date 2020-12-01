using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound {
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0f, 1f)] public float pitch = 1f;

    public string name {
        get { return clip.name; }
        private set { }
    }
}

public class SoundManager : MonoBehaviour {
    public static SoundManager instance;

    public ISoundObject[] soundsObject;

    private Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();
    private Dictionary<string, List<AudioSource>> sources = new Dictionary<string, List<AudioSource>>();

    void Awake() {
        instance = this;

        //soundsObject = Tools.FindAssets<SoundObject>(); // Load all useful sounds

        //for (int i = 0; i < soundsObject.Length; i++) {
        //    for (int j = 0; j < soundsObject[i].sounds.Length; j++) {
        //        Sound s = soundsObject[i].sounds[j];

        //        if(!sources.ContainsKey(s.name)) {
        //            sounds.Add(s.name, s);
        //            AddSource(s);
        //        }
        //    }
        //}
    }

    //public AudioSource Play(string name) { // Obsolete
    //    //Sound s = Array.Find(sounds, sound => sound.name == name);
    //    Debug.LogWarning("Obselete method : Don't use that");

    //    if (!sources.ContainsKey(name)) { Debug.LogWarning("Can't find this sound : " + name); return null; }

    //    AudioSource source = null;
    //    source = GetNotPlayingSound(name);

    //    if (source == null) {
    //        AddSource(sounds[name]);
    //    }

    //    source.Play();
    //    return source;
    //}

    public void Play(Sound sound) {
        if (!sources.ContainsKey(sound.name)) {
            AddSource(sound);
        }

        AudioSource source = null;
        source = GetNotPlayingSound(name);

        if (source == null) {
            AddSource(sound);
        }

        source.pitch = sound.pitch;
        source.volume = sound.volume;
        source.Play();
    }

    public void Play(ISoundObject soundObject) {
        //if (soundObject.sounds.Length == 0) { Debug.LogWarning("Sound object is empty : " + soundObject.name); return; }

        //if (soundObject.sounds.Length == 1) {
        //    Play(soundObject.sounds[0]);
        //    return;
        //}

        //// Tirage au sort d'un son parmis le sound object

        //int totProb = 0;
        //for (int i = 0; i < soundObject.sounds.Length; i++) {
        //    totProb += soundObject.sounds[i].probability;
        //}

        //if (totProb < 100) { totProb = 100; }

        //int random = UnityEngine.Random.Range(0, totProb);

        //int prob = 0;
        //for (int i = 0; i < soundObject.sounds.Length; i++) {
        //    if (random < prob + soundObject.sounds[i].probability) {
        //        Play(soundObject.sounds[i]);
        //        return;
        //    }
        //    prob += soundObject.sounds[i].probability;
        //}

        soundObject.Play();
    }

    private AudioSource AddSource(Sound sound) {
        GameObject source = new GameObject(sound.name); // Create gameobject
        source.transform.parent = transform;

        AudioSource audioSource = source.AddComponent<AudioSource>(); // Add audioSource
        if (!sources.ContainsKey(name)) {
            sources.Add(sound.name, new List<AudioSource>());
        }

        sources[name].Add(audioSource);
        audioSource.clip = sound.clip;

        return audioSource;
    }

    private AudioSource GetNotPlayingSound(string name) {
        if (!sources.ContainsKey(name)) { Debug.LogWarning("This sound doesn't exist yet : " + name); return null; }

        AudioSource source = null;

        for (int i = 0; i < sources[name].Count; i++) {
            source = sources[name][i];
            if (!source.isPlaying) {
                return source;
            }
        }

        return null;
    }
}
