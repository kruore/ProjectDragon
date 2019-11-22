using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [Header("플레이어블 캐릭터")]
    public Player player;
    [Header("적")]
    public GameObject[] Enemy;
    public List<GameObject> EnemyArray;
    //그 외의 단일 적 사용하기
    public GameObject other;
    [Header("적이 사용해야하는 각도")]
    private float angle;

    public AudioClip BGM;

    [Header("적과의 거리 측정")]
    public List<float> EnemyDistance;

    [Header("사용할 스킬")]
    public string skillname;

    public void Start()
    {
        //SoundManager.Inst.Ds_BgmPlayer(BGM);
        Application.targetFrameRate = 60;
        //시작할때 while을 통해 코루틴을 기동시켜 반복 재생
        EnemyFinder();
        StartCoroutine("CalculateDistanceWithPlayer");
    }
    public void FixedUpdate()
    {
        //거리 측정하기~
        // CalculateDistanceWithPlayer();
    }
    public void EnemyFinder()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (Enemy.Length > 1)
        {
            for (int i = 0; i < Enemy.Length; i++)
            {
                EnemyArray.Add(Enemy[i]);
            }
        }
        else
        {
            return;
        }
    }
    public void SkillActive()
    {
        Debug.Log("스킬 작또오옹");
    }
    //적의 거리를 플레이어로 하여금 측정하도록 함
    public IEnumerator CalculateDistanceWithPlayer()
    {
        bool isActive;
        if (EnemyArray.Count >= 1)
        {
            isActive = true;
            while (isActive)
            {
                List<GameObject> EnemyDistanceArray = new List<GameObject>(EnemyArray.Count);
                for (int i = 0; i < EnemyArray.Count; i++)
                {
                    if (EnemyArray[i].Equals(null))
                    {
                        EnemyArray.RemoveAt(i);
                    }
                    EnemyDistanceArray.Add(EnemyArray[i]);
                }
                if (EnemyDistanceArray.Count > 0)
                {
                    for (int a = 0; a < EnemyDistanceArray.Count; a++)
                    {
                        other = EnemyDistanceArray[0];
                        EnemyDistanceArray[a].GetComponent<Monster>().distanceOfPlayer = DistanceCheckPlayerAndEnemy(player.GetComponent<Transform>(), EnemyDistanceArray[a].GetComponent<Transform>());
                    }
                    for (int a = 0; a < EnemyDistanceArray.Count; a++)
                    {
                        if (other.GetComponent<Monster>().distanceOfPlayer > EnemyDistanceArray[a].GetComponent<Monster>().distanceOfPlayer)
                        {
                            other = EnemyDistanceArray[a];
                        }
                    }
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
        else if (EnemyArray.Count.Equals(0))
        {
            //Map Clear;
            // yield return new WaitForSeconds(0.2f);
            isActive = false;
            StopCoroutine("CalculateDistanceWithPlayer");
            EnemyFinder();
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
    public void GotoLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
#endregion