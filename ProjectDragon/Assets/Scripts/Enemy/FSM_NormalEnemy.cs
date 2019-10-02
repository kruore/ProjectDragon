using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_NormalEnemy : Monster
{
    public State CurrentState
    {
        get { return myState; }
        set
        {
            myState = value;
            setState(myState);
        }
    }

    [SerializeField]
    protected bool isAttackActive;


    protected IEnumerator Start_On()
    {
        //1초후 추적
        yield return new WaitForSeconds(1.0f);

        StartCoroutine(AttackRangeCheck());
        CurrentState = State.Walk;
        yield return null;
    }


    //개체의 상태가 바뀔때마다 실행
    public void setState(State newState)
    {
        switch (newState)
        {
            case State.None:
                
                StartCoroutine(None());
                break;

            case State.Walk:
                
                StartCoroutine(Walk());
                break;

            case State.Attack:
                StartCoroutine(Attack());
                break;

        }
    }
    [SerializeField]
    bool inAtkRange;
    protected virtual IEnumerator None()
    {
        Debug.Log("None");
        StartCoroutine(CalcCooltime());
        //StartCoroutine(AttackCooltime());

        yield return null;

    }
    [SerializeField]
    public float Current_waitTime = 0;
    [SerializeField]
    public float Current_cooltime = 0;
    public virtual IEnumerator CalcCooltime()
    {
        while (true)
        {

            if (Current_cooltime < cooltime)                    //cooltime 전
            {

                if (Current_waitTime < waitTime)                 //waitTime 전
                {
                    Current_waitTime+= Time.deltaTime;
                    Current_cooltime = Current_waitTime;
                }
                else                                             //waitTime 후
                {
                    //공격범위에 플레이어가 없다면 추적
                    if (!inAtkRange)
                    {
                        CurrentState = State.Walk;   //Idle->Walk
                        yield break;
                    }
                    else  //공격범위에 플레이어가 있다면 대기
                    {
                        Current_cooltime+= Time.deltaTime;

                    }
                }
            }
            else                                                 //cooltime 후
            {
                if (CurrentState == State.None)     //Idle->Attack
                {
                    isAttackActive = true;
                    CurrentState = State.Attack;
                    yield break;

                }
            }
                yield return null;
        }
    }


    protected virtual IEnumerator AttackRangeCheck()
    {
        while (true)
        {
            inAtkRange = false;

            if (distanceOfPlayer < AtkRange)
            {
                inAtkRange = true;
                
            }
            yield return null;
        }

    }


    protected virtual IEnumerator Walk()
    {
        Debug.Log("Walk");
        while (CurrentState == State.Walk)
        {
            //공격범위에 들어오면 Attack
            if(inAtkRange)
            {
                isAttackActive = false;
                CurrentState = State.Attack;
                yield break;
            }
            objectAnimator.SetBool("Walk", true);
            //move
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, MoveSpeed * Time.deltaTime);

            yield return null;
        }

    }

    //attack Animation parameters
    protected virtual IEnumerator Attack()
    {
        Debug.Log("Attack");

        objectAnimator.SetBool("Walk", false);
        objectAnimator.SetTrigger("Attack");
        objectAnimator.SetBool("isAttackActive", isAttackActive);

        Current_waitTime = 0;
        Current_cooltime = 0;

        yield return null;
    }


    //근거리/애니메이션 프레임에 설정  -->몸과 충돌시
    //원거리 -->탄환 충돌시
    protected virtual IEnumerator Attack_On()
    {
        //Add Player hurt
        //    

        yield return null;
    }


    protected virtual IEnumerator AttackCooltime()
    {
        yield return null;
    }

    //protected virtual IEnumerator Skill() { yield return null; }


}
