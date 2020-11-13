using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Carnivore : MonoBehaviour {
    public GameObject chased;
    public Transform NavA;
    public List<GameObject> Targets = new List<GameObject>();
    public Transform target;
    public bool chasing;

    public Vector3 InitPos;

    void Update() {
        UpdateBool();

        if (chasing) {
            NavA = chased.transform;
        }

        if (!chasing) {
            NavA = target;
        }
    }

    private void UpdateBool() {
        if (chased != null) {
            chasing = true;
        } else {
            chasing = false;
        }
    }

    private void UpdateTarget(int id) {
        switch (id) {
            case 0:
                target = Targets[1].transform;
                break;
            case 1:
                target = Targets[2].transform;
                break;
            case 2:
                target = Targets[0].transform;
                break;
        }
    }

    public void Detected(GameObject newTarget) {
        chased = newTarget;
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Lair") {
            chasing = false;
            chased = null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Target") {
            UpdateTarget(other.gameObject.GetComponent<Target>().ID);
        }
    }
}

