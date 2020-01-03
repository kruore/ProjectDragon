/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-18////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : FSM_NormalEnemy
{
    CircleCollider2D circleCol;
    protected override void Awake()
    {

        base.Awake();
        circleCol = GetComponent<CircleCollider2D>();
        col = circleCol;
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall", "Cliff"); // 근거리는 Cliff 추가
        childDustParticle = transform.Find("DustParticle").gameObject;
        childDeadParticle = transform.Find("SlimeDead_Particle").gameObject;
    }

    protected override RaycastHit2D[] GetRaycastType()
    {
        //CircleCast
        return Physics2D.CircleCastAll(startingPosition, circleCol.radius, direction, AtkRange - originOffset, m_viewTargetMask);
    }

    protected override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        DustParticleCheck();
    }

    //애니메이션 프레임에 넣기
    protected override void Attack_On()
    {
        if (inAtkDetectionRange&&!isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE);
        }
            Debug.Log("Enemy Attack_On");
    }
    protected override IEnumerator Attack()
    {
        StartCoroutine(base.Attack());
        yield return null;
    }

    //임시
    protected override IEnumerator EnemyDead()
    {
        Color fadeColor = spriteRenderer.color;
        fadeColor.a = 0.0f;

        //Dead Animation parameters
        objectAnimator.SetTrigger("Dead");

        col.enabled = false;
        DeadParticle();

        //애니메이션 시간때문에..대략
        yield return new WaitForSeconds(2.0f);

        spriteRenderer.color = fadeColor;  

        Destroy(gameObject, 5.0f);

        yield return null;
    }


    //Vector3 PosBeforeAttack;
    //IEnumerator RushAttack()
    //{
    //    PosBeforeAttack = transform.position;
    //    rb2d.AddForce(direction * 1.0f, ForceMode2D.Impulse);
    //    yield return new WaitForSeconds(0.3f);

    //    rb2d.velocity = Vector2.zero;

    //    yield return null;

    //}

    //void AttackEndFrame()
    //{
    //    transform.position = PosBeforeAttack;
    //}
    void DeadParticle()
    {
        childDeadParticle.SetActive(true);
        childDeadParticle.GetComponent<ParticleSystem>().Play();
    }

}