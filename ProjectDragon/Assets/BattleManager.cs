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

    private float angle;


    public void Start()
    {
       
    }
    public void FixedUpdate()
    {
        EnemyFinder();
        CalculateDistanceWithPlayer();
        CalCulateAngleWithPlayer();
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
            Enemy[a].GetComponent<Monster>().distanceOfPlayer = DistanceCheckPlayerAndEnemy(player.GetComponent<Transform>(), Enemy[a].GetComponent<Transform>());
        }
        for(int a= 1; a< EnemyDistance.Length; a++)
        {
            if(Enemy[a].GetComponent<Monster>().distanceOfPlayer > Enemy[a-1].GetComponent<Monster>().distanceOfPlayer)
            {
                other = Enemy[a];
            }
            else if (Enemy[a].GetComponent<Monster>().distanceOfPlayer < Enemy[a - 1].GetComponent<Monster>().distanceOfPlayer)
            {
                other = Enemy[a - 1];
            }
        }
    }
    public void CalCulateAngleWithPlayer()
    {
        foreach(GameObject a in Enemy)
        {
            if (a.GetComponent<Monster>().distanceOfPlayer <= a.GetComponent<Monster>().AtkRange)
            {
                a.GetComponent<TestMonster>().angleOfPlayer = GetSideOfEnemyAndPlayerAngle(player.transform.position, a.transform.position);
                angle = a.GetComponent<TestMonster>().angleOfPlayer;
                a.GetComponent<TestMonster>().MonsterAnimation(angle);
            }
        }

    }
    
    #region 편의기능
    public float DistanceCheckPlayerAndEnemy(Transform Player, Transform Enemy)
    {
        Vector3 offset = Player.position - Enemy.position;
        float sqrLen = offset.sqrMagnitude;
        return sqrLen;
    }
    public static float GetSideOfEnemyAndPlayerAngle(Vector3 Player, Vector3 Enemy)
    {
        return Quaternion.FromToRotation(Vector3.up, Enemy - Player).eulerAngles.z;
    }

    #endregion
}
