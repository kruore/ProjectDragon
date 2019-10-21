﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Monster : Character
{

    protected Animator objectAnimator;
    protected Rigidbody2D rigidbody;
    protected SpriteRenderer spriteRenderer;


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
    protected RaycastHit2D hit;
    [SerializeField] protected float originOffset = 0.3f;
    protected Vector2 startingPosition;
    protected Vector3 directionOriginOffset;

    //Effect
    FlashWhite flashWhite;
    DamagePopup damagePopup;
   

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
    }

    public IEnumerator hurt(int other_attackDamage)
    {
        Debug.Log("Enemy Hurt!");

        isHit = true;

        //Hp 감소
        HPChanged(other_attackDamage);

        //넉백
        StartCoroutine(DirectionKnockBack());

        //White Shader
        flashWhite = GetComponent<FlashWhite>();
        StartCoroutine(flashWhite.Flash());


        //데미지 띄우기
        damagePopup = new DamagePopup();
        damagePopup.Create(transform.position, other_attackDamage,false,transform);
        yield return null;

    }

    // 방향넉백
    public IEnumerator DirectionKnockBack()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(-direction* knockPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockTime);

        rigidbody.velocity = Vector2.zero;

        isHit = false;

        yield return null;
    }
    //protected GameObject childDustParticle;
    bool DustParticle_Actuation = false;
    DustParticleController DustParticleController;

    //Dust Particle
    protected void DustParticleCheck()
    {
        DustParticle_Actuation = isHit || isWalk ? true : false;
        //childDustParticle.SetActive(DustParticle_Actuation);


        DustParticleController = GetComponentInChildren<DustParticleController>();
        DustParticleController.DustParticleCheck(DustParticle_Actuation, isHit);

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
