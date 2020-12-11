using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager {

    [CreateAssetMenu(fileName = "QueuedSound", menuName = "Sounds/QueuedSoundObject", order = 1)]
    public class QueuedSoundObject : SoundObject {
        [Space]
        public bool oneSoundAtTime = false;
        public int index = 0;

        [Space]
        [SerializeField] private Sound[] sounds;

        private AudioSource source;

        public override void Play() {
            if (sounds.Length == 0) { Debug.LogWarning("Sound object is empty : " + name); return; }

            if (sounds.Length == 1) {
                index = 0;
                Play(index);
                return;
            }

            index++;
            index %= sounds.Length;

            Play(index);
        }

        public void Play(int index, bool overwrite = true) {
            if (index >= sounds.Length) { Debug.LogWarning("Index exceed sounds length : " + index); return; }

            if (overwrite) this.index = index;
            if (oneSoundAtTime) Stop(source);

            Play(sounds[index]);
        }

        private void Play(Sound sound) {
            if (sound == null) { return; }

            source = SoundManager.instance.Play(sound);
        }

        private void Stop(AudioSource source) {
            SoundManager.instance.Stop(source);
        }
    }
}