using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum IsWear { None, DefaultCloth, AnimalCloth, Suit, DefultName, DefaltName2 }
public class Player : Character
{
    //템프 앵글
    public float temp_angle;

    //코루틴 제어 함수
    public bool isActive;
    // 플레이어 캐릭터 스테이터스

    // 테스팅용
    public bool TestBoolStick;
    public bool AngleisAttack;




    //플레이어 세팅
    public SEX sex;
    public IsWear isWear;


    public GameObject weaponSelection;
    private Animator weaponAnimator;
    public UISprite hpBar;

    //플레이어 정지

    public bool StopPlayer;
    public float StopTime;
    public float StopMaxTime;

    //플레이어 사운드

    public AudioClip walk_Sound;

    //대각 속도
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 5.0f;
    public GameObject DeadPanel;
    public SEX playerSex;



    //플레이어 애니메이션 컨트롤
    public Animator playerAnimationStateChanger;
    // player controll vector

    public Rigidbody2D rigidbody2d;

    //JoyStick
    protected JoyPad joyPad;
    public GameObject joypadinput;
    public Vector3 joystickPos;
    public Vector3 normalVec = new Vector3(0.0f, 0.0f, 0.0f);
    private Transform m_EnemyPos;

    //Check JoyStick
    public float h;
    public float v;


    public Transform EnemyPos { get { return m_EnemyPos; } set { m_EnemyPos = value; } }

    //죽었을때 패널
    public GameObject EndPanel;

    //적 찾기
    public RoomManager EnemyRoom;
    public GameObject[] Enemy;
    public List<GameObject> EnemyArray;
    public GameObject TempEnemy;

    //MP 임시사용용 변수
    public int mp;
    public int maxMp = 100;

    public override int HPChanged(int ATK)
    {
        DataTransaction.Inst.CurrentHp = HP;
        Debug.Log((float)HP / (float)maxHp);
        base.HPChanged(ATK);
        hpBar.fillAmount = (float)HP / (float)maxHp;
        return HP;
    }
    //MP 임시 사용

    public override int HP
    {
        get { return (int)DataTransaction.Inst.CurrentHp; }
        set
        {
            if (value > 0)
            {
                DataTransaction.Inst.CurrentHp = value;
                hp = (int)DataTransaction.Inst.CurrentHp;
                DataTransaction.Inst.CurrentHp = Mathf.Clamp(value, 0, maxHp);
            }
            else if (!isDead)
            {
                hp = -1;
                DataTransaction.Inst.CurrentHp = maxHp;
                isDead = true;
                CurrentState = State.Dead;
                Debug.Log("죽었습니다.");
            }
        }
    }
    public int MP
    {
        get { return mp; }
        set
        {
            if (value > 0)
            {
                mp = value;
                mp = Mathf.Clamp(value, 0, maxMp);
            }
            else
            {
                mp = -1;
            }
        }
    }

    public virtual int MPChanged(int Cost)
    {
        MP = MP - Cost;
        return MP;
    }

    public IEnumerator CalculateDistanceWithPlayer()
    {
        // 적이 하나라도 있으면
        if (EnemyArray.Count >= 1)
        {
            isActive = true;
            //동작
            while (isActive)
            {
                //EnemyArray = EnemyRoom.PlayerLocationRoomMonsterData();
                if (EnemyArray.Count > 0)
                {
                    for (int a = 0; a < EnemyArray.Count; a++)
                    {
                        TempEnemy = EnemyArray[0];
                        EnemyArray[a].GetComponent<Monster>().distanceOfPlayer = DistanceCheck(this.GetComponent<Transform>(), EnemyArray[a].GetComponent<Transform>());
                    }
                    for (int a = 0; a < EnemyArray.Count; a++)
                    {
                        if (TempEnemy.GetComponent<Monster>().distanceOfPlayer > EnemyArray[a].GetComponent<Monster>().distanceOfPlayer)
                        {
                            TempEnemy = EnemyArray[a];
                        }
                    }
                    if (DistanceCheck(this.GetComponent<Transform>(), TempEnemy.GetComponent<Transform>()) <= this.GetComponent<Player>().AtkRange)
                    {
                        if (TempEnemy.GetComponent<Character>().HP > 0)
                        {
                            AngleisAttack = true;
                            this.CurrentState = State.Attack;
                            this.enemy_angle = GetAngle(TempEnemy.transform.position, this.transform.position);
                        }
                    }
                    if (DistanceCheck(this.GetComponent<Transform>(), TempEnemy.GetComponent<Transform>()) > this.GetComponent<Player>().AtkRange)
                    {
                        AngleisAttack = false;
                        if (AngleisAttack == false)
                        {
                            if (current_angle == 0)
                            {
                                this.CurrentState = State.Idle;
                                isWalk = false;
                            }
                            if (enemy_angle != 0 && isWalk == true)
                            {
                                CurrentState = State.Walk;
                            }
                        }
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    break;
                }
            }
        }
    }
    public void TempNullSet()
    {
        TempEnemy = null;
        CurrentState = State.Idle;
    }
    void PlayerPrefDataTrascation()
    {
        maxHp = (int)DataTransaction.Inst.MaxHp;
        mp = (int)DataTransaction.Inst.Mp;
        sex = DataTransaction.Inst.Sex;
        ATTACKDAMAGE = (int)DataTransaction.Inst.CurrentDamage;
    }

    // Start is called before the first frame update
    void Awake()
    {
        TestBoolStick = true;
        //TODO: 뒤에 로비 완성되면 무기 합칠것, 스테이터스를 DB에서 받아오기
        EndPanel.SetActive(false);
        isWear = IsWear.DefaultCloth;
        playerSex = SEX.Female;
        PlayerPrefDataTrascation();
        MoveSpeed = 3.0f;
        ATKChanger(0);
        ATKSpeedChanger(1.0f);
        MoveSpeed = 3;
        CurrentState = State.Idle;
        AtkRangeChanger(4);
    }
    void Start()
    {
        StartCoroutine(CalculateDistanceWithPlayer());
        //내가 끼고 있는 칼에 대한 정의
        //Database.Inventory myWeapon = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        joyPad = FindObjectOfType<JoyPad>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerAnimationStateChanger = GetComponent<Animator>();
        hpBar = GameObject.Find("UI Root/PlayerHPBGI/PlayerHP").GetComponent<UISprite>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        current_angle = GetAngle(joypadinput.GetComponent<JoyPad>().target2.transform.position, joypadinput.GetComponent<JoyPad>().target.transform.position);
        myPos = gameObject.transform.position;
        joystickPos = joypadinput.GetComponent<JoyPad>().position;
#if UNITY_EDITOR

        if (TestBoolStick == false)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            if (h > 0 && v == 0)
            {
                current_angle = 90f;
            }
            if (h < 0 && v == 0)
            {
                current_angle = 270f;
            }
            if (h == 0 && v < 0)
            {
                current_angle = 360f;
            }
            if (h == 0 && v > 0)
            {
                current_angle = 180f;
            }
            enemy_angle = current_angle;

        }

#endif

        if (TestBoolStick)
        {
            //joystick
            h = joystickPos.x;
            v = joystickPos.y;
            //Make Right direction by Set Animatoion bool setting
        }
        if (StopPlayer.Equals(false))
        {
            rigidbody2d.velocity = new Vector2(10.0f * Time.deltaTime * h * horizontalSpeed * moveSpeed, 10.0f * Time.deltaTime * v * verticalSpeed * moveSpeed);
            ////transform.Translate(Vector2.right * Time.deltaTime * h * horizontalSpeed * moveSpeed, Space.World);

            //transform.Translate(Vector2.up * Time.deltaTime * v * verticalSpeed * moveSpeed, Space.World);
        }
        else if (StopPlayer.Equals(true))
        {
            StopTime += Time.deltaTime;
            if (StopTime >= StopMaxTime)
            {
                StopPlayer = false;
                StopTime = 0;
            }
        }


    }

    public static float GetAngle(Vector3 Start, Vector3 End)
    {
        Vector3 v = End - Start;
        return Quaternion.FromToRotation(Vector3.up, End - Start).eulerAngles.z;
    }
    public float DistanceCheck(Transform Player, Transform Enemy)
    {
        Vector3 offset = Player.position - Enemy.position;
        float sqrLen = offset.sqrMagnitude;
        return sqrLen;
    }

    #region Animation Changer
    protected override void SetState(State newState)
    {
        switch (newState)
        {
            case State.None:
                break;
            case State.Dead:
                isAttacking = false;
                isWalk = false;
                isDead = true;
                isHit = false;
                isSkillActive = false;
                Time.timeScale = 0;
                EndPanel.SetActive(true);
                EndPanel.GetComponentInChildren<UILabel>().text = "사망하셨습니다.";
                break;
            case State.Walk:
                isWalk = true;
                break;
            case State.Idle:
                isWalk = false;
                break;
            case State.Skill:
                isAttacking = false;
                isWalk = false;
                isDead = false;
                isHit = false;
                isSkillActive = true;
                break;
            case State.Attack:
                isAttacking = true;
                isWalk = false;
                isDead = false;
                isHit = false;
                isSkillActive = false;
                break;
            case State.Hit:
                isAttacking = false;
                isWalk = false;
                isDead = false;
                isHit = true;
                isSkillActive = false;
                break;
        }
    }
    public AnglePos Current_AngleCaseString(float angle)
    {
        if (angle == 0)
        {
            return AnglePos.Front;
        }
        if (angle < 45)
        {
            return AnglePos.Front;
        }
        else if (angle < 135)
        {
            return AnglePos.Right;
        }
        else if (angle < 225)
        {
            return AnglePos.Back;
        }
        else if (angle < 315)
        {
            return AnglePos.Left;
        }
        return AnglePos.Front;
    }
    public AnglePos Enemy_AngleCaseString(float angle)
    {
        if (angle == 0)
        {
            return AnglePos.Front;
        }
        if (angle >= 0 && angle < 45)
        {
            return AnglePos.Front;
        }
        else if (angle >= 45 && angle < 135)
        {
            return AnglePos.Right;
        }
        else if (angle >= 135 && angle < 225)
        {
            return AnglePos.Back;
        }
        else if (angle >= 225 && angle < 315)
        {
            return AnglePos.Left;
        }
        return AnglePos.Front;
    }
    //콜리젼에 따른 플레이어 밀림 방지
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {

                rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
                rigidbody2d.velocity = Vector2.zero;
            }
        }
    }
    // 콜리젼이 해제 됐을 때의 플레이어 밀림 방지
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                rigidbody2d.bodyType = RigidbodyType2D.Dynamic;

            }
        }
    }
}
#endregion