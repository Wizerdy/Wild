using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager {
    public interface ISoundObject {
        void Play();
    }

    [Serializable]
    public abstract class SoundObject : ScriptableObject, ISoundObject {
        public abstract void Play();
    }
}
