using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    [Header("플레이어블 캐릭터")]
    public Player player;
    [Header("적")]
    public GameObject[] Enemy;
    //그 외의 단일 적 사용하기
    public GameObject other;
    [Header("적이 사용해야하는 각도")]
    private float angle;

    [Header("적과의 거리 측정")]
    public float[] EnemyDistance;

    [Header("사용할 스킬")]
    public string skillname;

    public void Start()
    {
        //시작할때 while을 통해 코루틴을 기동시켜 반복 재생
        EnemyFinder();
        StartCoroutine("CalculateDistanceWithPlayer");
    }
    public void FixedUpdate()
    {
        //거리 측정하기~
        CalculateDistanceWithPlayer();
    }
    public void EnemyFinder()
    {
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    //적의 거리를 플레이어로 하여금 측정하도록 함
    IEnumerator CalculateDistanceWithPlayer()
    {
        bool isActive = true;
        while (isActive)
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
            player.GetComponent<Player>().EnemyPos = other.transform as Transform;
            if (DistanceCheckPlayerAndEnemy(player.GetComponent<Transform>(), other.GetComponent<Transform>()) < player.GetComponent<Player>().AtkRange)
            {
                if (other == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                player.StateChaner(State.Attack);
                player.enemy_angle = GetSideOfEnemyAndPlayerAngle(other.transform.position, player.transform.position);
                player.current_angle = GetSideOfEnemyAndPlayerAngle(player.normalVec, player.joystickPos);

            }
            if (DistanceCheckPlayerAndEnemy(player.GetComponent<Transform>(), other.GetComponent<Transform>()) > player.GetComponent<Player>().AtkRange)
            {
                if (other == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                player.StateChaner(State.Walk);
                //적을 보는 각도(플레이어)
                player.enemy_angle = GetSideOfEnemyAndPlayerAngle(other.transform.position,player.transform.position);
                //플레이어 조이스틱
                player.current_angle = GetSideOfEnemyAndPlayerAngle(player.normalVec, player.joystickPos);
            }

            yield return new WaitForSeconds(0.1f);
        }

    }
    public void CalCulateAngleWithPlayer()
    {
        //적의 각도 측정기(적)
        foreach (GameObject a in Enemy)
        {
            if (a.GetComponent<Monster>().distanceOfPlayer <= a.GetComponent<Monster>().moveDistance)
            {
                a.GetComponent<EnemyFollow>().enabled = true;
                a.GetComponent<Monster>().angleOfPlayer = GetSideOfEnemyAndPlayerAngle(player.transform.position, a.transform.position);
                angle = a.GetComponent<Monster>().angleOfPlayer;
                //a.GetComponent<Monster>().MonsterAnimation(angle);
                a.GetComponent<Monster>().moveDistance = 256;
            }
            if (a.GetComponent<Monster>().distanceOfPlayer >= a.GetComponent<Monster>().moveDistance)
            {
                //a.GetComponent<Monster>().MonsterAnimation(0);
            }
        }
    }
    #region 편의기능
    //거리를 찍어주는 것
    public float DistanceCheckPlayerAndEnemy(Transform Player, Transform Enemy)
    {
        Vector3 offset = Player.position - Enemy.position;
        float sqrLen = offset.sqrMagnitude;
        return sqrLen;
    }
    //거리 말고 나머지 앵글(각도 측정)
    public static float GetSideOfEnemyAndPlayerAngle(Vector3 Player, Vector3 Enemy)
    {
        return Quaternion.FromToRotation(Vector3.up, Enemy - Player).eulerAngles.z;
    }
    #endregion
}