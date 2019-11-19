using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Monster : Character
{


    protected Rigidbody2D rb2d;
    protected SpriteRenderer spriteRenderer;
    protected Animator objectAnimator;






    [Header("[Enemy Attribute]")]
    //public string name;
    [SerializeField] protected float waitTime;          //Idle->Walk time
    [SerializeField] protected float cooltime;          //Idle ->Attack time
    protected float Current_waitTime = 0;
    protected float Current_cooltime = 0;
    [SerializeField] protected float knockTime = 0.15f;
    [SerializeField] protected float knockPower = 1.0f;
    //public float Attribute;



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
    protected FlashWhite flashWhite;
    protected DamagePopup damagePopup;
    protected GameObject childDustParticle;
    protected GameObject childDeadParticle;
    bool DustParticle_Actuation = false;

    protected override void Awake()
    {
        objectAnimator = gameObject.GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        other = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Effect
        fadeOut = GetComponent<FadeOut>();
        damagePopup = new DamagePopup();
        flashWhite = GetComponent<FlashWhite>();
        base.Awake();

     //   Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), true);
    }

    [HideInInspector]
    public float Angle
    {
        get { return current_angle; }
        set
        {
            current_angle = value;
            objectAnimator.SetFloat("Angle", current_angle);
        }
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

    public override int HPChanged(int ATK)
    {
        if (!isDead)
        {
            //데미지 띄우기
            damagePopup.Create(transform.position + new Vector3(0.0f, 0.5f, 0.0f), ATK, false, transform);
            return base.HPChanged(ATK);
        }
        return 0;
    }


    // 방향넉백
    public IEnumerator DirectionKnockBack()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(-direction* knockPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockTime);

        rb2d.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.2f);
        isHit = false;

        yield return null;
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

   

    ////플레이어를 바라보는 방향에 대한 각도체크
    //float AngleCheck() 
    //{
    //    direction = (other.transform.position-gameObject.transform.position).normalized;
    //    Angle = Vector3.Angle(direction, Vector3.up);
    //    if(direction.x < 0)   //↖
    //    {
    //        Angle = 360.0f - Angle;
    //    }
    //    return Angle;
    //}










    //삭제할것
    //플레이어와 적과의 거리 캐스팅

    public float distanceOfPlayer;
    [HideInInspector]
    public float angleOfPlayer;
    [HideInInspector]
    public float moveDistance;

}
