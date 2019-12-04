using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Monster
{
    protected Rigidbody2D rb2d;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] LayerMask m_viewTargetMask; // 인식 가능한 타켓의 마스크
    protected BoxCollider2D col;
    [SerializeField]  protected bool collisionPlayer = false;
   

    [Header("[Enemy Attribute]")]
    //public string name;
    [SerializeField] protected float readyTime;          //Idle->Walk time
    [SerializeField] protected float cooltime;          //Idle ->Attack time
    protected float Current_readyTime = 0;
    protected float Current_cooltime = 0;
    [SerializeField] protected float knockTime = 0.15f;
    [SerializeField] protected float knockPower = 1.0f;


    [Header("[Rare Enemy Attribute]")]
    [SerializeField] protected float skillCooltime;     //상태2->상태1 time [Rare Type만]
    public int skillDamage;


    //Distance calculation
    [Header("[Distance calculation]")]
    [SerializeField] protected bool m_bDebugMode = false;      //테스트용
    public bool inAtkDetectionRange;
    protected Vector3 direction;
    protected RaycastHit2D[] hit;
    [SerializeField] protected float originOffset;
    protected Vector2 startingPosition;
    protected Vector3 directionOriginOffset;


    //Effect
    protected FadeOut fadeOut;
    protected GameObject childDustParticle;
    protected GameObject childDeadParticle;
    bool DustParticle_Actuation = false;

    public float Angle
    {
        get { return current_angle; }
        set
        {
            current_angle = value;
            objectAnimator.SetFloat("Angle", current_angle);
        }
    }


    protected override void Awake()
    {
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall");

        col = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        other = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Effect
        fadeOut = GetComponent<FadeOut>();

        base.Awake();
    }

    protected void FixedUpdate()
    {
        direction = (other.transform.position - gameObject.transform.position).normalized;

        //플레이어를 바라보는 방향에 대한 각도체크
        if (CurrentState != State.Attack)
        {
            Angle = BattleManager.GetSideOfEnemyAndPlayerAngle(transform.position, GetComponent<Tracking>().currentWaypoint);
        }
    }

    //개체의 상태가 바뀔때마다 실행
    protected void SetEnemyState<T>(T _state)
    {
        StartCoroutine(_state.ToString());
    }
    RoomManager RoomManager;
    public virtual IEnumerator Start_On()
    {
        //Grid 생성
        GetComponent<Tracking>().pathFinding.Create(col.size.x, col.size.y, transform.GetComponentInParent<t_Grid>());

         RoomManager = GameObject.FindWithTag("RoomManager").GetComponent<RoomManager>();

        yield return null;
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


        hit = Physics2D.RaycastAll(startingPosition, direction, AtkRange - originOffset, m_viewTargetMask);

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
            Debug.DrawRay(startingPosition, direction * (AtkRange - originOffset), Color.red);
            Gizmos.DrawWireSphere(transform.position, AtkRange);
        }
    }
    //+ Vector2.Dot(directionOriginOffset, direction)



    private void OnCollisionStay2D(Collision2D collision)
    {
        //Player에게 다가가는 무리들에 대한 이동조정.. (walk)
        if (collision.gameObject.CompareTag("Player") ||
             (collision.gameObject.CompareTag("Enemy") && (collision.gameObject.GetComponent<Enemy>().collisionPlayer)))
        {
            collisionPlayer = true;
            if ( collisionPlayer)
            {
                rb2d.isKinematic = true;
                rb2d.velocity = Vector2.zero;
            } 
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Enemy"))
        {
            rb2d.isKinematic = false;
            collisionPlayer = false;
        }
    }

    public bool EnemeisHit=false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isHit)
            {
                EnemeisHit = true;
                Physics2D.IgnoreCollision(collision, col);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Walk이면 Object 충돌무시(Astar)
        if (isWalk && (collision.gameObject.CompareTag("Object") || collision.gameObject.CompareTag("Wall")))
        {
            Physics2D.IgnoreCollision(collision, col);
        }
        //Hit중이면 Enemy 충돌무시
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isHit)
            {
                EnemeisHit = true;
                Physics2D.IgnoreCollision(collision, col);
            }
            else
            {
                EnemeisHit = false;
                Physics2D.IgnoreCollision(collision, col, false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Object") || collision.gameObject.CompareTag("Wall"))
        {
            Physics2D.IgnoreCollision(collision, col, false);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemeisHit = false;
            Physics2D.IgnoreCollision(collision, col,false);
        }
    }

    protected IEnumerator KnockBackCor;
    // 방향넉백
    public IEnumerator DirectionKnockBack()
    {
        rb2d.AddForce(-direction * knockPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockTime);

        rb2d.velocity = Vector2.zero;
        isHit = false;
    }

    //Dust Particle
    protected void DustParticleCheck()
    {
        if (!isDead)
        {
            DustParticle_Actuation = isHit || isWalk ? true : false;
            childDustParticle.SetActive(DustParticle_Actuation);
        }
        else
        {
            childDustParticle.SetActive(false);
        }
    }


}
