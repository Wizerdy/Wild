using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreventDeselectionGroup : MonoBehaviour
{
   public EventSystem evt;

    private void Start()
    {
        evt = EventSystem.current;
    }

    public GameObject sel;

    private void Update()
    {
        if (evt.currentSelectedGameObject != null && evt.currentSelectedGameObject)
            sel = evt.currentSelectedGameObject;
        else if (sel != null && evt.currentSelectedGameObject == null)
            evt.SetSelectedGameObject(sel);
         if (evt.currentSelectedGameObject.active == false)
        {
            evt.SetSelectedGameObject(evt.firstSelectedGameObject);
        }
      
    }
    public void Select()
    {
            sel = evt.firstSelectedGameObject;
    }
}
