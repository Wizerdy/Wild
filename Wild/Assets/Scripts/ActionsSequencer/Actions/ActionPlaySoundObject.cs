using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundManager;

public class ActionPlaySoundObject : Action {
    public SoundObject soundObject;

    protected override void OnStart() { }

    protected override void OnExecute() {
        soundObject.Play();
    }

    public override bool IsActionEnded() {
        if (actionEnded) {
            return true;
        }

        return actionEnded;
    }
}
