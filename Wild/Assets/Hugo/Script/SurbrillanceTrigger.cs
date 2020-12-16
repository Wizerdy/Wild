using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurbrillanceTrigger : MonoBehaviour
{
    private SphereCollider sp;
    private Coroutine isPlaying;
    private float cooldown;

    public Entity lion;
    public byte alpha;
    public float range;
    public float SkillCooldown;
    public float timeToGrow;
    public float Skilltime;

    private void Start() {
        sp = gameObject.GetComponent<SphereCollider>();
        sp.enabled = false;
    }

    private void Update() {
        cooldown -= Time.deltaTime;
        /*if (Input.GetKeyDown(KeyCode.A) && cooldown < 0 && null == isPlaying) {
            lion.StopMove(timeToGrow + Skilltime);
            Tools.ChangeAlphaMaterial(gameObject, alpha);
            transform.position = lion.transform.position;
            isPlaying = StartCoroutine(trigger());
            cooldown = SkillCooldown;
        }*/
    }

    public void ActiveInstinctMode()
    {
        cooldown -= Time.deltaTime;
        if (cooldown < 0 && null == isPlaying)
        {
            lion.StopMove(timeToGrow + Skilltime);
            GameManager.Instance.instinctButton.GetComponent<Image>().color = GameManager.Instance.instinctColor[0];
            Tools.ChangeAlphaMaterial(gameObject, alpha);
            transform.position = lion.transform.position;
            isPlaying = StartCoroutine(trigger());
            cooldown = SkillCooldown;
        }
    }

    IEnumerator trigger() {
        float fps = 60f;
        Vector3 baseRange = gameObject.transform.localScale;
        sp.enabled = true;
        for (int i = 0; i < timeToGrow * fps; i++) {
            gameObject.transform.localScale = new Vector3(Mathf.Lerp(baseRange.x,range, (float)i/(timeToGrow*fps) ), 1, Mathf.Lerp(baseRange.x, range, (float)i / (timeToGrow * fps)));
            yield return new WaitForSeconds(timeToGrow / (timeToGrow* fps));
        }
        yield return new WaitForSeconds(Skilltime);
        lion.StopMove(0.01f);
        Tools.ChangeAlphaMaterial(gameObject, 0);
        gameObject.transform.localScale = baseRange;
        yield return new WaitForSeconds(0.2f);
        sp.enabled = false;
        isPlaying = null;
        GameManager.Instance.instinctButton.GetComponent<Image>().color = GameManager.Instance.instinctColor[1];
        yield break;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponentInChildren<Surbrillance2d>() != null) {
            other.gameObject.GetComponentInChildren<Surbrillance2d>().Shine(true);
        }
        
        if (other.gameObject.GetComponent<Surbrillance3d>() != null) {
            other.gameObject.GetComponent<Surbrillance3d>().Shine(true);
        } 
        
        if (other.gameObject.GetComponent<SurbrillanceSkinnedMesh>() != null) {
            other.gameObject.GetComponent<SurbrillanceSkinnedMesh>().Shine(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponentInChildren<Surbrillance2d>() != null) {
            other.gameObject.GetComponentInChildren<Surbrillance2d>().Shine(false);
        }
        if (other.gameObject.GetComponent<Surbrillance3d>() != null) {
            other.gameObject.GetComponent<Surbrillance3d>().Shine(false);
        } 
        
        if (other.gameObject.GetComponent<SurbrillanceSkinnedMesh>() != null) {
            other.gameObject.GetComponent<SurbrillanceSkinnedMesh>().Shine(false);
        }
    }
}