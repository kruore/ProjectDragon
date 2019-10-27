using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doldori : FSM_NormalEnemy
{

    protected override void Awake()
    {
        base.Awake();
        childDustParticle = transform.Find("DustParticle").gameObject;
    }

    protected override void Start()
    {
        StartCoroutine(Start_On());
    }

    void Update()
    {
        DustParticleCheck();

        if (isAttacking)
        {
            //무적상태
            invincible = true;
        }
        else
        {
            invincible = false;
        }

        //test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HPChanged(1);
        }
    }
    protected override IEnumerator Attack()
    {
        //플레이어 방향으로 돌진
        rb2d.AddForce(direction * 3.0f, ForceMode2D.Impulse);

        yield return base.Attack();
    }

    //벽 or 플레이어 부딪힌후 -> Idle로
    protected override IEnumerator AttackEnd()
    {
        yield return null;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CurrentState == State.Attack)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                IsAttacking = false;
                rb2d.velocity = Vector2.zero;
            }
        }
        //if(collision.gameObject.CompareTag("Wall"))
    }


    protected override IEnumerator Attack_On()
    {
        yield return null;

    }

}
