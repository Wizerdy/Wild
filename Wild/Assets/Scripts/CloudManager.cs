using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [Header("Options")]
    public GameObject cloudPrefab;
    public Transform depart; 
    public Transform arrivé;
    public int rotationX;
    

    [Header("SpawnConfig")]

    [Range(0f,100f)]
    public float speed;
    public float spawnInterval;
    public int nbSpawn =1;
    

    List<Transform> cloudTransform = new List<Transform>();
    List<float> posList = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < cloudTransform.Count; i++)
        {
            posList[i] += (speed/100f) * Time.deltaTime;
            cloudTransform[i].position = Vector3.Lerp(depart.position, arrivé.position, posList[i]);

            if (posList[i] > 1f)
            {
                posList[i] = 0f;
                cloudTransform[i].position = depart.position;
            }
        }
        
    }

    IEnumerator Spawn() 
    {
        for (int i = 0; i < nbSpawn; i++)
        {
            Transform cloud = Instantiate(cloudPrefab, arrivé.position, Quaternion.identity).transform;
            cloud.eulerAngles = new Vector3(rotationX,0,0);
            posList.Add(0f);
            cloudTransform.Add(cloud);
            yield return new WaitForSeconds(spawnInterval);
        }
      
        
    }
}
