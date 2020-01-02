﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RimmotalEnemyState { Idle, Walk, Attack1, Attack2, }
public class Rimmotal : Enemy
{
    [Header("[Enemy State]")]
    [SerializeField] protected RimmotalEnemyState rimmotalEnemyState;
    CapsuleCollider2D capsuleCol;

    public RimmotalEnemyState REState
    {
        get { return rimmotalEnemyState; }
        set
        {
            rimmotalEnemyState = value;
            SetState(rimmotalEnemyState);

        }
    }
    protected override void Awake()
    {
        base.Awake();
        capsuleCol = GetComponent<CapsuleCollider2D>();
        col = capsuleCol;
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall", "Cliff"); // 근거리는 Cliff 추가
        childDustParticle = transform.Find("DustParticle").gameObject;
        thornTargeting = new ThornTargeting();
    }
    protected override RaycastHit2D[] GetRaycastType()
    {
        //float maxSizeAxis = capsuleCol.size.x < capsuleCol.size.y ? capsuleCol.size.y : capsuleCol.size.x;

        //CapsuleCas
        return Physics2D.CapsuleCastAll(startingPosition, capsuleCol.size, CapsuleDirection2D.Vertical, 0, direction, AtkRange - originOffset/*- (maxSizeAxis*0.5f)*/, m_viewTargetMask);
    }


    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator Start_On()
    {
        StartCoroutine(base.Start_On());

        //1초후 추적
        yield return new WaitForSeconds(1.0f);
        REState = RimmotalEnemyState.Walk;

        //공격감지 체크
        StartCoroutine(AttackRangeCheck());
        yield return null;
    }

    public override void Dead()
    {
        base.Dead();
    }



    /********************************************************************/


    IEnumerator Idle()
    {
        Debug.Log("Idle");

        yield return new WaitForSeconds(1.0f);
        if (inAtkDetectionRange)
        {
            REState = RimmotalEnemyState.Attack1;
        }
        else
        {
            REState = RimmotalEnemyState.Walk;
        }

    }

    IEnumerator Walk()
    {
        Debug.Log("Walk");
        //Walk Animation parameters
        objectAnimator.SetBool("Walk", true);
        while (REState == RimmotalEnemyState.Walk && !isDead)
        {
            if (isHit)
            {
                yield return null;
                continue;
            }

            //공격감지범위에 들어오면 Attack
            if (inAtkDetectionRange)
            {
                isWalk = false;
                REState = RimmotalEnemyState.Attack1;
                objectAnimator.SetBool("Walk", false);
                yield break;
            }

            if (rb2d.velocity != Vector2.zero)
            {
                PushStopCor = PushStop();
                StartCoroutine(PushStopCor);
            }
            else
            {
                //move
                if (!collisionPlayer)
                {
                    isWalk = true;
                    //AStar
                    GetComponent<Tracking>().FindPathManager(rb2d, MoveSpeed);
                }
                else { isWalk = false; }
            }
            yield return null;
        }
    }

     void Attack1_On()
    {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE,0,false);
    }
    void Attack2_On()
    {

            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE, 1, true);
    }

    IEnumerator Attack1()
    {
        Debug.Log("Attack1");

        //Attack Animation parameters
        objectAnimator.SetBool("Attack1", true);

        while(!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            yield return null;
        }

        isAttacking = true;

        AnimatorClipInfo[] clipInfo = objectAnimator.GetCurrentAnimatorClipInfo(0);
        float cliptime = clipInfo[0].clip.length;
        yield return new WaitForSeconds(cliptime / objectAnimator.GetCurrentAnimatorStateInfo(0).speed);

        REState = RimmotalEnemyState.Attack2;
        objectAnimator.SetBool("Attack1", false);
    }

    IEnumerator Attack2()
    {
        Debug.Log("Attack2");

        //Attack Animation parameters
        objectAnimator.SetBool("Attack2", true);
        IsFix = true;
      
        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rimmotal_Burrow"))
        {
            yield return null;
        }

        float cliptime = objectAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(cliptime/ objectAnimator.GetCurrentAnimatorStateInfo(0).speed);

        StartCoroutine(CoolTimeCheck());
        yield return StartCoroutine(ThornAttack());
        objectAnimator.SetBool("Attack2", false);

        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rimmotal_Restore"))
        {
            yield return null;
        }

        float cliptime1 = objectAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(cliptime1 / objectAnimator.GetCurrentAnimatorStateInfo(0).speed);

        REState = RimmotalEnemyState.Idle;

    }

    ThornTargeting thornTargeting;
    bool _thorn_attacking = true;

    /// <summary>
    /// 가시공격
    /// </summary>
    IEnumerator ThornAttack()
    {
        Debug.Log("Thorn Create Ready");
        _thorn_attacking = true;

        while (_thorn_attacking)
        {
            if (inAtkDetectionRange)
            {
                thornTargeting.Create(skillDamage, "ThornTargeting", other.position);
                yield return new WaitForSeconds(2.0f);
            }
            yield return null;
        }
    }
    /// <summary>
    /// 공격2 쿨타임 검사
    /// </summary>
    /// <returns></returns>
    IEnumerator CoolTimeCheck()
    {
        yield return new WaitForSeconds(6.0f);
        _thorn_attacking = false;

    }

}
