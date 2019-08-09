using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BattleManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] Enemy;
    public GameObject other;
    public float[] EnemyDistance;
    public string skillname;
    private float angle;
    public void Start()
    {
        EnemyFinder();
        StartCoroutine("CalculateDistanceWithPlayer");
    }
    public void FixedUpdate()
    {
        CalculateDistanceWithPlayer();
        CalCulateAngleWithPlayer();
    }
    public void EnemyFinder()
    {
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        EnemyMapping();
    }
    public void EnemyMapping()
    {
        for(int i =0; i<Enemy.Length;i++)
        {
            Enemy[i].GetComponent<EnemyFollow>().stopDistance = Enemy[i].GetComponent<Monster>().AtkRange;
            Enemy[i].GetComponent<EnemyFollow>().speed = Enemy[i].GetComponent<Monster>().MoveSpeed;
        }
      
    }
    IEnumerator CalculateDistanceWithPlayer()
    {
        bool isActive = true;
        while(isActive)
        {
            EnemyDistance = new float[Enemy.Length];

            if (Enemy.Length == 0)
            {
                // yield return new WaitForSeconds(0.2f);
                isActive = false;
                StopCoroutine("CalculateDistanceWithPlayer");
                //Map Clear;
            }
            for (int a = 0; a < Enemy.Length; a++)
            {
                other = Enemy[0];
                Enemy[a].GetComponent<Monster>().distanceOfPlayer = DistanceCheckPlayerAndEnemy(player.GetComponent<Transform>(), Enemy[a].GetComponent<Transform>());
            }
            for (int a = 0; a < EnemyDistance.Length; a++)
            {
                if (other.GetComponent<Monster>().distanceOfPlayer > Enemy[a].GetComponent<Monster>().distanceOfPlayer)
                {
                    other = Enemy[a];
                }
            }
            yield return new WaitForSeconds(0.2f);
        }

    }
    public void Shoot()
    {
        //투사체 발사
        GameObject skill = ObjectPool.Instance.PopFromPool(skillname);
        skill.transform.position = transform.position + transform.up;
        skill.SetActive(true);
    }
    public void CalCulateAngleWithPlayer()
    {
        foreach(GameObject a in Enemy)
        {
            if (a.GetComponent<Monster>().distanceOfPlayer <= a.GetComponent<Monster>().moveDistance)
            {
                a.GetComponent<EnemyFollow>().enabled = true;
                a.GetComponent<Monster>().angleOfPlayer = GetSideOfEnemyAndPlayerAngle(player.transform.position, a.transform.position);
                angle = a.GetComponent<Monster>().angleOfPlayer;
                a.GetComponent<Monster>().MonsterAnimation(angle);
                a.GetComponent<Monster>().moveDistance = 256;
            }
            if (a.GetComponent<Monster>().distanceOfPlayer >= a.GetComponent<Monster>().moveDistance)
            {
                a.GetComponent<Monster>().MonsterAnimation(0);
                
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
