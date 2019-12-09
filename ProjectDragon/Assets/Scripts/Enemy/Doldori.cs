using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doldori : FSM_NormalEnemy
{
    [SerializeField]
    bool invincible = false;  //무적상태인지


    protected override void Awake()
    {
        base.Awake();
        childDustParticle = transform.Find("DustParticle").gameObject;
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

    //애니메이션 프레임에 넣기
    protected override IEnumerator Attack_On()
    {
        if (inAtkDetectionRange && !isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE);
        }
        yield return null;
    }
    //부딪히면 Attack-> Idle로
    protected virtual IEnumerator AttackEnd()
    {

    }


    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (NEState == NormalEnemyState.Attack)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isAttacking = false;
                rb2d.velocity = Vector2.zero;
            }
        }
        //if(collision.gameObject.CompareTag("Wall"))
    }



}
