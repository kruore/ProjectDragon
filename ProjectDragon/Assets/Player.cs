using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum IsWear { None, DefaultCloth, AnimalCloth, Suit, DefultName, DefaltName2 }
public class Player : Character
{
    public GameObject weaponSelection;
    private Animator weaponAnimator;

    public GameObject battlemanager;

    public string Charactermotion = "motion01";
    //대각 속도
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 5.0f;
    public GameObject DeadPanel;
    public SEX playerSex;
    //JoyStick
    protected JoyPad joyPad;
   // public AnglePos my_AnglePos { get { return myAnim_AnglePos; } set { myAnim_AnglePos = value; GetComponent<Animator>().CrossFade(playerSex.ToString(), 0.3f); } }

    //Animation Contorl
    public Animator playerAnimationStateChanger;
    // player controll vector
    public Vector2 playerControllerVec;
    public Rigidbody2D rigidbody2d;
    public GameObject joypadinput;
    public Vector3 joystickPos;
    public Vector3 normalVec = new Vector3(0.0f, 0.0f, 0.0f);
    public IsWear isWear;
    private Transform m_EnemyPos;
    public Transform EnemyPos { get { return m_EnemyPos; } set { m_EnemyPos = value; } }


    //Check JoyStick
    private float h;
    private float v;

    // Start is called before the first frame update
    void Awake()
    {
      
        isWear = IsWear.DefaultCloth;
        //playerSex = Database.Inst.playData.sex;
        playerSex = SEX.Male;
        //TODO: 뒤에 로비 완성되면 무기 합칠것, 스테이터스를 DB에서 받아오기
        HP = 100;
        ATKChanger(10);
        ATKSpeedChanger(1.0f);
        MoveSpeed = 1;
        myState = State.Walk;
        AtkRangeChanger(10);
        myAttackType = AttackType.ShortRange;
        //weaponAnimator = weaponSelection.GetComponent<Animator>();
        // weaponAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("SwordAnimation");
    }
    void Start()
    {
        isWalk = true;
        //내가 끼고 있는 칼에 대한 정의
        //Database.Inventory myWeapon = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        joyPad = FindObjectOfType<JoyPad>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerAnimationStateChanger = GetComponent<Animator>();
        // gameObject.GetComponent<CircleCollider2D>().radius = AtkRange;
        //  playerAnimationStateChanger.SetInteger("myRange", myAttackType.GetHashCode());
        //  playerAnimationStateChanger.SetInteger("isMale", playerSex.GetHashCode());
        //   playerAnimationStateChanger.SetInteger("isWear", isWear.GetHashCode());
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        current_angle = joypadinput.GetComponent<UIJoystick>().angle;
        myPos = gameObject.transform.position;
        joystickPos = joypadinput.GetComponent<UIJoystick>().position;
        //키보드 세팅
        //float h = horizontalSpeed * Input.GetAxis("Horizontal");
        //float v = verticalSpeed * Input.GetAxis("Vertical");

        //joystick
        h = joystickPos.x;
        v = joystickPos.y;

        //Make Right direction by Set Animatoion bool setting

        AnimationChanger();

        transform.Translate(Vector2.right * Time.deltaTime * h * horizontalSpeed, Space.World);
        transform.Translate(Vector2.up * Time.deltaTime * v * verticalSpeed, Space.World);
        #region 구버젼 애니메이터
        ////Angle of joystick and normalVec
        //if (isAttacking.Equals(false))
        //{
        //  angle = GetAngle(joystickPos, normalVec);
        // //   AnimationChanger(Angle);
        //}
        //if (isAttacking.Equals(true))
        //{
        //  //  float Angle = GetAngle(EnemyPos.transform.position, normalVec);
        //  //  AnimationChanger(Angle);
        //}
        #endregion

    }
    IEnumerator DeadFade()
    {
        yield return new WaitForSeconds(2.0f);
        DeadPanel.SetActive(true);
    }

    public static float GetAngle(Vector3 Start, Vector3 End)
    {
        Vector3 v = End - Start;
        return Quaternion.FromToRotation(Vector3.up, End - Start).eulerAngles.z;
    }

    #region Animation Changer
    public void AnimationChanger()
    {
        #region 구버젼 애니메이터
        // AngleCalculate(angle);
        // // Check Complete
        //// Debug.Log("MaleIs" + myState.ToString() + myAttackType.ToString() + AngleCalculate(angle));
        // if (playerSex == SEX.Male)
        // {
        //     if (myAttackType.Equals(AttackType.ShortRange) && isAttacking.Equals(false))
        //     {
        //         switch (myState)
        //         {
        //             case State.Walk:
        //                 weaponSelection.SetActive(true);
        //                 AnimatorCast("MaleIs" + myState.ToString() + myAttackType.ToString() + AngleCalculate(angle));
        //                 weaponSelection.GetComponent<Animator>().Play("TestSword"+AngleCalculate(angle));
        //                 break;
        //             case State.Skill:
        //                 AnimatorCast("MaleIs" + myState.ToString() + Charactermotion + myAttackType.ToString() + AngleCalculate(angle));
        //                 break;
        //             case State.Dead:
        //                 weaponSelection.SetActive(false);
        //                 AnimatorCast("MaleIsDead");
        //                 break;
        //         }
        //     }
        //     if (myAttackType.Equals(AttackType.ShortRange) && isAttacking.Equals(true))
        //     {
        //         switch (myState)
        //         {
        //             case State.Attack:
        //                 AngleCalculate(angle);
        //                 AnimatorCast("MaleIs" + myState.ToString() + myAttackType.ToString() + AngleCalculate(angle));
        //                 weaponSelection.GetComponent<Animator>().Play("TestSwordAttack" + AngleCalculate(angle));
        //                 break;
        //         }
        //     }
        // }
        //#endregion
        //playerAnimationStateChanger.SetFloat("h", h);
        //playerAnimationStateChanger.SetFloat("v", v);
        //playerAnimationStateChanger.SetBool("isAttack", isAttacking);
        //playerAnimationStateChanger.SetBool("isWalk", isWalk);
        //playerAnimationStateChanger.SetBool("isDead", isDead);
        //// playerAnimationStateChanger.SetBool("isSkillActive", isSkillActive);
        //playerAnimationStateChanger.SetBool("isHit", isHit);
        //playerAnimationStateChanger.SetFloat("Angle", AngleCalculate);
        //weaponAnimator.SetFloat("h", h);
        //weaponAnimator.SetFloat("v", v);
        //weaponAnimator.SetBool("isAttack", isAttacking);
        //weaponAnimator.SetBool("isWalk", isWalk);
        //weaponAnimator.SetBool("isDead", isDead);
        // playerAnimationStateChanger.SetBool("isSkillActive", isSkillActive);
        //weaponAnimator.SetBool("isHit", isHit);
       // weaponAnimator.SetFloat("Angle", AngleCalculate);
        #endregion
    }
    public void WeaponAnimatorChanger()
    {
        //    weaponAnimator.SetBool();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            if (EnemyPos == null)
            {
                return;
            }
            StateChaner(State.Attack);
          //  enemy_angle = GetAngle(EnemyPos.position, gameObject.transform.position);
          //  AngleCalculate = enemy_angle;
        }
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
#endregion