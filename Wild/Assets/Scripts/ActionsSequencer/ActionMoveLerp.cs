using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMoveLerp : ActionEntity {
    public Vector3 destination = new Vector3();
    public float time = 0f;
    public int steps = 0;

    // Editor
    [HideInInspector] public int currentTab;

    public override void Execute() {
        StartCoroutine(MoveTo(destination, time, steps));
    }

    IEnumerator MoveTo(Vector3 destination, float time, int steps) {
        Vector3 pos = transform.position;
        for (int i = 0; i < steps; i++) {
            yield return new WaitForSeconds(time / (float)steps);
            entity.MoveToDestination(Vector3.Lerp(pos, destination, (float)i / (float)steps));
        }
        entity.MoveToDestination(destination);
        actionEnded = true;
    }
}
