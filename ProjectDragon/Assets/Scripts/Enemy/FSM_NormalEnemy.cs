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

    protected bool isAttackActive;


    protected IEnumerator Start_On()
    {
        //1초후 추적
        yield return new WaitForSeconds(1.0f);

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
                StartCoroutine(AttackRangeCheck());
                break;

            case State.Attack:
                StartCoroutine(Attack());
                break;

        }
    }

     protected virtual IEnumerator None()
    {
        StartCoroutine(AttackCooltime());

        while (true)
        {
            //AtkRange에 플레이어가 없다면 추적
            if (distanceOfPlayer > AtkRange)
            {
                CurrentState = State.Walk;
                yield break;
            }

            yield return null;
        }

    }


    protected virtual IEnumerator AttackRangeCheck()
    {
        while (true)
        {
            if (distanceOfPlayer < AtkRange)
            {
                //attack Animation parameters
                //

                isAttackActive = false;
                CurrentState = State.Attack;

                yield break;
            }

            yield return null;
        }

    }


    protected virtual IEnumerator Walk()
    {
        while (CurrentState == State.Walk)
        {
            //move
            transform.position = Vector2.MoveTowards(transform.position, other.transform.position, MoveSpeed * Time.deltaTime);

            objectAnimator.SetBool("Walk", true);

            yield return null;
        }
    }

    protected virtual IEnumerator Attack()
    {
        objectAnimator.SetBool("isAttackActive", isAttackActive);
        objectAnimator.SetBool("Walk", false);
        objectAnimator.SetTrigger("Attack");

        yield return null;
    }


    //근거리/애니메이션 프레임에 설정
    //원거리/탄환 충돌시
    protected virtual IEnumerator Attack_On()
    {
        //Add Player hurt
        //    
        //Debug.Log("Attack!!!!!");
        Debug.Log("안뇽친구들 난 부모에욘");
        yield return null;
    }


    protected virtual IEnumerator AttackCooltime()
    {
        yield return null;
    }

    //protected virtual IEnumerator Skill() { yield return null; }


}
