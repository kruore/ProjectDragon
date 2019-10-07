using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Monster : Character
{

    protected Animator objectAnimator;
    protected Rigidbody2D rigidbody;

    //Distance calculation
    [Header("Distance calculation")]
    [SerializeField] protected bool m_bDebugMode = false;      //테스트용
    public bool inAtkRange;
    protected Vector3 direction;
    protected RaycastHit2D hit;
    [SerializeField] protected float originOffset = 0.3f;
    protected Vector2 startingPosition;
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
  

    [Header("Enemy Attribute")]
    public string name;
    [SerializeField] protected float waitTime;          //Idle->Walk time
    [SerializeField] protected float cooltime;          //Idle ->Attack time
    protected float Current_waitTime = 0;
    protected float Current_cooltime = 0;
    [SerializeField] protected float skillCooltime;     //상태2->상태1 time [Rare Type만]
    public int skillDamage;
    //public float Attribute;
    //enum KnockBackResistance { 상,중,하};

    protected void FixedUpdate()
    {
        AngleCheck();
    }


    //삭제할것
    //플레이어와 적과의 거리 캐스팅
    public float distanceOfPlayer;
    public float angleOfPlayer;
    public float moveDistance;


    //플레이어를 바라보는 방향에 대한 각도체크
    float AngleCheck() 
    {
        direction = (other.transform.position-gameObject.transform.position).normalized;
        Angle = Vector3.Angle(direction, Vector3.up);
        if(direction.x < 0)   //↖
        {
            Angle = 360.0f - Angle;
        }
        return Angle;
    }

}
