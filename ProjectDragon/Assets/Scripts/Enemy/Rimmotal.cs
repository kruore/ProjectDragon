using System.Collections;
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
    }
    protected override RaycastHit2D[] GetRaycastType()
    {
        //CapsuleCas
        return Physics2D.CapsuleCastAll(startingPosition, capsuleCol.size, CapsuleDirection2D.Vertical, 0, direction, AtkRange - originOffset, m_viewTargetMask);
    }


    protected override void Start()
    {
        base.Start();
        //StartCoroutine(Start_On());
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

    IEnumerator Walk()
    {
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
        if (!isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE,0,false);
        }
    }
    void Attack2_On()
    {
        if (!isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE,1,true);
        }
    }

    IEnumerator Attack1()
    {
        yield return null;

        //Attack Animation parameters
        objectAnimator.SetBool("Attack1", true);
        objectAnimator.SetBool("Walk", false);

        AnimatorClipInfo[] clipInfo = objectAnimator.GetCurrentAnimatorClipInfo(0);
        //Debug.Log(clipInfo[0].clip.name);

        float cliptime = clipInfo[0].clip.length;
        yield return new WaitForSeconds(cliptime);

        REState = RimmotalEnemyState.Attack2;
        yield return null;


    }

    IEnumerator Attack2()
    {
        //Attack Animation parameters
        objectAnimator.SetBool("Attack2", true);
        isFix = true;
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX; //밀림방지
        float cliptime=objectAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(cliptime);

        ThornAttack();

        yield return null;
    }
    /// <summary>
    /// 가시공격
    /// </summary>
    void ThornAttack()
    {

    }

}
