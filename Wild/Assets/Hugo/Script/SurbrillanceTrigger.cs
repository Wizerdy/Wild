using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurbrillanceTrigger : MonoBehaviour
{
    private SphereCollider sp;
    private float cooldown;
    public float SkilCooldown;
    public float Skilltime;

    private void Start()
    {
        sp = gameObject.GetComponent<SphereCollider>();
        sp.enabled = false;
    }
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.A) && cooldown < 0)
        {
            StartCoroutine(trigger());
            cooldown = SkilCooldown;
        } 
    }

    

    IEnumerator trigger() 
    {
        
        gameObject.transform.localScale = new Vector3(3, 1, 1);
        sp.enabled = true;
        yield return new WaitForSeconds(Skilltime);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        sp.enabled = false;
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Surbrillance>() != null)
        {
            other.gameObject.GetComponent<Surbrillance>().Shine(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Surbrillance>() != null)
        {
            other.gameObject.GetComponent<Surbrillance>().Shine(false);
        }
    }
}
