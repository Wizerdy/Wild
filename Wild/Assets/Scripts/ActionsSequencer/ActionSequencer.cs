using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequencer : MonoBehaviour
{
    [HideInInspector] public Action[] actions;
    public bool launch = false;

    private int actionIndex = -1;
    private bool running = false;

    #region Properties

    public bool IsRunning {
        get { return running; }
        private set {}
    }

    public bool HasActions {
        get { return actions.Length > 0; }
        private set {}
    }

    #endregion

    #region Unity callbacks

    void Awake() {
        actions = GetComponentsInChildren<Action>();
    }

    void Update() {
        if (launch && HasActions) {
            UpdateAction();
        }
    }

    #endregion

    void UpdateAction() {
        running = true;

        if (actionIndex == -1) {
            actionIndex++;
            actions[actionIndex].Execute();
        }

        while (!(actionIndex == actions.Length - 1) && actions[actionIndex].IsFinished()) {
            actionIndex++;
            actions[actionIndex].Execute();
        }

        if (actions[actions.Length - 1].IsFinished()) {
            ResetActions();
        }
    }

    public void ResetActions() {
        if (HasActions) {
            for (int i = 0; i < actions.Length; i++) {
                actions[i].ResetAction();
            }
        }

        actionIndex = -1;
        running = false;
        launch = false;
    }

    public void Launch() {
        launch = true;
    }
}
