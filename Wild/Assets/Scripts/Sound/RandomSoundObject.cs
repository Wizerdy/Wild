using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager {

    [CreateAssetMenu(fileName = "RandomSound", menuName = "Sounds/RandomSoundObject", order = 2)]
    public class RandomSoundObject : SoundObject {
        [Serializable]
        private class ProbSound {
            public Sound sound = null;
            [Range(0, 100)] public int probability = 10;
        }

        [Space]
        [SerializeField] private ProbSound[] sounds;

        private AudioSource source;

        public override void Play() {
            if (sounds.Length == 0) { Debug.LogWarning("Sound object is empty : " + name); return; }

            if (sounds.Length == 1) {
                Play(sounds[0]);
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
                    Play(sounds[i]);
                    return;
                }
                prob += sounds[i].probability;
            }
        }

        private void Play(ProbSound sound) {
            if(sound == null) { return; }

            source = SoundManager.instance.Play(sound.sound);
        }
    }
}