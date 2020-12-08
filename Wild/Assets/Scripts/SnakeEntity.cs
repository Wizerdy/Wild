using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEntity : AnimalEntity
{
    public float presenceRadius = 2f;
    public string preyGroupId;

    private GameObject FeelPresence(string preyId)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, presenceRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            Entity entity = colliders[i].gameObject.GetComponent<Entity>();
            for (int index = 0; index < entity.entityGroup.Length; i++)
            {
                if (entity != null && entity.entityGroup[index] == preyId && entity.GetComponent<AnimalEntity>().hidden == true)
                {
                    return entity.gameObject;
                }
            }
        }
        return null;
    }

    public void Attack()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (null != FeelPresence(preyGroupId))
        {
            GameObject obj = FeelPresence(preyGroupId);
            Attack();
        }
    }
}
