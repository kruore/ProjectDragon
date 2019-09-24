using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyPos
{
    None = 0, Front, Right, Left, Back
}

public class Monster : Character
{

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

    bool isSkillActive = false;



    //플레이어와 적과의 거리 캐스팅
    public float distanceOfPlayer;
    public float angleOfPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        objectAnimator = gameObject.GetComponent<Animator>();
        //other = GameObject.FindGameObjectWithTag("Player").transform;

        battleManager= GetComponent<BattleManager>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
        setState(myState);

    }

    private void Start()
    {
        
        myState = State.Walk;

    }

    void setState(State newState)
    {
        myState = newState;
        switch(myState)
        {
            case State.None:

                break;

            case State.Walk:
                Tracking();
                break;

            case State.Attack:
                break;

        }
    }

    void Tracking()
    {
        if (distanceOfPlayer <= AtkRange)
        {
            myState = State.Attack;
            //attack

        }

        //move
        transform.position = Vector2.MoveTowards(transform.position, battleManager.player.transform.position, MoveSpeed * Time.deltaTime);

    }

    //애니메이션 프레임에 설정할 것
    void Attack_On()
    {
        //Add Player Damage 
        //

        //StartCoroutine(AttackCooltime());
    }
    protected virtual IEnumerator AttackCooltime() { yield return null; }
    //IEnumerator AttackCooltime()
    //{


    //    yield return new WaitForSeconds(cooltime);
    //    myState = State.None;

    //    yield return null;

    //}
    //protected virtual IEnumerator Skill() { yield return null; }



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
}
