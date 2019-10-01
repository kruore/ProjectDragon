using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Monster : Character
{
    public enum EnemyPos { None = 0, Front, Right, Left, Back };

    protected Animator objectAnimator;
    protected Rigidbody2D rigidbody;
    protected Vector3 direction;
    float angle;

    //아직 사용X
    protected EnemyPos enemyPos;
    public float moveDistance;

    BattleManager battleManager;


    //add 
    public string name;
    public int skillDamage;
    public float cooltime;
    public float skillCooltime;
    public float Attribute;
    enum KnockBackResistance { 상,중,하};



    //플레이어와 적과의 거리 캐스팅
    public float distanceOfPlayer;
    public float angleOfPlayer;

    float temp;
    float AngleCheck()
    {
        direction = (gameObject.transform.position - other.transform.position).normalized;
        angle = Vector3.Angle(direction, Vector3.up);
        if(direction.x < 0)   //↖
        {
            angle = -angle;
        }
        return angle;
    }




}
