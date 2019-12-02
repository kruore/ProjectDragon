﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NormalEnemyState { Idle, Walk, Attack, Wait, Hit}
public class FSM_NormalEnemy : Enemy
{

    [Header(" ")]
    bool isAttackActive;

    [Header("[Enemy State]")]
    [SerializeField] protected NormalEnemyState normalEnemyState;
    public NormalEnemyState NEState
    {
        get { return normalEnemyState; }
        set
        {
            normalEnemyState = value;
            SetEnemyState(normalEnemyState);
        }
    }


    protected override void SetState(State newState)
    {
        if (newState == State.Dead)
        {
            StartCoroutine(Dead());
        }
    }

    public override int HPChanged(int ATK)
    {
        //살아 있을때
        if (!isDead)
        {
            isHit = true;
            isWalk = false;

            //White Shader
            StartCoroutine(flashWhite.Flash());
            //넉백
            StartCoroutine(DirectionKnockBack());


            return base.HPChanged(ATK);
        }

        return 0;
    }

    public override IEnumerator Start_On()
    {
        StartCoroutine(base.Start_On());
        
        //1초후 추적
        yield return new WaitForSeconds(1.0f);
        NEState = NormalEnemyState.Walk;

        //공격감지 체크
        StartCoroutine(AttackRangeCheck());


        yield return null;
    }




    protected virtual IEnumerator Idle()
    {
        objectAnimator.SetBool("Attack", false);
        StartCoroutine(CalcCooltime());
        yield return null;
    }

    public virtual IEnumerator CalcCooltime()
    {
        while (NEState == NormalEnemyState.Idle&&!isDead)
        {
            //[조건] cooltime > readyTime
            if (Current_cooltime < cooltime)                    //cooltime 전
            {
                if (Current_readyTime < readyTime)                 //readyTime 전
                {
                    Current_readyTime += Time.deltaTime;
                    Current_cooltime = Current_readyTime;
                }
                else                                             //readyTime 후
                {
                    //공격범위에 플레이어가 없다면 추적
                    if (!inAtkDetectionRange)
                    {
                        isAttackActive = false;
                        NEState = NormalEnemyState.Walk;   //Idle->Walk
                        yield break;
                    }
                    else  //공격범위에 플레이어가 있다면 대기
                    {
                        Current_cooltime += Time.deltaTime;
                    }
                }
            }
            else                                                 //cooltime 후
            {
                if (NEState == NormalEnemyState.Idle)     //Idle->Attack
                {
                    isAttackActive = true;
                    NEState = NormalEnemyState.Attack;
                    yield break;
                }
            }
            yield return null;
        }
    }


    protected virtual IEnumerator Walk()
    {
        //Walk Animation parameters
        objectAnimator.SetBool("Walk", true);
        //GetComponent<Tracking>().pathFinding.Create(col.size.x, col.size.y, transform.GetComponentInParent<t_Grid>());

        float currentWalkTime = 0;
        float walkTime = Random.Range(2.0f, 6.0f);

        while (NEState == NormalEnemyState.Walk && !isDead)
        {
            //공격감지범위에 들어오면 Attack
            if (inAtkDetectionRange)
            {
                isWalk = false;
                //if(!isHit)
                //{
                //rb2d.velocity = Vector2.zero;  //move가 velocity로 하는거라면 넣기
                //}
                NEState = NormalEnemyState.Attack;
                yield break;
            }

            //move
            if (!isHit && !collisionPlayer)
            {
                isWalk = true;
                currentWalkTime += Time.deltaTime;
                if (walkTime < currentWalkTime)
                {
                    //Wait
                    NEState = NormalEnemyState.Wait;
                    yield break;

                }
                //AStar
                Debug.Log("Move");
                GetComponent<Tracking>().FindPathManager(rb2d, MoveSpeed);


                //rb2d.velocity = direction * MoveSpeed * 10.0f * Time.deltaTime;
                //transform.position = Vector3.MoveTowards(transform.position, other.transform.position, MoveSpeed * Time.deltaTime);
            }
            //else
            //{
            //    isWalk = false;
            //    rb2d.velocity = Vector2.zero;
            //}
            yield return null;
        }
    }

    protected IEnumerator Wait()
    {
        //Walk Animation parameters
        objectAnimator.SetBool("Wait", true);
        objectAnimator.SetBool("Walk", false);

        float waitTime = Random.Range(3.0f, 5.0f);
        float current_waitTime = 0;

        isWalk = false;
        //rb2d.velocity = Vector2.zero;
        while (NEState == NormalEnemyState.Wait && !isDead)
        {
            //공격감지범위에 들어오면 Attack
            if (inAtkDetectionRange)
            {
                NEState = NormalEnemyState.Attack;           //Wait -> Attack
                yield break;
            }
            current_waitTime += Time.deltaTime;
            if (waitTime < current_waitTime)
            {
                objectAnimator.SetBool("Wait", false);
                NEState = NormalEnemyState.Walk;             //Wait -> Walk
                yield break;
            }
            
            yield return null;
        }
    }


    protected virtual IEnumerator Attack()
    {
        isAttacking = true;

        //Attack Animation parameters
        objectAnimator.SetBool("Attack", true);
        objectAnimator.SetBool("Walk", false);
        objectAnimator.SetBool("Wait", false);
        objectAnimator.SetBool("isAttackActive", isAttackActive);

        //Cooltime Initialize
        Current_readyTime = 0;
        Current_cooltime = 0;

        yield return null;
    }

    protected IEnumerator Hit()
    {
        yield return null;
    }

    #region 구버전애니메이션 관리
    //    //Attack 애니메이션 1번만 돌리고 -> Idle로
    //    protected virtual IEnumerator AttackEnd()
    //    {

    //        clipInfo = objectAnimator.GetCurrentAnimatorClipInfo(0);
    //        Debug.Log(clipInfo[0].clip.name);


    //        float cliptime = clipInfo[0].clip.length;
    //        Debug.Log(cliptime);
    //        yield return new WaitForSeconds(cliptime-0.1f);

    //        yield return null;
    //        IsAttacking = false;


    //    }

    #endregion

    protected virtual IEnumerator Dead()
    {
        //Dead Animation parameters
        objectAnimator.SetTrigger("Dead");

        col.enabled = false;

        //애니메이션 시간때문에..대략
        yield return new WaitForSeconds(2.0f);

        //Fade Out
        StartCoroutine(fadeOut.FadeOut_Cor(spriteRenderer));

        Destroy(gameObject, 5.0f);

        yield return null;
    }


    //근거리/애니메이션 프레임에 설정  -->몸과 충돌시
    //원거리 -->탄환 충돌시
    protected virtual IEnumerator Attack_On() { yield return null; }



}
