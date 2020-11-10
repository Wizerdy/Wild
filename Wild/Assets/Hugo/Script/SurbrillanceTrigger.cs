using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurbrillanceTrigger : MonoBehaviour
{
    private SphereCollider sp;
    public float range;
    private float cooldown;
    public float SkilCooldown;
    public float timeToGrow;
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
        float fps = 60f;
        Vector3 baseRange = gameObject.transform.localScale;
        sp.enabled = true;
        for (int i = 0; i < timeToGrow * fps; i++)
        {
            gameObject.transform.localScale = new Vector3(Mathf.Lerp(baseRange.x,range, (float)i/(timeToGrow*fps) ), 1, 1);
            yield return new WaitForSeconds(timeToGrow / (timeToGrow* fps));

        }
        yield return new WaitForSeconds(Skilltime);

        gameObject.transform.localScale = baseRange;
        yield return new WaitForSeconds(0.2f);
        sp.enabled = false;
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<Surbrillance>() != null)        {
            
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
