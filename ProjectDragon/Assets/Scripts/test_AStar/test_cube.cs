using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_cube : MonoBehaviour
{
    EnemyTracking enemyTracking;

    private void Awake()
    {
        GameObject AStar = GameObject.Find("AStar");
        enemyTracking = AStar.transform.GetComponent<EnemyTracking>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        enemyTracking.Tracking(transform.position, 2.0f);
    }
}
