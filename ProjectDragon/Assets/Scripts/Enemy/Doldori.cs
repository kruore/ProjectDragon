﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doldori : FSM_NormalEnemy
{

    protected override void Awake()
    {
        base.Awake();
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall" , "Cliff"); // 근거리는 Cliff 추가
        childDustParticle = transform.Find("DustParticle").gameObject;
    }

    void Update()
    {
        DustParticleCheck();

    }

    Vector3 attackDirection;
    protected override IEnumerator Attack()
    {
        AttackStart();
        //무적
        invincible = true;
        yield return new WaitForSeconds(1.5f);

        //Attacking
        isAttacking = true;
        objectAnimator.Play("Attacking");
        attackDirection = direction;
        while (isAttacking)
        {
            rb2d.AddForce(attackDirection * 0.2f, ForceMode2D.Impulse);
            yield return null;
        }
    }

    protected override void Attack_On()
    {
        if (!isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE);
        }
    }


    //부딪히면 Attack-> Idle로
    protected override IEnumerator AttackEnd()
    {
        isAttacking = false;
        invincible = false;
        rb2d.velocity = Vector2.zero;

        StartCoroutine(DirectionKnockBack(attackDirection, 0.5f, 1.5f));

        //Attack Animation parameters
        objectAnimator.SetBool("Attack", isAttacking);
        NEState = NormalEnemyState.Idle;
        yield return null;

        AttackEndCor = null;
    }



    IEnumerator AttackEndCor=null;
    //벽과 플레이어 부딪히면 그로기 상태
    //protected void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //연속으로 될경우 방지 (한번만 돌리게)
    //    if (NEState == NormalEnemyState.Attack&& AttackEndCor == null)
    //    {
    //        if(collision.gameObject.CompareTag("Player")||collision.gameObject.CompareTag("Wall"))
    //        {
    //            if (collision.gameObject.CompareTag("Player"))
    //            {
    //                Attack_On();
    //            }
    //            AttackEndCor = AttackEnd();
    //            StartCoroutine(AttackEnd());
    //        }
    //    }
    //}
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        //연속으로 될경우 방지 (한번만 돌리게)
        if (NEState == NormalEnemyState.Attack && AttackEndCor == null)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall")
                || collision.gameObject.CompareTag("Cliff")|| collision.gameObject.CompareTag("Object"))
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    Attack_On();
                }
                /////////////////////////////////////////////////////나중에
                //else if(collision.gameObject.CompareTag("Object")&&collision.gameObject.GetComponent<Box>().hp)
                //{

                // }
                AttackEndCor = AttackEnd();
                StartCoroutine(AttackEnd());
            }
        }
    }
    protected override void OnCollisionExit2D(Collision2D collision)
    {
    } 
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (isAttacking)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                //충돌할때 Attack이면 콜라이더끄기
                Physics2D.IgnoreCollision(collision, col);
            }
        }
    }
}
