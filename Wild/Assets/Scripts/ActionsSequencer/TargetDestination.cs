using System;
using UnityEngine;

[Serializable]
public class TargetDestination {
    public enum DESTINATION_TARGET_TYPE {
        VECTOR = 0,
        TRANSFORM,
    }

    public DESTINATION_TARGET_TYPE targetType = DESTINATION_TARGET_TYPE.VECTOR;

    public Vector3 vector = Vector3.zero;
    public Transform transform = null;

    public Vector3 FindDestination() {
        switch (targetType) {
            case DESTINATION_TARGET_TYPE.VECTOR:
                return vector;

            case DESTINATION_TARGET_TYPE.TRANSFORM:
                return transform.position;
        }

        return Vector3.zero;
    }

    public string DestinationName() {
        string name = "";

        if (targetType == DESTINATION_TARGET_TYPE.VECTOR) { name = "TO " + vector.ToString(); }
        else if (targetType == DESTINATION_TARGET_TYPE.TRANSFORM && transform != null) { name = "TO " + transform.name; }

        return name;
    }
}
