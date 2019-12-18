/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-18////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : FSM_NormalEnemy
{
    BoxCollider2D boxCol;
    protected override void Awake()
    {
        base.Awake();
        boxCol = GetComponent<BoxCollider2D>();
        col = boxCol;
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall", "Cliff"); // 근거리는 Cliff 추가
        childDustParticle = transform.Find("DustParticle").gameObject;
        childDeadParticle = transform.Find("DeadParticle").gameObject;
    }

    protected override RaycastHit2D[] GetRaycastType()
    {
        //BoxCast
        return Physics2D.BoxCastAll(startingPosition, boxCol.size, 0, direction, AtkRange - originOffset, m_viewTargetMask);
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
    }

    //임시
    protected override IEnumerator Dead()
    {
        Color fadeColor = spriteRenderer.color;
        fadeColor.a = 0.0f;

        //Dead Animation parameters
        objectAnimator.SetTrigger("Dead");

        col.enabled = false;

        //애니메이션 시간때문에..대략
        yield return new WaitForSeconds(2.0f);

        DeadParticle();
        spriteRenderer.color = fadeColor;  

        Destroy(gameObject, 5.0f);

        yield return null;
    }

    void DeadParticle()
    {
        childDeadParticle.SetActive(true);
    }

}