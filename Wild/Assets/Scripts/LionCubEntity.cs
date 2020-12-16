using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LionCubEntity : AnimalEntity {
    public string predatorGroup = "Hyenas";
    public Vector2 spawnPoint;
    public bool isDying = false;
    public bool changingcolor = false;
    public GameObject MachoirAnim;
    public GameObject Gameover;
    public GameObject fondu;
   

    //protected override void OnTriggerEnter(Collider collide) {
    //    base.OnTriggerEnter(collide);
    //    if (collide.gameObject.tag == "Hide") {
    //        if (collide.gameObject.GetComponent<Entity>() != null) {
    //            Entity[] entities = EntitiesManager.FindEntities(collide.gameObject.GetComponent<Entity>().entityGroup[1]);
    //            for (int i = 0; i < entities.Length; i++) {
    //                Tools.ChangeAlphaMaterial(entities[i].gameObject, 100);
    //            }
    //        } else {
    //            Tools.ChangeAlphaMaterial(collide.gameObject, 100);
    //        }
    //    }
    //}
    //protected override void OnTriggerExit(Collider collide) {
    //    base.OnTriggerExit(collide);
    //    if (hideCoat <= 0 && collide.gameObject.tag == "Hide") {
    //        if (collide.gameObject.GetComponent<Entity>() != null) {
    //            Entity[] entities = EntitiesManager.FindEntities(collide.gameObject.GetComponent<Entity>().entityGroup[1]);
    //            for (int i = 0; i < entities.Length; i++) {
    //                Tools.ChangeAlphaMaterial(entities[i].gameObject, 255);
    //            }
    //        } else {
    //            Tools.ChangeAlphaMaterial(collide.gameObject, 255);
    //        }
    //    }
    //}

    protected void OnCollisionEnter(Collision collision) {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (entity == null) return;

        if(Array.IndexOf(entity.entityGroup, predatorGroup) >= 0 && !isDying) {
            isDying = true;
            gameOver();
           
        }
    }

   

    public void Respawn() {
        Gameover.SetActive(false);
        fondu.SetActive(false);
        isDying = false;
        Entity[] hyenas = EntitiesManager.FindEntities("Hyenas");
        MoveInstant(spawnPoint.ConvertTo3D());

        if (hyenas == null) return;

        for (int i = 0; i < hyenas.Length; i++) {
            HyenaEntity hyena = hyenas[i].gameObject.GetComponent<HyenaEntity>();
            //if (hyena != null && (hyena.awarness == HyenaEntity.Awarness.SUSPICIOUS || hyena.awarness == HyenaEntity.Awarness.CHASING)) {
                hyenas[i].GetComponent<HyenaEntity>().ResetToStart();
                //Debug.Log("Patrol " + hyenas[i].name);
            //}
        }
    }
    
    public void gameOver() 
    {
        fondu.SetActive(true);
        fondu.GetComponentInChildren<Animation>().Play();
        Debug.Log("here");
        StartCoroutine(nextgameover());

    }

    IEnumerator nextgameover() 
    {
        yield return new WaitForSeconds(1);
        MachoirAnim.SetActive(true);
        MachoirAnim.GetComponentInChildren<Animation>().Play();
        yield return new WaitForSeconds(2.5f);
        MachoirAnim.GetComponentInChildren<Animation>().Stop();
        MachoirAnim.SetActive(false);
        Gameover.SetActive(true);

    }

    public void ChangeSpawnPoint(Vector3 position) {
        ChangeSpawnPoint(position.ConvertTo2D());
    }

    public void ChangeSpawnPoint(Vector2 position) {
        spawnPoint = position;
    }

    public void Hide(HideZone zone) {
        if (hidden == false) {
            hidden = true;
            hideId = "Hole";
            Position = zone.transform.position.Overwrite(Position.y, Tools.IgnoreMode.Y);
        } else {
            hidden = false;
            hideId = "";
            Position = zone.exitPoint;
        }
    }
}