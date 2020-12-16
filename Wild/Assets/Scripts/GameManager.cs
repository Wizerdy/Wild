using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    [Header("PostProcessing")]
    public PostProcessProfile defaultProfile;
    public PostProcessProfile snakeProfile;

    [Header("Player")]
    public Entity player;

    [Header("Animation")]
    public GameObject dashButton;
    public Color[] dashColor;
    public GameObject instinctButton;
    public Color[] instinctColor;
    public GameObject chaseMode;

    public int hyenasChasing;

    public void IncrementChasingHyenas(int num)
    {
        hyenasChasing += num;
        if (hyenasChasing > 0)
        {
            GameManager.Instance.chaseMode.SetActive(true);
            //GameManager.Instance.chaseMode.GetComponent<Animation>().Play();
        } else
        {
            //GameManager.Instance.chaseMode.GetComponent<Animation>().Stop();
            GameManager.Instance.chaseMode.SetActive(false);
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
