using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMoveLerp : ActionEntity
{
    private Vector3 destination = new Vector3();
    private float time = 0f;
    private int steps = 0;

    public override void Execute()
    {
        StartCoroutine(MoveTo(destination, time, steps));
    }

    IEnumerator MoveTo(Vector3 destination, float time, int steps)
    {
        Vector3 pos = transform.position;
        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForSeconds(time / (float)steps);
            entity.MoveToDestination(Vector3.Lerp(pos, destination, (float)i / (float)steps));
        }
        entity.MoveToDestination(destination);
        actionEnded = true;
    }
}
