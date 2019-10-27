using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_NormalEnemy : Monster
{

    [Header(" ")]
    [SerializeField]
    protected bool isAttackActive;

    //개체의 상태가 바뀔때마다 실행
    protected override void SetState(State newState)
    {
        StartCoroutine(CurrentState.ToString());
    }

    protected IEnumerator Start_On()
    {
        //1초후 추적
        yield return new WaitForSeconds(1.0f);
        CurrentState = State.Walk;
        //공격감지 체크
        StartCoroutine(AttackRangeCheck());

        yield return null;
    }


    protected virtual IEnumerator None()
    {
        StartCoroutine(CalcCooltime());

        yield return null;

    }

    public virtual IEnumerator CalcCooltime()
    {
        
        while (true)
        {
            //[조건] cooltime > waitTime
            if (Current_cooltime < cooltime)                    //cooltime 전
            {
                if (Current_waitTime < waitTime)                 //waitTime 전
                {
                    Current_waitTime += Time.deltaTime;
                    Current_cooltime = Current_waitTime;
                }
                else                                             //waitTime 후
                {
                    //공격범위에 플레이어가 없다면 추적
                    if (!inAtkDetectionRange)
                    {
                        CurrentState = State.Walk;   //Idle->Walk
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



    //raycast
    protected  IEnumerator AttackRangeCheck()
    {
        while (true)
        {
            inAtkDetectionRange = CheckRaycast();
            
            yield return null;
        }
    }

    protected bool CheckRaycast()
    {
        inAtkDetectionRange = false;

        directionOriginOffset = originOffset * new Vector3(direction.x, direction.y, transform.position.z);
        startingPosition = transform.position + directionOriginOffset;

        hit = Physics2D.Raycast(startingPosition, direction, AtkRange, LayerMask.GetMask("Default"));

        if (hit.collider != null)
        {
            //Debug.Log("hit name :" + hit.collider.gameObject.name);
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                inAtkDetectionRange = true;
            }
        }
        return inAtkDetectionRange;
    }

    //Draw!!!! 테스트용
    private void OnDrawGizmos()
    {
        if (m_bDebugMode)
        {
            Debug.DrawRay(startingPosition, direction * AtkRange, Color.red);
            Gizmos.DrawWireSphere(transform.position, AtkRange + Vector2.Dot(directionOriginOffset, direction));
        }
    }

    

    protected virtual IEnumerator Walk()
    {
        //Walk Animation parameters
        objectAnimator.SetBool("Walk", true);

        while (CurrentState == State.Walk)
        {
            //공격감지범위에 들어오면 Attack
            if(inAtkDetectionRange)
            {
                isWalk = false;
                rb2d.velocity = Vector2.zero;
                CurrentState = State.Attack;
                yield break;
            }

            //test move
            if (!isHit)
            {
                isWalk = true;
                rb2d.velocity = direction * MoveSpeed * 10.0f * Time.deltaTime;
                //transform.position = Vector3.MoveTowards(transform.position, other.transform.position, MoveSpeed * Time.deltaTime);
                //StartCoroutine(aStar.FindPathAgain(rigidbody, direction, MoveSpeed));
            }

            yield return null;
        }
    }

    protected virtual IEnumerator Attack()
    {
        IsAttacking = true;

        //Attack Animation parameters
        objectAnimator.SetBool("Walk", false);
        objectAnimator.SetBool("isAttackActive", isAttackActive);

        //Cooltime Initialize
        Current_waitTime = 0;
        Current_cooltime = 0;

        yield return null;
        StartCoroutine(AttackEnd());

    }



    //Attack 애니메이션 1번만 돌리고 -> Idle로
    protected virtual IEnumerator AttackEnd()
    {
        yield return null;

        clipInfo = objectAnimator.GetCurrentAnimatorClipInfo(0);
        Debug.Log(clipInfo[0].clip.name);


        float cliptime = clipInfo[0].clip.length;
        Debug.Log(cliptime);
        yield return new WaitForSeconds(cliptime);

        IsAttacking = false;


    }

    protected IEnumerator Dead()
    {
        if (isDead)
        {
            //Dead Animation parameters
            objectAnimator.SetTrigger("Dead");

            //Fade Out
            StartCoroutine(fadeOut.FadeOut_Cor(spriteRenderer));
        }
        yield return null;
    }

    public override int HPChanged(int ATK)
    {
        if (!invincible)
        {
            isHit = true;

            //넉백
            StartCoroutine(DirectionKnockBack());

            //White Shader
            StartCoroutine(flashWhite.Flash());

            return base.HPChanged(ATK);
        }

        return 0;
    }


    //근거리/애니메이션 프레임에 설정  -->몸과 충돌시
    //원거리 -->탄환 충돌시
    protected virtual IEnumerator Attack_On() { yield return null; }



}
