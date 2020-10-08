using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorTrigger : MonoBehaviour
{
    private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponent<NpcMover>().sneak == false)
        {
            
            parent.GetComponent<Carnivore>().Detected(other.gameObject);
        }
        
    }
}
