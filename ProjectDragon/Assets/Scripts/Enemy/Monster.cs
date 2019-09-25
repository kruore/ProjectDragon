using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Monster : Character
{
    public enum EnemyPos { None = 0, Front, Right, Left, Back };

    public Animator objectAnimator;
    protected EnemyPos enemyPos;
    public float moveDistance;

    BattleManager battleManager;

   

    //add 
    public string name;
    public int skillDamage;
    public float cooltime;
    public float skillCooltime;
    public float Attribute;
    enum KnockBackResistance { 상,중,하};




    //플레이어와 적과의 거리 캐스팅
    public float distanceOfPlayer;
    public float angleOfPlayer;


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        objectAnimator = gameObject.GetComponent<Animator>();
        other = GameObject.FindGameObjectWithTag("Player").transform;



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        

    }

    protected virtual void Start()
    {

        myState = State.Walk;
        setState(myState);
    }

    //개체의 상태가 바뀔때마다 실행
    public void setState(State newState)
    {
        myState = newState;
        switch(myState)
        {
            case State.None:
                StartCoroutine(Idle_On());
                break;

            case State.Walk:
                StartCoroutine(AttackRangeCheck());
                StartCoroutine(Tracking());
                break;

            case State.Attack:
                break;

        }
    }


    protected IEnumerator Tracking()
    {
        while (myState == State.Walk)
        {
            //move
            transform.position = Vector2.MoveTowards(transform.position, other.transform.position, MoveSpeed * Time.deltaTime);
            objectAnimator.SetBool("Walk", true);

            yield return null;
        }


    }

    //애니메이션 프레임에 설정할 것
    protected void Attack_On()
    {
        //Add Player Damage 
        //
        Debug.Log("Attack!!!!!");
       
    }

    protected IEnumerator Idle_On()
    {
        StartCoroutine(AttackCooltime());

        while (true)
        {
            //AtkRange에 플레이어가 없다면 추적
            if (distanceOfPlayer > AtkRange)
            {
                myState = State.Walk;
                setState(myState);
                yield break;
            }
 
        yield return null;
        }

    }


    //walk->attack
    protected IEnumerator AttackRangeCheck()
    {
        while (true)
        {
            if (distanceOfPlayer < AtkRange)
            {
                //attack Animation parameters
                //
                objectAnimator.SetBool("isAttackActive", false);
                objectAnimator.SetBool("Walk", false);
                objectAnimator.SetTrigger("Attack");
                myState = State.Attack;

                yield break;
            }

            yield return null;
        }

    }

    protected virtual IEnumerator AttackCooltime() {
        Debug.Log("hi");
        yield return null; }

    //protected virtual IEnumerator Attack

    //protected virtual IEnumerator Skill() { yield return null; }


    #region 전에 있던 코드
    ////Puppet_skill 01 is Move Back
    //IEnumerator Puppet_Skill01()
    //{
    //     //if(isSkillActive.Equals(true)) ? yield return new WaitForSeconds(skillCoolDown) :  yield return new WaitForSeconds(skillCoolDown);
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
    //                //      AnimatorCast(monsterName + AngleCalculate(angle));
    //                break;
    //        }
    //    }
    //}
    //IEnumerator AttackTime()
    //{
    //    yield return new WaitForSeconds(ATTACKSPEED);
    //}
    #endregion
}
