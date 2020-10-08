using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Action : MonoBehaviour {
    protected enum WaitType { NONE, TIME, ACTION_END, BOTH }

    protected WaitType waitType = WaitType.NONE;
    protected bool timeWaited = false;
    protected bool actionEnded = false;
    protected float timeToWait;


    protected void Start() {
        if(waitType == WaitType.TIME || waitType == WaitType.BOTH) {
            StartCoroutine("Wait", timeToWait);
        }
    }

    public bool IsFinished() {
        switch(waitType) {
            case WaitType.NONE:
                return true;
            case WaitType.TIME:
                return timeWaited;
            case WaitType.ACTION_END:
                return actionEnded;
            case WaitType.BOTH:
                return (actionEnded && timeWaited);
            default:
                Debug.LogError("Unknown WaitType");
                return true;
        }
    }

    protected IEnumerator Wait(float time) {
        yield return new WaitForSeconds(time);
        timeWaited = true;
    }

    public abstract void Execute();
}