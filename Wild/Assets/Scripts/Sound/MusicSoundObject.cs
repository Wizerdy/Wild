using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager {

    [CreateAssetMenu(fileName = "Music", menuName = "Sounds/MusicObject", order = 1)]
    public class MusicSoundObject : SoundObject {
        [Serializable]
        private class NamedSound {
            public string name;
            public Sound sound;
        }

        [Space]
        [SerializeField] private int bpm;

        [Space]
        [SerializeField] private NamedSound[] sounds;

        private AudioSource source;
        private float bps;

        #region Properties

        private bool IsPlaying {
            get { return (source != null && source.isPlaying); }
        }

        private float Bps {
            get { if (bpm > 0 && bps <= 0f) { bps = 60f / bpm; } return bps; }
        }

        #endregion

        public override void Play() {
            if (sounds.Length == 0) { Debug.LogWarning("Sound object is empty : " + name); return; }

            Play(sounds[0].name);
        }

        public void Play(string name, bool inRythm = true) {
            if (sounds.Length == 0) { Debug.LogWarning("Sound object is empty : " + this.name); return; }

            NamedSound sound = Find(name);
            if (sound == null) { Debug.LogWarning("Sound not found in the object : " + name); return; }

            if (inRythm) {
                PlayInRythm(sound.sound);
                return;
            }

            if (IsPlaying) {
                SoundManager.instance.Stop(source);
            }

            Play(sound);
        }

        private void PlayInRythm(Sound sound) {
            float timeToWait = 0f;

            if (IsPlaying) {
                timeToWait = Bps - source.time % Bps;
                SoundManager.instance.StopWithDelay(source, timeToWait);
                timeToWait = Bps - source.time % Bps;
                source = null;
            }

            source = SoundManager.instance.PlayWithDelay(sound, timeToWait);
        }

        private void Play(NamedSound sound) {
            if (sound == null) { return; }

            source = SoundManager.instance.Play(sound.sound);
        }

        private void Stop(AudioSource source) {
            SoundManager.instance.Stop(source);
            source = null;
        }

        private NamedSound Find(string name) {
            for (int i = 0; i < sounds.Length; i++) {
                if (name.Equals(sounds[i].name)) {
                    return sounds[i];
                }
            }
            return null;
        }
    }
}