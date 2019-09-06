﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { None = 0, Walk, Attack, Dead, Skill, Hit };
public enum AnglePos
{
    None = 0, Front, Right, RightSide, Up, LeftSide, Left
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
    [SerializeField] public float angle;
    public Vector3 myPos;
    public Vector3 myRotat;


    // TODO : 이건 적이나 플레이어에게만 규정할 것 (스킬이 존재하고 있는)
    //[SerializeField] private float skillCoolDown;
    //[SerializeField] private float skillRange;

    public State myState;
    public AttackType myAttackType;

    [SerializeField] protected bool isAttacking;
    [SerializeField] protected bool isWalk;
    [SerializeField] protected bool isDead;
    [SerializeField] protected bool isHit;
    [SerializeField] protected bool isSkillActive;
    public Transform other;
    //Check Range

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
    public float AngleCalculate { get { return angle; } set { angle = value; } }
    public string AngleCaseString(float angle)
    {
        if (angle == 0)
        {

        }
        if (angle < 22.5)
        {
            return "front";
        }
        else if (angle < 112.5)
        {
            return "Right";
        }
        else if (angle < 112.5 + 45)
        {
            return "RightSide";
        }
        else if (angle < 112.5 + 90)
        {
            return "Up";
        }
        else if (angle < 112.5 + 135)
        {
            return "LeftSide";
        }
        else if (angle < 112.5 + 180)
        {
            return "Left";
        }
        else
        {
            return "Front";
        }
    }

    public void AnimatorCast(string animationtype)
    {
        gameObject.GetComponent<Animator>().Play(animationtype);
    }
    public State StateChaner(State state)
    {
        switch (state)
        {
            case State.Dead:
                isAttacking = false;
                isWalk = false;
                isDead = true;
                isHit = false;
                isSkillActive = false;
                return State.Dead;
            case State.Walk:
                isAttacking = false;
                isWalk = true;
                isDead = false;
                isHit = false;
                isSkillActive = false;
                return State.Attack;
            case State.Skill:
                isAttacking = false;
                isWalk = false;
                isDead = false;
                isHit = false;
                isSkillActive = true;
                return State.Skill;
            case State.Attack:
                isAttacking = true;
                isWalk = false;
                isDead = false;
                isHit = false;
                isSkillActive = false;
                return State.Skill;
            case State.Hit:
                isAttacking = false;
                isWalk = false;
                isDead = false;
                isHit = true;
                isSkillActive = false;
                return State.Hit;
        }
        return State.None;
    }
}