using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    public GameObject weaponSelection;
    public string Charactermotion = "motion01";
    //대각 속도
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 5.0f;
    public GameObject DeadPanel;
    public SEX playerSex;
    //JoyStick
    protected JoyPad joyPad;
    public Animator playerAnimationStateChanger;
    // player controll vector
    public Vector2 playerControllerVec;
    public Rigidbody2D rigidbody2d;
    public GameObject joypadinput;
    public Vector3 joystickPos;
    private Vector3 normalVec = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Awake()
    {
       //playerSex = Database.Inst.playData.sex;
        playerSex = SEX.Male;
        //TODO: 뒤에 로비 완성되면 무기 합칠것, 스테이터스를 DB에서 받아오기
        HP = 100;
        ATKChanger(10);
        ATKSpeedChanger(1.0f);
        MoveSpeed = 1;
        myState = State.Walk;
        AtkRangeChanger(3);
        myAttackType = AttackType.ShortRange;
        weaponSelection.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("SwordAnimation") as RuntimeAnimatorController;
    }
    void Start()
    {
        //내가 끼고 있는 칼에 대한 정의
        //Database.Inventory myWeapon = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        joyPad = FindObjectOfType<JoyPad>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        myPos = gameObject.transform.position;
       // DistanceCheck(3.0f);
        joystickPos = joypadinput.GetComponent<UIJoystick>().position;
        //키보드 세팅
        //float h = horizontalSpeed * Input.GetAxis("Horizontal");
        //float v = verticalSpeed * Input.GetAxis("Vertical");

        //joystick
        float h = horizontalSpeed * joystickPos.x;
        float v = verticalSpeed * joystickPos.y;

        playerControllerVec = new Vector3(h, v, 0);

        transform.Translate(Vector2.right * Time.deltaTime * h, Space.World);
        transform.Translate(Vector2.up * Time.deltaTime * v, Space.World);

        //Angle of joystick and normalVec
        if (isAttacking.Equals(false))
        {
            float Angle = GetAngle(joystickPos, normalVec);
            AnimationChanger(Angle);
        }
        if (isAttacking.Equals(true))
        {
          //  float Angle = GetAngle(EnemyPos.transform.position, normalVec);
          //  AnimationChanger(Angle);
        }
     
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
    public void AnimationChanger(float angle)
    {
        AngleCalculate(angle);
        Debug.Log("MaleIs" + myState.ToString() + myAttackType.ToString() + AngleCalculate(angle));
        if (playerSex == SEX.Male)
        {
            if (myAttackType.Equals(AttackType.ShortRange) && isAttacking.Equals(false))
            {

                switch (myState)
                {
                    case State.Walk:
                        weaponSelection.SetActive(true);
                        AnimatorCast("MaleIs" + myState.ToString() + myAttackType.ToString() + AngleCalculate(angle));
                        weaponSelection.GetComponent<Animator>().Play("TestSword"+AngleCalculate(angle));
                        break;
                    case State.Skill:
                        AnimatorCast("MaleIs" + myState.ToString() + Charactermotion + myAttackType.ToString() + AngleCalculate(angle));
                        break;
                    case State.Dead:
                        weaponSelection.SetActive(false);
                        AnimatorCast("MaleIsDead");
                        break;
                }
            }
            if (myAttackType.Equals(AttackType.ShortRange) && isAttacking.Equals(true))
            {
                switch (myState)
                {
                    case State.Attack:
                        AngleCalculate(angle);
                        AnimatorCast("MaleIs" + myState.ToString() + myAttackType.ToString() + AngleCalculate(angle));
                        weaponSelection.GetComponent<Animator>().Play("TestSwordAttack" + AngleCalculate(angle));
                        break;
                }
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
       
    }
}
#endregion