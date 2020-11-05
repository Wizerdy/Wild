using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Action : MonoBehaviour {
    public enum WaitType { NONE, TIME, ACTION_END, BOTH }

    public WaitType waitType = WaitType.NONE;
    protected bool timeWaited = false;
    protected bool actionEnded = false;
    public float timeToWait = 0f;

    protected virtual void Start() {
        
    }

    public bool IsFinished() {
        switch(waitType) {
            case WaitType.NONE:
                return true;
            case WaitType.TIME:
                return timeWaited;
            case WaitType.ACTION_END:
                return IsActionEnded();
            case WaitType.BOTH:
                return (IsActionEnded() && timeWaited);
            default:
                Debug.LogError("Unknown WaitType");
                return true;
        }
    }

    protected IEnumerator Wait(float time) {
        yield return new WaitForSeconds(time);
        timeWaited = true;
    }

    public virtual void Execute() {
        if (waitType == WaitType.TIME || waitType == WaitType.BOTH) {
            StartCoroutine("Wait", timeToWait);
        }
    }

    public abstract bool IsActionEnded();

    public virtual void ResetAction() {
        actionEnded = false;
        timeWaited = false;
        StopAllCoroutines();
    }
}