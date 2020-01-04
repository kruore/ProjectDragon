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
        return Physics2D.CircleCastAll(startingPosition, circleCol.radius, direction, AtkRange - originOffset- circleCol.radius, m_viewTargetMask);
    }

    protected override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        DustParticleCheck();
    }


    // In this case you choose event based on the clip weight
    public void Attack_On(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f)
        {
            rb2d.velocity = Vector2.zero;
            isNuckback = true;

            if (inAtkDetectionRange && !isDead)
            {
                //Player hit
                //other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE);
            }
        }
    }
    public Vector3 previousAttackPos;
    // In this case you choose event based on the clip weight
    public void AttackEndFrame(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f)
        {
            //transform.position = previousAttackPos;
        }
    }

    protected override IEnumerator Attack()
    {
        previousAttackPos = transform.position;
        Debug.Log("previousAttackPos    " +  previousAttackPos.ToString());
        //StartCoroutine(RushAttack());
        isNuckback = false;

        StartCoroutine(base.Attack());
        yield return null;
    }
    IEnumerator RushAttack()
    {
        Debug.Log("Slime RushAttack");
        rb2d.AddForce(direction * 3.0f, ForceMode2D.Impulse);
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