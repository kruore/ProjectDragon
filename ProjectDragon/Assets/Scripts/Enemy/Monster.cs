using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Monster : Character
{

    protected Animator objectAnimator;
    protected Rigidbody2D rigidbody;
    protected Vector3 direction;

    float Angle
    {
        get { return current_angle; }
        set
        {
            current_angle = value;
            objectAnimator.SetFloat("Angle", current_angle);
        }
    }
    public float moveDistance;

    //add 
    public string name;
    public float waitTime;          //Idle->Walk time
    public float cooltime;          //Idle ->Attack time
    public float skillCooltime;     //상태2->상태1 time [Rare Type만]
    public int skillDamage;
    public float Attribute;
    enum KnockBackResistance { 상,중,하};

    protected void FixedUpdate()
    {
        AngleCheck();
    }

    //플레이어와 적과의 거리 캐스팅
    public float distanceOfPlayer;
    public float angleOfPlayer;


    //플레이어를 바라보는 방향체크
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
