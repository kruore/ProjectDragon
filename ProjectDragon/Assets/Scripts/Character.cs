using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { None = 0, Walk, Attack, Dead, Skill, Hit}
public enum AnglePos { None = 0, Front, Right, Back, Left }
public enum AttackType { None = 0, LongRange, MiddleRange, ShortRange }


public class Character : MonoBehaviour, PersonalSpecificational
{
    [SerializeField]
    private State myState;
    public State CurrentState
    {
        get { return myState; }
        set
        {
            myState = value;
            SetState(myState);
        }
    }
    
    
    //personal Specification
    [SerializeField] protected int hp;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int atk;
    [SerializeField] protected float atkSpeed;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float atkRange;
    public Vector3 myPos;
    public Vector3 myRotat;
    public float current_angle;

    public Transform other;

    protected bool isAttacking;
    protected bool isWalk;
    protected bool isDead;
    protected bool isHit;
    protected bool isSkillActive;



    #region ATKSPEED
    public float ATTACKSPEED
    {
        get { return atkSpeed; }
        set
        {
            atkSpeed = value;
        }
    }

    public float ATKSpeedChanger(float _attackSpeed)
    {
        ATTACKSPEED = ATTACKSPEED + _attackSpeed;
        return ATTACKSPEED;
    }
    #endregion

    #region ATK
    public int ATTACKDAMAGE
    {
        get { return atk; }
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
        get { return hp; }
        set
        {
            hp = value;
            hp = Mathf.Clamp(value, 0, maxHp);

            if (value <= 0)
            {
                CurrentState = State.Dead;
                Debug.Log("죽었습니다.");
            }
        }
    }

    public int HPChanged(int ATK)
    {
        HP = HP - ATK;
        return HP;
    }
    #endregion

    #region MoveSpeed
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = value;
        }
    }

    public float MoveSpeedChanger(float _moveSpeed)
    {
        MoveSpeed = MoveSpeed - _moveSpeed;
        return MoveSpeed;
    }
    #endregion

    #region AtkRange
    public float AtkRange
    {
        get { return atkRange; }
        set
        {
            atkRange = value;
        }
    }

    public float AtkRangeChanger(float _atkRange)
    {
        AtkRange = _atkRange;
        return AtkRange;
    }
    #endregion



    //개체의 상태가 바뀔때마다 실행
    protected virtual void SetState(State newState)
    {
    }


    //@ 삭제예정 (플레이어로?)
    //공격을 할때 각도에 따라서 모션을 보여주기 위해 만듬 (즉, 적이 있을때만 사용)

    [HideInInspector]
    public int current_Anim_Frame; 
    [HideInInspector]
    public float enemy_angle; 
    [HideInInspector]
    public AttackType myAttackType; 
    [HideInInspector]
    public AnglePos myAnim_AnglePos;  
}
