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
        Application.targetFrameRate = 60;
        //시작할때 while을 통해 코루틴을 기동시켜 반복 재생
        //EnemyFinder();
        //StartCoroutine("CalculateDistanceWithPlayer");
    }
    public void FixedUpdate()
    {
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        other = Enemy[0];
        float otherDistance = DistanceCheckPlayerAndEnemy(player.GetComponent<Transform>(), Enemy[0].GetComponent<Transform>());
        for (int a = 1; a < Enemy.Length; a++)
        {
            if (Enemy[a] != null)
            {
                float distanceOfPlayer = DistanceCheckPlayerAndEnemy(player.GetComponent<Transform>(), Enemy[a].GetComponent<Transform>());
                if (otherDistance > distanceOfPlayer)
                {
                    other = Enemy[a];
                    otherDistance = distanceOfPlayer;
                }
            }
        }
        //거리 측정하기~
        //CalculateDistanceWithPlayer();
    }
    public void EnemyFinder()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void SkillActive()
    {
        Debug.Log("스킬 작또오옹");
    }
    //적의 거리를 플레이어로 하여금 측정하도록 함
    public IEnumerator CalculateDistanceWithPlayer()
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
            int nullCount = 0;
            foreach (GameObject obj in Enemy)
            {
                if (obj == null) nullCount++;
            }
            if (Enemy.Length - nullCount > 0)
            {
                //GameObject[] temp_Enemy;
                //int nullCount = 0;
                //foreach(GameObject obj in Enemy)
                //{
                //    if (obj == null) nullCount++;
                //}
                //Debug.Log(nullCount);
                //if (!nullCount.Equals(0) && Enemy.Length - nullCount != 0)
                //{
                //    temp_Enemy = new GameObject[Enemy.Length - nullCount];
                //    int index = 0;
                //    for (int i = 0; i < Enemy.Length - nullCount; i++)
                //    {
                //        for(int j = 0; j < Enemy.Length; j++)
                //        {
                //            if (Enemy[j] == null) continue;
                //            if (index > j) continue;

                //            temp_Enemy[i] = Enemy[j];
                //            index = j;
                //        }
                //    }
                //}
                //////////////////////////////////////////// 쀄에에에에에에ㅔ에에에에에에에에에엑 

                ////////////////////////////////////////////////////////////
                player.GetComponent<Player>().EnemyPos = other.transform as Transform;
                if (DistanceCheckPlayerAndEnemy(player.GetComponent<Transform>(), other.GetComponent<Transform>()) < player.GetComponent<Player>().AtkRange)
                {
                    if (other == null)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
                    player.CurrentState = State.Attack;
                    if (other.GetComponent<Character>().HP > 0)
                    {
                        player.enemy_angle = GetSideOfEnemyAndPlayerAngle(other.transform.position, player.transform.position);
                    }
                }
                if (DistanceCheckPlayerAndEnemy(player.GetComponent<Transform>(), other.GetComponent<Transform>()) > player.GetComponent<Player>().AtkRange)
                {
                    if (other == null)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
                    player.CurrentState = State.Walk;
                    //적을 보는 각도(플레이어)
                    if (other.GetComponent<Character>().HP > 0)
                    {
                        player.enemy_angle = GetSideOfEnemyAndPlayerAngle(other.transform.position, player.transform.position);
                    }
                    if (other.GetComponent<Character>().HP < 0)
                    {
                        player.enemy_angle = player.current_angle;
                    }
                    //플레이어 조이스틱
                    // player.current_angle =
                }

                yield return new WaitForSeconds(0.1f);
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