using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Monster : Character
{

    protected Animator objectAnimator;
    protected Rigidbody2D rigidbody;
    protected SpriteRenderer spriteRenderer;


    [Header("[Enemy Attribute]")]
    public string name;
    [SerializeField] protected float waitTime;          //Idle->Walk time
    [SerializeField] protected float cooltime;          //Idle ->Attack time
    protected float Current_waitTime = 0;
    protected float Current_cooltime = 0;

    protected float KnockBackResistance;
    //public float Attribute;
    //enum KnockBackResistance { 상,중,하};

    [Header("[Rare Enemy Attribute]")]
    [SerializeField] protected float skillCooltime;     //상태2->상태1 time [Rare Type만]
    public int skillDamage;


    //Distance calculation
    [Header("[Distance calculation]")]
    [SerializeField] protected bool m_bDebugMode = false;      //테스트용
    public bool inAtkDetectionRange;
    protected Vector3 direction;
    protected RaycastHit2D hit;
    [SerializeField] protected float originOffset = 0.3f;
    [SerializeField]
    protected Vector2 startingPosition;
    [SerializeField]
    protected Vector3 directionOriginOffset;

    float Angle
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
            Angle = BattleManager.GetSideOfEnemyAndPlayerAngle(transform.position, other.transform.position);
        }


        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(hurt(1));
            StartCoroutine(hurt(1));
        }
    }

    Color _hideColor, _showColor;
    [SerializeField] bool isHit;
    float twinklingTime = 0;

    public IEnumerator hurt(int other_attackDamage)
    {
        Debug.Log("Enemy Hurt!");
        //Hp감소
        HPChanged(other_attackDamage);


        _showColor = _hideColor = spriteRenderer.color;
        _hideColor.a = 0.5f;
        _showColor.a = 1.0f;

        isHit = true;
        

        yield return new WaitForSeconds(0.25f);
        
        spriteRenderer.color = _hideColor;

        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = _showColor;

        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = _hideColor;

        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = _showColor;

        isHit = false;

        yield return null;



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
    [HideInInspector]
    public float distanceOfPlayer;
    [HideInInspector]
    public float angleOfPlayer;
    [HideInInspector]
    public float moveDistance;

}
