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

    public AnimationCurve propagation;
    public float timeToGrow;
    public float Skilltime;

    public float fadeAwayTime;
    public Transform graphic;

    private void Start() {
        sp = gameObject.GetComponent<SphereCollider>();
        sp.enabled = false;
    }

    private void Update() {
        cooldown -= Time.deltaTime;

        if (cooldown < 0) {
            GameManager.Instance.instinctButton.GetComponent<Image>().color = GameManager.Instance.instinctColor[1];
        }
    }

    public void ActiveInstinctMode()
    {
        if (cooldown < 0 && null == isPlaying)
        {
            //lion.StopMove(timeToGrow + Skilltime);
            GameManager.Instance.instinctButton.GetComponent<Image>().color = GameManager.Instance.instinctColor[0];

            Color color = graphic.GetComponent<SpriteRenderer>().color;
            color.a = 1f;
            graphic.GetComponent<SpriteRenderer>().color = color;

            //Tools.ChangeAlphaMaterial(gameObject, alpha);
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
            float scale = Mathf.Lerp(baseRange.x, range, propagation.Evaluate((float)i / (timeToGrow * fps)));
            gameObject.transform.localScale = new Vector3(scale, 1, scale);
            yield return new WaitForSeconds(timeToGrow / (timeToGrow * fps));
        }
        yield return new WaitForSeconds(Skilltime);

        for (int i = 0; i < fadeAwayTime * fps; i++) {
            Color color = graphic.GetComponent<SpriteRenderer>().color;
            color.a = Mathf.Lerp(255, 0, (float)i / (fadeAwayTime * fps)) / 255f;
            graphic.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(fadeAwayTime / (fadeAwayTime * fps));
        }

        //lion.StopMove(0.01f);
        //Tools.ChangeAlphaMaterial(gameObject, 0);
        gameObject.transform.localScale = baseRange;
        yield return new WaitForSeconds(0.2f);
        sp.enabled = false;
        isPlaying = null;
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

        if(other.gameObject.GetComponent<HyenaEntity>() != null) {
            other.gameObject.GetComponent<HyenaEntity>().ShowVision(true);
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

        if (other.gameObject.GetComponent<HyenaEntity>() != null) {
            other.gameObject.GetComponent<HyenaEntity>().ShowVision(false);
        }
    }
}