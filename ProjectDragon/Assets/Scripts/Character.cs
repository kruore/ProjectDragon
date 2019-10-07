using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { None = 0, Walk, Attack, Dead, Skill, Hit};
public enum AnglePos
{
   None = 0, Front, Right, Back, Left
}
public enum AttackType { None = 0, LongRange, MiddleRange, ShortRange };
public class Character : MonoBehaviour, PersonalSpecificational
{
    //state of animation
    //state of Attack type for range


    //personal Specification
    [SerializeField] private int hp;
    [SerializeField] private int atk;
    [SerializeField] private float atkSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float atkRange;
    public Vector3 myPos;
    public Vector3 myRotat;
    public int current_Anim_Frame; //@
    public float current_angle;
    public float enemy_angle; //@

    // TODO : 이건 적이나 플레이어에게만 규정할 것 (스킬이 존재하고 있는)
    //[SerializeField] private float skillCoolDown;
    //[SerializeField] private float skillRange;

    public State myState;
    public AttackType myAttackType; //@
    public AnglePos myAnim_AnglePos;  //@


    //@
    [SerializeField] protected bool isAttacking;
    [SerializeField] protected bool isWalk;
    [SerializeField] protected bool isDead;
    [SerializeField] protected bool isHit;
    [SerializeField] protected bool isSkillActive;


    public Transform other;


    //@
    public bool DistanceCheck(float closeDistance)
    {
        if (other)
        {
            Vector3 offset = other.position - transform.position;
            float sqrLen = offset.sqrMagnitude;

            if (sqrLen < (closeDistance * closeDistance))
            {
                return isAttacking = true;
            }
        }
        return isAttacking = false;
    }
    #region ATKSPEED
    public float ATTACKSPEED
    {
        get
        {
            return atkSpeed;
        }
        set
        {
            atkSpeed = value;
        }
    }
    public float ATKSpeedChanger(float AttackSpeed)
    {
        atkSpeed = atkSpeed + AttackSpeed;
        return atkSpeed;
    }
    #endregion
    #region ATK
    public int ATTACKDAMAGE
    {
        get
        {
            return atk;
        }
        set
        {

            atk = value;
        }
    }
    public int ATKChanger(int attackDamage)
    {
        atk = atk + attackDamage;
        return atk;
    }
    #endregion
    #region HPControll
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            if (value <= 0)
            {
                Debug.Log("죽었습니다.");
                myState = State.Dead;
            }
            else
            {
                hp = value;
            }
        }
    }
    public int HPChanged(int ATK)
    {
        hp = hp - ATK;
        return hp;
    }
    #endregion
    #region MoveSpeed
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }
    public float MoveSpeedChanger(float MoveSpeed)
    {
        moveSpeed = moveSpeed - MoveSpeed;
        return moveSpeed;
    }
    #endregion
    #region AtkRange
    public float AtkRange
    {
        get
        {
            return atkRange;
        }
        set
        {
            atkRange = value;
        }
    }
    public float AtkRangeChanger(float AtkRange)
    {
        atkRange = AtkRange;
        return atkRange;
    }
    #endregion


    //@
    //공격을 할때 각도에 따라서 모션을 보여주기 위해 만듬 (즉, 적이 있을때만 사용)
    public AnglePos Current_AngleCaseString(float angle)
    {
        if (angle == 0)
        {
            return AnglePos.Front;
        }
        if (angle < 45)
        {
            return AnglePos.Back;
        }
        else if (angle < 135)
        {
            return AnglePos.Right;
        }
        else if (angle < 225)
        {
            return AnglePos.Front;
        }
        else if (angle < 315)
        {
            return AnglePos.Left;
        }
        return AnglePos.Back;
    }
    public string Enemy_AngleCaseString(float angle)
    {
        if (angle < 45)
        {
            return "Front";
        }
        else if (angle < 135)
        {
            return "Left";
        }
        else if (angle < 225)
        {
            return "Back";
        }
        else if (angle < 315)
        {
            return "Right";
        }
        return "Front";
    }

    public AnglePos Current_AngleCaseString2(float _angle)
    {
        if(-45 <= _angle && _angle <= 45)    
        {
            return AnglePos.Front;
        }
        else if(45 < _angle && _angle < 135)  
        {
            return AnglePos.Right;
        }
        else if(-135 < _angle && _angle <-45)
        {
            return AnglePos.Left;
        }
        else
        {
            return AnglePos.Back;
        }
    }

    public void AnimatorCast(string animationtype)
    {
        gameObject.GetComponent<Animator>().Play(animationtype);
    }
}