using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] Enemy;
    public List<GameObject> Enemys;
    public int EnemyCount;
    public GameObject other;
    public float[] EnemyDistance;


    public void Start()
    {
       

    }
    public void Update()
    {
        EnemyFinder();
        CalculateDistanceWithPlayer();
    }

    public void EnemyFinder()
    {
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void CalculateDistanceWithPlayer()
    {
        EnemyDistance = new float[Enemy.Length];

        if (Enemy.Length == 0)
        {
            return;
            //Map Clear;
        }
        for (int a = 0; a < Enemy.Length; a++)
        {
            EnemyDistance[a] = DistanceCheckPlayerAndEnemy(player.GetComponent<Transform>(), Enemy[a].GetComponent<Transform>());
        }
        for(int a= 1; a< EnemyDistance.Length; a++)
        {
            if(EnemyDistance[a]> EnemyDistance[a-1])
            {
                other = Enemy[a];
            }
            else if(EnemyDistance[a]<EnemyDistance[a-1])
            {
                other = Enemy[a - 1];
            }
        }
    }
    public float DistanceCheckPlayerAndEnemy(Transform Player, Transform Enemy)
    {
        Vector3 offset = Player.position - Enemy.position;
        float sqrLen = offset.sqrMagnitude;

        return sqrLen;
    }
}
