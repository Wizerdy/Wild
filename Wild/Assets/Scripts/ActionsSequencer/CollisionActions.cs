using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionActions : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onStay;
    [SerializeField] private UnityEvent onExit;

    private void OnTriggerEnter(Collider collider) {
        if(onEnter != null) {
            onEnter.Invoke();
        }
    }
    
    private void OnTriggerStay(Collider collider) {
        if(onStay != null) {
            onStay.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider collider) {
        if(onExit != null) {
            onExit.Invoke();
        }
    }
}
