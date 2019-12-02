using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : FSM_NormalEnemy
{
    protected override void Awake()
    {
        base.Awake();
        childDustParticle = transform.Find("DustParticle").gameObject;
        childDeadParticle = transform.Find("DeadParticle").gameObject;
    }

    protected override void Start()
    {
        atk = 0;
        base.Start();
        //StartCoroutine(Start_On());
    }

    public void Update()
    {
        DustParticleCheck();

    }

    //애니메이션 프레임에 넣기
    protected override IEnumerator Attack_On()
    {
        if (inAtkDetectionRange&&!isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE);
        }
        yield return null;

    }

    protected override IEnumerator Dead()
    {
        Color fadeColor=spriteRenderer.color;
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