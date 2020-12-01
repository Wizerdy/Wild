using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Sound", menuName = "Sounds/SoundObject", order = 1)]
public interface ISoundObject {
    void Play();
}

[CreateAssetMenu(fileName = "RandomSound", menuName = "Sounds/RandomSoundObject", order = 0)]
public class RandomSoundObject : ScriptableObject, ISoundObject {
    [Serializable]
    public struct ProbSound {
        public Sound sound;
        public int probability;
    }

    private AudioSource source;

    public ProbSound[] sounds;

    public void Play() {
        if (sounds.Length == 0) { Debug.LogWarning("Sound object is empty : " + name); return; }

        if (sounds.Length == 1) {
            SoundManager.instance.Play(sounds[0].sound);
            return;
        }

        int totProb = 0;
        for (int i = 0; i < sounds.Length; i++) {
            totProb += sounds[i].probability;
        }

        if (totProb < 100) { totProb = 100; }

        int random = UnityEngine.Random.Range(0, totProb);

        int prob = 0;
        for (int i = 0; i < sounds.Length; i++) {
            if (random < prob + sounds[i].probability) {
                SoundManager.instance.Play(sounds[i].sound);
                return;
            }
            prob += sounds[i].probability;
        }
    }
}

[CreateAssetMenu(fileName = "Music", menuName = "Sounds/MusicObject", order = 1)]
public class MusicSoundObject : ScriptableObject, ISoundObject {
    private AudioSource source;
    public Sound[] sounds;

    public void Play() {
        
    }
}
