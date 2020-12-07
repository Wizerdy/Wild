using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileEntity : AnimalEntity
{
    public enum Awarness
    {
        NONE, SUSPICIOUS, HUNT
    }

    [Header("Field of view")]
    public int raycastNumber = 2;
    public float fieldOfView = 90f;
    public float distanceOfView = 10f;

    public float presenceRadius = 2f;

    [Header("Chase")]
    public float timeBeforeChase = 2f;
    public Awarness awarnessState;
    public string preyGroupId;
    //private GameObject prey;

    #region Properties

    public bool Suspicious
    {
        get { return (suspiciousFactor >= 1f ? true : false); }
        private set { }
    }

    #endregion

    private float suspiciousFactor = 0f;
    private bool isSuspicious = false;

    private GameObject FeelPresence(string preyId)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, presenceRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            Entity entity = colliders[i].gameObject.GetComponent<Entity>();
            for (int index = 0; index < entity.entityGroup.Length; i++)
            {
                if (entity != null && entity.entityGroup[index] == preyId)
                {
                    return entity.gameObject;
                }
            }
        }
        return null;
    }

    private void BeSuspicious()
    {
        isSuspicious = true;
    }

    private void UpdateSuspicious()
    {
        if (timeBeforeChase == 0f) { return; }

        if (isSuspicious)
        {
            if (suspiciousFactor < 1f)
            {
                suspiciousFactor += Time.deltaTime / timeBeforeChase;
            }
        }
        else if (suspiciousFactor > 0f)
        {
            suspiciousFactor -= Time.deltaTime / timeBeforeChase;
        }

        isSuspicious = false;
    }

    void Hunting(string preyId)
    {
        if (null != FeelPresence(preyId).GetComponent<LionCubEntity>())
        {
            FeelPresence(preyId).GetComponent<LionCubEntity>().Respawn();
        }
        else
        {
            Destroy(FeelPresence(preyId));
        }
        awarnessState = Awarness.SUSPICIOUS;
    }

    void Update()
    {
        if (null != FeelPresence(preyGroupId))
        {
            if (awarnessState == Awarness.NONE)
            {
                awarnessState = Awarness.SUSPICIOUS;
            }
            switch (awarnessState)
            {
                case Awarness.SUSPICIOUS:
                    BeSuspicious();
                    if (Suspicious)
                    {
                        awarnessState = Awarness.HUNT;
                    }
                    break;
                case Awarness.HUNT:
                    Hunting(preyGroupId);
                    break;
            }
        }

        UpdateSuspicious();

        if (suspiciousFactor <= 0f)
            awarnessState = Awarness.NONE;
    }
}
