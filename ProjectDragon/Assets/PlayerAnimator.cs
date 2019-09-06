using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimator : MonoBehaviour
{
    //JoyStick
    // protected JoyPad joyPad;

    //Check JoyStick
    private float h;
    private float v;

    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 5.0f;

    public Vector3 joystickPos;
    public GameObject joypadinput;

    [Header("PlayerScript")]
    public Player collectPlayer;

    [Header("SpriteRenderer")]
    public SpriteRenderer spriteRenderer;

    [Header("Player Sprite")]
    Sprite[] play_animtionSprite;
    string animationname { get { return m_animationName; } set { if (!value.Equals(m_animationName)) { m_animationName = value; AnimationNameChecker(); } } }
    string m_animationName;
    int current_Animcount = 0;

    [Header("AttackTYPE")]
    public AttackType m_attacktype;
    public IsWear m_cloth;
    public State m_state;
    //AnimationControl

    [Header("AnimationCounter")]
    public int AttackAniCount;
    public int WalkAniCount;
    public int HitAniCount;
    public int SkillAniCount;

    [Header("Dead")]
    public Sprite[] PlayerDead;
    [Header("Walk")]
    public Sprite[] PlayerWalkLeft;
    public Sprite[] PlayerWalkLeftSide;
    public Sprite[] PlayerWalkRight;
    public Sprite[] PlayerWalkRightSide;
    public Sprite[] PlayerWalkFront;
    public Sprite[] PlayerWalkUp;
    [Header("Attack")]
    public Sprite[] PlayerAttackLeft;
    public Sprite[] PlayerAttackLeftSide;
    public Sprite[] PlayerAttackRight;
    public Sprite[] PlayerAttackRightSide;
    public Sprite[] PlayerAttackFront;
    public Sprite[] PlayerAttackUp;
    [Header("Hit")]
    public Sprite[] PlayerHitLeft;
    public Sprite[] PlayerHitLeftSide;
    public Sprite[] PlayerHitRight;
    public Sprite[] PlayerHitRightSide;
    public Sprite[] PlayerHitFront;
    public Sprite[] PlayerHitUp;
    [Header("Skill")]
    public Sprite[] PlayerSkillLeft;
    public Sprite[] PlayerSkillLeftSide;
    public Sprite[] PlayerSkillRight;
    public Sprite[] PlayerSkillRightSide;
    public Sprite[] PlayerSkillFront;
    public Sprite[] PlayerSkillUp;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_attacktype = AttackType.LongRange;
        m_cloth = IsWear.DefaultCloth;
        m_state = State.Dead;
        DeadAnim();
        AnimationRangeCheck();
        //  Attack_AnimationMatching();
        Walk_AnimationMatching();
        collectPlayer = gameObject.GetComponent<Player>();
        play_animtionSprite = PlayerWalkFront;
        StartCoroutine("AnimationControllPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        animationname = "PlayerWalkFront";
        //"Player" + collectPlayer.AngleCaseString(collectPlayer.angle) + collectPlayer.myState.ToString();
        // myPos = gameObject.transform.position;
        //joystickPos = joypadinput.GetComponent<UIJoystick>().position;

        ////조이스틱
        //h = joystickPos.x;
        //v = joystickPos.y;


        //transform.Translate(Vector2.right * Time.deltaTime * h * horizontalSpeed, Space.World);
        //transform.Translate(Vector2.up * Time.deltaTime * v * verticalSpeed, Space.World);
    }

    void AttackRangeAnimationSetting(AttackType m_attacktype)
    {
        switch (m_attacktype)
        {
            case AttackType.ShortRange:
                break;
            case AttackType.MiddleRange:
                break;
            case AttackType.LongRange:
                break;
        }
    }
    void DeadAnim()
    {
        PlayerDead = new Sprite[10];
        string name = "PlayerAnimation/" + AttackType.ShortRange.ToString() + "/" + m_cloth.ToString() + "/" + m_state.ToString() + "/" + (m_cloth.ToString() + m_state.ToString());
        for (int i = 1; i < PlayerDead.Length + 1; i++)
        {
            PlayerDead[i - 1] = Resources.Load<Sprite>(name + (i - 1));
        }
    }
    void AnimationRangeCheck()
    {
        switch (m_attacktype)
        {
            case AttackType.ShortRange:
                AttackAniCount = 11;
                WalkAniCount = 5;
                HitAniCount = 4;
                break;
            case AttackType.MiddleRange:
                AttackAniCount = 11;
                WalkAniCount = 5;
                HitAniCount = 4;
                break;
            case AttackType.LongRange:
                AttackAniCount = 11;
                WalkAniCount = 5;
                HitAniCount = 4;
                break;
        }
    }
    void Attack_AnimationMatching()
    {
        PlayerAttackLeft = new Sprite[AttackAniCount];
        PlayerAttackLeftSide = new Sprite[AttackAniCount];
        PlayerAttackRight = new Sprite[AttackAniCount];
        PlayerAttackRightSide = new Sprite[AttackAniCount];
        PlayerAttackFront = new Sprite[AttackAniCount];
        PlayerAttackUp = new Sprite[AttackAniCount];
        string name = ("PlayerAnimation/" + m_attacktype.ToString() + "/" + m_cloth.ToString() + "/" + State.Attack.ToString() + "/");
        for (int i = 1; i < AttackAniCount + 1; i++)
        {
            PlayerAttackLeft[i] = Resources.Load<Sprite>(name + AnglePos.Left.ToString() + i);
            PlayerAttackLeftSide[i] = Resources.Load<Sprite>(name + AnglePos.LeftSide.ToString() + i);
            PlayerAttackRight[i] = Resources.Load<Sprite>(name + AnglePos.Right.ToString() + i);
            PlayerAttackRightSide[i] = Resources.Load<Sprite>(name + AnglePos.RightSide.ToString() + i);
            PlayerAttackFront[i] = Resources.Load<Sprite>(name + AnglePos.Front.ToString() + i);
            PlayerAttackUp[i] = Resources.Load<Sprite>(name + AnglePos.Up.ToString() + i);
        }
    }
    void Walk_AnimationMatching()
    {
        PlayerWalkLeft = new Sprite[WalkAniCount];
        PlayerWalkLeftSide = new Sprite[WalkAniCount];
        PlayerWalkRight = new Sprite[WalkAniCount];
        PlayerWalkRightSide = new Sprite[WalkAniCount];
        PlayerWalkFront = new Sprite[WalkAniCount];
        PlayerWalkUp = new Sprite[WalkAniCount];
        string name = ("PlayerAnimation/" + m_attacktype.ToString() + "/" + m_cloth.ToString() + "/" + State.Walk.ToString() + "/");
        for (int i = 0; i < WalkAniCount; i++)
        {
            PlayerWalkLeft[i] = Resources.Load<Sprite>(name + AnglePos.Left.ToString() + "/" + m_cloth.ToString() + "Walk" + AnglePos.Left.ToString() + i);
            PlayerWalkLeftSide[i] = Resources.Load<Sprite>(name + AnglePos.LeftSide.ToString() + "/" + m_cloth.ToString() + "Walk" + AnglePos.LeftSide.ToString() + i);
            PlayerWalkRight[i] = Resources.Load<Sprite>(name + AnglePos.Right.ToString() + "/" + m_cloth.ToString() + "Walk" + AnglePos.Right.ToString() + i);
            PlayerWalkRightSide[i] = Resources.Load<Sprite>(name + AnglePos.RightSide.ToString() + "/" + m_cloth.ToString() + "Walk" + AnglePos.RightSide.ToString() + i);
            PlayerWalkFront[i] = Resources.Load<Sprite>(name + AnglePos.Front.ToString() + "/" + m_cloth.ToString() + "Walk" + AnglePos.Front.ToString() + i);
            PlayerWalkUp[i] = Resources.Load<Sprite>(name + AnglePos.Up.ToString() + "/" + m_cloth.ToString() + "Walk" + AnglePos.Up.ToString() + i);
        }
    }
    void Skill_AnimationMatching()
    {
        PlayerSkillLeft = new Sprite[SkillAniCount];
        PlayerSkillLeftSide = new Sprite[SkillAniCount];
        PlayerSkillRight = new Sprite[SkillAniCount];
        PlayerSkillRightSide = new Sprite[SkillAniCount];
        PlayerSkillFront = new Sprite[SkillAniCount];
        PlayerSkillUp = new Sprite[SkillAniCount];
        string name = "PlayerAnimation/" + m_attacktype.ToString() + "/" + m_cloth.ToString() + "/" + State.Skill.ToString() + "/" + (m_cloth.ToString() + State.Skill.ToString());
        for (int i = 0; i < SkillAniCount; i++)
        {
            PlayerSkillLeft[i] = Resources.Load<Sprite>(name + AnglePos.Left.ToString() + i);
            PlayerSkillLeftSide[i] = Resources.Load<Sprite>(name + AnglePos.LeftSide.ToString() + i);
            PlayerSkillRight[i] = Resources.Load<Sprite>(name + AnglePos.Right.ToString() + i);
            PlayerSkillRightSide[i] = Resources.Load<Sprite>(name + AnglePos.RightSide.ToString() + i);
            PlayerSkillFront[i] = Resources.Load<Sprite>(name + AnglePos.Front.ToString() + i);
            PlayerSkillUp[i] = Resources.Load<Sprite>(name + AnglePos.Up.ToString() + i);
        }
    }
    IEnumerator AnimationControllPlayer()
    {
        bool dead = false;
        float m_frameTime = 1;
        float collect_frameTime = 0.5f;

        while (!dead)
        {
            collect_frameTime = AnimationFramePlay(play_animtionSprite, m_frameTime);
            spriteRenderer.sprite = play_animtionSprite[current_Animcount];
            current_Animcount++;
            if(current_Animcount>=play_animtionSprite.Length)
            {
                current_Animcount = 0;
            }
            yield return new WaitForSeconds(collect_frameTime);
        }
    }
    float AnimationFramePlay(Sprite[] spritePack, float frameTime)
    {
        return frameTime / spritePack.Length;
    }
    void AnimationNameChecker()
    {
        switch (animationname)
        {
            case "PlayerWalkFront":
                play_animtionSprite = PlayerWalkFront;
                break;
        }
        current_Animcount = 0;
    }
}
