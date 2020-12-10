using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEntity : AnimalEntity
{
    public float presenceRadius = 2f;
    public string preyGroupId;

    [Header("SnakeEffect")]
    public int reducedSpeedMax;
    public float effectTime;

    private GameObject FeelPresence(string preyId)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, presenceRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            Entity entity = colliders[i].gameObject.GetComponent<Entity>();
            if (entity != null && entity.IsEntityId(preyId))
            {
                return entity.gameObject;
            }
        }
        return null;
    }

    public void AttackLion(GameObject obj, float time)
    {
        if (obj != null)
        {
            Entity entity = obj.GetComponent<Entity>();
            entity.StartDashCooldown(time);
            entity.StartSpeedReducedForSeconds(reducedSpeedMax, time);
            entity.underEffect = true;
            Debug.Log("Attacked " + entity.speedMax);
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        AttackLion(FeelPresence(preyGroupId), effectTime);
    }
}
