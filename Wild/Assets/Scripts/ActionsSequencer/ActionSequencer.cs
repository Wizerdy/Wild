using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequencer : MonoBehaviour
{
    [HideInInspector] public Action[] actions;
    [HideInInspector] public bool launch = false;
    [HideInInspector] int actionIndex = -1;

    private bool running = false;

    void Awake()
    {
        actions = GetComponentsInChildren<Action>();
    }

    void Update()
    {
        if(launch && actions.Length > 0)
        {
            UpdateAction();
        }
    }

    void UpdateAction()
    {
        running = true;

        if(actionIndex == -1)
        {
            actionIndex++;
            actions[actionIndex].Execute();
        }

        while (!(actionIndex == actions.Length - 1) && actions[actionIndex].IsFinished())
        {
            actionIndex++;
            actions[actionIndex].Execute();
        }

        if(actions[actions.Length - 1].IsFinished())
        {
            running = false;
        }
    }

    public bool IsRunning()
    {
        return running;
    }
}
