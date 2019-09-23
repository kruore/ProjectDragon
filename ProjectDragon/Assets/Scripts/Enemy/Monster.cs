using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle, Walk, Attack, Hit, Dead
}
public enum EnemyPos
{
    None = 0, Front, Right, Left, Back
}

public class Monster : Character
{
    public float skillCoolDown = 10.0f;
    public bool IsSkillActive = false;
    public string monsterName;
    public Animator objectAnimator;
    protected EnemyPos enemyPos;
    public float moveDistance;

    //add patrol
    public Vector3 beginPos;    
    public Vector3 patrolPoint; 
    public bool Arrived = true;

    public float patrolRange;
    public float findRange;
    public float attackRange;

    public float speed=2.0f;


    //플레이어와 적과의 거리 캐스팅
    public float distanceOfPlayer;
    public float angleOfPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        objectAnimator = gameObject.GetComponent<Animator>();
        other = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Start()
    {
        beginPos = gameObject.transform.position;
        myState = State.None;

    }

    IEnumerator Patrol()
    {
        myState = State.None;

        while (true)
        {
            if (distanceOfPlayer <= findRange)
            {
                yield break;
            }

            if (Arrived)
            {
                //Idle Animation
                //1초 대기 
                yield return new WaitForSeconds(1.0f);

                Arrived = false;

                //random patrolPoint
                patrolPoint = beginPos + new Vector3(Random.insideUnitCircle.x * patrolRange, Random.insideUnitCircle.y * patrolRange);

            }
            //move
            transform.position = Vector2.MoveTowards(transform.position, patrolPoint, speed * Time.deltaTime);

            //Arrived at point
            if (myPos.Equals(patrolPoint))
            {
                Arrived = true;
            }

            yield return null;
        }


    }

    void PlayerTracking()
    {
        myState = State.Walk;

        if (distanceOfPlayer <= attackRange)
        {
            //attack
        }

        //move
        //transform.position=Vector2.MoveTowards(transform.position,)

    }




    ////Puppet_skill 01 is Move Back
    //IEnumerator Puppet_Skill01()
    //{
    //    //  if(IsSkillActive.Equals(true)) ? yield return new WaitForSeconds(skillCoolDown) :  yield return new WaitForSeconds(skillCoolDown);
    //    {
    //        yield return new WaitForSeconds(skillCoolDown);
    //    }
    //    yield return new WaitForSeconds(0);
    //}
    //public void MonsterAnimation(float angle)
    //{
    //    if (myAttackType.Equals(AttackType.ShortRange))
    //    {
    //        switch (myState)
    //        {
    //            case State.Walk:
    //          //      AnimatorCast(monsterName + AngleCalculate(angle));
    //                break;
    //        }
    //    }
    //}
    //IEnumerator AttackTime()
    //{
    //    yield return new WaitForSeconds(ATTACKSPEED);
    //}
}
