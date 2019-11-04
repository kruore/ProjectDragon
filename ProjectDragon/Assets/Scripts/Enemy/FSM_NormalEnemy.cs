using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_NormalEnemy : Monster
{

    [Header(" ")]
    [SerializeField]
    protected bool isAttackActive;
    [SerializeField] LayerMask m_viewTargetMask; // 인식 가능한 타켓의 마스크
    protected BoxCollider2D col;
    [SerializeField]
    public bool isCollision = false;

    protected override void Awake()
    {
        base.Awake();
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall");
        col = GetComponent<BoxCollider2D>();
    }

    //개체의 상태가 바뀔때마다 실행
    protected override void SetState(State newState)
    {
        StartCoroutine(CurrentState.ToString());
    }

    public override int HPChanged(int ATK)
    {
        //살아 있을때
        if (!isDead)
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

    public IEnumerator Start_On()
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
        objectAnimator.SetBool("Attack", false);

        StartCoroutine(CalcCooltime());

        yield return null;

    }

    public virtual IEnumerator CalcCooltime()
    {

        while (CurrentState == State.None)
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
                        isAttackActive = false;
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
    protected IEnumerator AttackRangeCheck()
    {
        //살아있을때만 raycast 체크
        while (!isDead)
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

        #region int layerMask 숫자로 변환 해두기..

        ////layerMask = ~layerMask;   //이런것도 있다. 
        ////int layerMask = (1 << ;player_Layer_num; | 1 << ;player_Layer_num; );
        ////int layerMask = (1 << 8) | (1 << 13) | (1 << 12);

        //int layerMask = (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Wall"));

        #endregion


        hit = Physics2D.RaycastAll(startingPosition, direction, AtkRange, m_viewTargetMask);

        foreach (RaycastHit2D _hit in hit)
        {
            if (_hit.collider != null)
            {
                //Debug.Log("hit name :" + _hit.collider.gameObject.name);
                if (_hit.collider.gameObject.CompareTag("Wall"))
                {
                    break;
                }
                if (_hit.collider.gameObject.CompareTag("Player"))
                {
                    inAtkDetectionRange = true;
                    break;
                }
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
            if (inAtkDetectionRange)
            {
                isWalk = false;
                rb2d.velocity = Vector2.zero;
                CurrentState = State.Attack;
                yield break;
            }

            //test move
            if (!isHit && !isCollision)
            {
                isWalk = true;
                rb2d.velocity = direction * MoveSpeed * 10.0f * Time.deltaTime;
                //transform.position = Vector3.MoveTowards(transform.position, other.transform.position, MoveSpeed * Time.deltaTime);
                //StartCoroutine(aStar.FindPathAgain(rigidbody, direction, MoveSpeed));
            }

            yield return null;
        }
    }


    //충돌했을때 서로 콜라이더로 밀지 않게 
    //protected void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player")|| (collision.gameObject.GetComponent<FSM_NormalEnemy>().isCollision))
    //    {
    //        isCollision = true;
    //        rb2d.bodyType = RigidbodyType2D.Kinematic;
    //        rb2d.velocity = Vector2.zero;
    //    }
    //}
    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") ||
            (collision.gameObject.CompareTag("Enemy") && (collision.gameObject.GetComponent<FSM_NormalEnemy>().isCollision)))
        {
            isCollision = true;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.velocity = Vector2.zero;
        }
  
    }
    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") ||
             (collision.gameObject.CompareTag("Enemy") && (collision.gameObject.GetComponent<FSM_NormalEnemy>().isCollision)))
        {
            Debug.Log("떨어짐");
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            isCollision = false;
        }
    }

    protected virtual IEnumerator Attack()
    {
        isAttacking = true;

        //Attack Animation parameters
        objectAnimator.SetBool("Attack", true);
        objectAnimator.SetBool("Walk", false);
        objectAnimator.SetBool("isAttackActive", isAttackActive);

        //Cooltime Initialize
        Current_waitTime = 0;
        Current_cooltime = 0;

        yield return null;
        

    }


    #region 구버전애니메이션 관리
    //    //Attack 애니메이션 1번만 돌리고 -> Idle로
    //    protected virtual IEnumerator AttackEnd()
    //    {

    //        clipInfo = objectAnimator.GetCurrentAnimatorClipInfo(0);
    //        Debug.Log(clipInfo[0].clip.name);


    //        float cliptime = clipInfo[0].clip.length;
    //        Debug.Log(cliptime);
    //        yield return new WaitForSeconds(cliptime-0.1f);

    //        yield return null;
    //        IsAttacking = false;


    //    }

    #endregion

    protected virtual IEnumerator Dead()
    {

        //Dead Animation parameters
        objectAnimator.SetTrigger("Dead");

        col.enabled = false;

        //애니메이션 시간때문에..대략
        yield return new WaitForSeconds(2.0f);

        //Fade Out
        StartCoroutine(fadeOut.FadeOut_Cor(spriteRenderer));

        Destroy(gameObject, 5.0f);

        yield return null;
    }


    //근거리/애니메이션 프레임에 설정  -->몸과 충돌시
    //원거리 -->탄환 충돌시
    protected virtual IEnumerator Attack_On() { yield return null; }



}
