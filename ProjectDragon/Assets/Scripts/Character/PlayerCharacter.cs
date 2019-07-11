using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public enum PlayerWeapon { None = 0, SWORD, BOW, STAFF };
    public enum PlayerState { None = 0, IDEL, WALK, ATTACK, SKILLATTACK, DEAD };
    public enum PlayerMoveAnimation { None = 0, MaleCharacterMoveFront, MaleCharacterMoveLeft, MaleCharacterMoveLeftSide, MaleCharacterMoveRight, MaleCharacterMoveRightSide, MaleCharacterMoveUp }

    protected JoyPad joyPad;
    public Animator playerAnimationStateChanger;
    public PlayerState playerState;

    //대각 속도
    public float horizontalSpeed = 1.0f;
    public float verticalSpeed = 0.3f;
    // player controll vector
    public Vector2 playerControllerVec;
    public Rigidbody2D rigidbody2d;
    public GameObject joypadinput;
    public Vector3 joystickPos;
    public Vector3 normalVec = new Vector3(-765, -355, 0);


    //PlayerHP
    public int PLAYERHP = 100;
    public int ATK = 1;
    public int ATKSPEED = 1;
    public bool isAttacking = false;



    void Start()
    {
        playerControllerVec = new Vector2(10, 0);
        joyPad = FindObjectOfType<JoyPad>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerAnimationStateChanger = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        joystickPos = joypadinput.GetComponent<UIJoystick>().position;
        //키보드 세팅
        //float h = horizontalSpeed * Input.GetAxis("Horizontal");
        //float v = verticalSpeed * Input.GetAxis("Vertical");
        float h = horizontalSpeed * joystickPos.x;
        float v = verticalSpeed * joystickPos.y;

        playerControllerVec = new Vector3(h, v, 0);

        transform.Translate(Vector2.right * Time.deltaTime * h, Space.World);
        transform.Translate(Vector2.up * Time.deltaTime * v, Space.World);

        //Angle of joystick and normalVec
        float Angle = GetAngle(joystickPos, normalVec);
        Debug.Log(Angle);
        MoveAnimationChanger(Angle);
    }

    public void MaleCharacterMove(string animationtype)
    {
        Animator[] childAnimator;
        childAnimator = GetComponentsInChildren<Animator>();
        foreach (Animator anim in childAnimator)
        {
            anim.Play(animationtype);
        }
    }
    public static float GetAngle(Vector3 Start, Vector3 End)
    {
        Vector3 v = End - Start;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
    public void MoveAnimationChanger(float angle)
    {
        if (angle <= -80)
        {
            MaleCharacterMove(PlayerMoveAnimation.MaleCharacterMoveRight.ToString());
        }
        else if (angle <= -70)
        {
            MaleCharacterMove(PlayerMoveAnimation.MaleCharacterMoveRightSide.ToString());
        }
        else if (angle <= -30)
        {
            MaleCharacterMove(PlayerMoveAnimation.MaleCharacterMoveUp.ToString());
        }
        else if (angle < -10)
        {
            MaleCharacterMove(PlayerMoveAnimation.MaleCharacterMoveLeftSide.ToString());
        }
        else if (angle == 0)
        {
            MaleCharacterMove(PlayerMoveAnimation.MaleCharacterMoveFront.ToString());
        }
        else if (angle < 40)
        {
            MaleCharacterMove(PlayerMoveAnimation.MaleCharacterMoveLeft.ToString());
        }
        else if (angle < 70)
        {
            MaleCharacterMove(PlayerMoveAnimation.MaleCharacterMoveFront.ToString());
        }
        else if (angle < 90)
        {
            MaleCharacterMove(PlayerMoveAnimation.MaleCharacterMoveRight.ToString());
        }
        
    }

    #region HPControll
    public int HP {
        get
        {
            return PLAYERHP;
        }
        set
        {
            if (value <= 0)
            {
                Debug.Log("죽었습니다.");
                playerState = PlayerState.DEAD;
            }
            else
            {
                PLAYERHP = value;
            }
        }
    }
    public int HPChanged(int ATK)
    {
        HP = HP - ATK;
        return HP;
    }
    #endregion

    #region ATK
    public int ATTACKDAMAGE
    {
        get
        {
            return ATK;
        }
        set
        {

             ATK = value;
        }
    }
    public int ATKChanger(int attackDamage)
    {
        ATK=ATK+ attackDamage;
        return ATK;
    }
    #endregion

    #region ATKSPEED
    public int ATTACKSPEED
    {
        get
        {
            return ATKSPEED;
        }
        set
        {
            ATKSPEED = value;
        }
    }
    public int ATKSpeedChanger(int attackSpeed)
    {
        ATKSPEED = ATKSPEED + attackSpeed;
        return ATKSPEED;
    }
    #endregion

    #region (TakeDamage) Player take a Damage by Monster

    private IEnumerator Attack()
    {
        isAttacking = true;

        yield return new WaitForSeconds(1);
    }
    #endregion
}

