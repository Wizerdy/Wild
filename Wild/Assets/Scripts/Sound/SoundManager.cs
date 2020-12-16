using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundManager {

    [Serializable]
    public class Sound {
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0f, 1f)] public float pitch = 1f;
        public bool loop = false;

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

        public MusicSoundObject[] musics;
        public int level = 0;
        public RandomSoundObject randomSounds;
        [Range(1.0f, 10.0f)] public float time;

        private Coroutine playDelay = null;
        private Coroutine stopDelay = null;

        #region Unity callbacks

        void Awake() {
            instance = this;

            if(musics.Length > 0) {
                PlayMusic(0);
            }

            if (randomSounds != null) {
                StartCoroutine(PlaySoundAtXSeconds(time));
            }

            #region Editor
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
            #endregion
        }

        #endregion

        #region Play

        public void Play(AudioSource source, Vector3? position = null) {
            if (!source.gameObject.activeSelf) {
                source.gameObject.SetActive(true);
            }

            if (position != null) {
                source.transform.position = (Vector3)position;
                source.rolloffMode = AudioRolloffMode.Custom;
                source.spatialize = true;
                source.spatialBlend = 1f;
                source.maxDistance = 100f;
            }

            source.Play();
        }

        public AudioSource Play(Sound sound, Vector3? position = null) {
            if (!sources.ContainsKey(sound.name)) {
                AddSource(sound);
            }

            AudioSource source = GetSource(sound);

            SetSource(ref source, sound);

            Play(source, position);
            return source;
        }

        public void Play(ISoundObject soundObject, Vector3? position = null) {
            soundObject.Play(position);
        }

        public AudioSource PlayWithDelay(Sound sound, float time) {
            if(playDelay != null) { StopCoroutine(playDelay); }

            AudioSource source = GetSource(sound);
            SetSource(ref source, sound);
            playDelay = StartCoroutine(PlayDelay(source, time));
            return source;
        }

        private IEnumerator PlayDelay(AudioSource source, float time) {
            yield return new WaitForSeconds(time);
            Play(source);
        }

        public IEnumerator PlaySoundAtXSeconds(float time)
        {
            Play(randomSounds);
            yield return new WaitForSeconds(time);
            StartCoroutine(PlaySoundAtXSeconds(time));
        }

        #endregion

        #region Stop

        public void Stop(AudioSource source) {
            source.Stop();
            source.gameObject.SetActive(false);
        }

        public void Stop(Sound sound) {
            if (!sources.ContainsKey(sound.name)) { Debug.LogWarning("Can't stop no-existent sound : " + sound.name); return; }

            AudioSource source = GeFirstPlayingSound(sound.name);
            Stop(source);
        }

        private AudioSource AddSource(Sound sound) {
            Transform parent;
            if (!sources.ContainsKey(sound.name)) {
                sources.Add(sound.name, new List<AudioSource>());
                parent = new GameObject(sound.name).transform;
                parent.transform.parent = transform;
            } else {
                parent = transform.Find(sound.name);
            }

            GameObject source = new GameObject(sound.name); // Create gameobject
            source.transform.parent = parent;

            AudioSource audioSource = source.AddComponent<AudioSource>(); // Add audioSource

            sources[sound.name].Add(audioSource);
            audioSource.clip = sound.clip;

            return audioSource;
        }

        public void StopWithDelay(AudioSource source, float time) {
            if (stopDelay != null) { StopCoroutine(stopDelay); }

            stopDelay = StartCoroutine(StopDelay(source, time));
        }

        private IEnumerator StopDelay(AudioSource source, float time) {
            yield return new WaitForSeconds(time);
            Stop(source);
        }

        #endregion

        #region Getters

        private AudioSource GetSource(Sound sound) {
            AudioSource source = null;
            source = GetNotPlayingSound(sound.name);

            if (source == null) {
                source = AddSource(sound);
            }
            return source;
        }

        private AudioSource GetNotPlayingSound(string name) {
            if (!sources.ContainsKey(name)) { /*Debug.LogWarning("This sound doesn't exist yet : " + name);*/ return null; }

            AudioSource source = null;

            for (int i = 0; i < sources[name].Count; i++) {
                source = sources[name][i];
                if (!source.isPlaying) {
                    return source;
                }
            }

            return null;
        }

        private AudioSource GeFirstPlayingSound(string name) {
            if (!sources.ContainsKey(name)) { /*Debug.LogWarning("This sound doesn't exist yet : " + name);*/ return null; }

            AudioSource source = null;

            for (int i = 0; i < sources[name].Count; i++) {
                source = sources[name][i];
                if (source.isPlaying) {
                    return source;
                }
            }

            return null;
        }

        #endregion

        #region Setters

        public void SetSource(ref AudioSource source, Sound sound) {
            source.pitch = sound.pitch;
            source.volume = sound.volume;
            source.loop = sound.loop;
        }

        #endregion

        public void ChangeLevel(int index) {
            level = index;
        }

        public void PlayMusic(int index) {
            //if (index >= musics[level]) { Debug.LogWarning("Music not set " + musics.Length + " .. " + index); return; }

            if (musics[level].Playing(index)) { return; }

            Debug.LogWarning("+ " + index);

            musics[level].Play(index);
        }
    }
}
