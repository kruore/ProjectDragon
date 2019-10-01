using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControll : MonoBehaviour
{
    [Tooltip("재생할 오브젝트를 받아옵니다.")]
    public Character Anim_Master;

    [Tooltip("변수 할당용")]
    public AnglePos anglepos;
    [Tooltip("변수 할당용")]
    public State my_state;

    //캐릭터스프라이트 이름
    public string characterName;
    public string EnemyNumber;


    [Tooltip("now player animation")]
    public Animator playeranim;
    [Tooltip("runtime controller when the play begin")]
    public RuntimeAnimatorController controller;
    [Tooltip("override the controller after")]
    public AnimatorOverrideController overrideController;


    [Tooltip("죽음에 관련된 애니메이션")]
    public AnimationClip[] animDead;
    [Tooltip("걷기에 관련된 애니메이션")]
    public AnimationClip[] animWalk;
    [Tooltip("Hit에 관련된 애니메이션")]
    public AnimationClip[] animHit;
    [Tooltip("스킬에 관련된 애니메이션")]
    public AnimationClip[] animSkill;
    [Tooltip("공격에 관련된 애니메이션")]
    public AnimationClip[] animAttack;
    [Tooltip("대기에 관련된 애니메이션")]
    public AnimationClip[] animIdle;

    public string ClearAnimator_Name
    {
        get
        {
            return clearAnimator_name;
        }
        set
        {
            if (!value.Equals(clearAnimator_name))
            {
                AngleStringCast(value);
                clearAnimator_name = value;
            }
        }
    }
    public string temp_name;
    public string clearAnimator_name;
    //public Animation animation;
    //public RuntimeAnimatorController controller;
    //public AnimationClip animationClip;
    private void FixedUpdate()
    {
        temp_name = clearAnimator_name;
        ClearAnimator_Name = characterName + "_" + my_state.ToString() + "_" + anglepos.ToString();
    }
    public void Update()
    {
        //anglepos = Anim_Master.Current_AngleCaseString(Anim_Master.current_angle);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            InitPlayer();
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            InitEnemy();
        }

    }

    //초기화
    public void InitPlayer()
    {
        characterName = "Female_DefaultCloth";
        temp_name = "초기화값";
        clearAnimator_name = "초기화값";
        anglepos = 0;
        playeranim = GetComponent<Animator>();
        controller = playeranim.runtimeAnimatorController;
        overrideController = new AnimatorOverrideController(playeranim.runtimeAnimatorController);

        //애니메이터 초기화 검사
        for (int i = 1; i <= State.Hit.GetHashCode(); i++)
        {
            my_state++;
            Player_AnimationController_CastingCurrentAnim(animationValueChanger(my_state), my_state);
        }
    }


    //초기화 적
    public void InitEnemy()
    {
        //일단 예시
        EnemyNumber = "01";
        characterName = "Enemy";
        temp_name = "초기화값";
        clearAnimator_name = "초기화값";
        anglepos = 0;
        playeranim = GetComponent<Animator>();
        controller = playeranim.runtimeAnimatorController;
        overrideController = new AnimatorOverrideController(playeranim.runtimeAnimatorController);

        //애니메이터 초기화 검사
        for (int i = 1; i <= State.Hit.GetHashCode(); i++)
        {
            my_state++;
            Enemy_AnimationController_CastingCurrentAnim(animationValueChanger(my_state), my_state);
        }

    }

    void Player_AnimationController_CastingCurrentAnim(AnimationClip[] animationbundle, State state)
    {
        // clip = playeranim.GetCurrentAnimatorClipInfo(0)[0].clip;
        for (int i = 0; i < animationbundle.Length; i++)
        {
            //앵글 각도를 0으로 찍고 각 스트링에 따라 애니메이션 결합(Resoures.Load를 통함)
            anglepos++;
            animationbundle[i] = Resources.Load<AnimationClip>("Animation/"+characterName+"/" +characterName + "_"+ state.ToString() + "_" + anglepos.ToString());
            overrideController[characterName + "_" + state + "_" + anglepos.ToString()] = animationbundle[i];
            playeranim.runtimeAnimatorController = overrideController;
        }
        anglepos = AnglePos.None;
        //playeranim.runtimeAnimatorController = overrideController;
        //타임 스피드 변경
        playeranim.speed = 0.1f;
        //들고 있는 애니메이션 중에서 무엇을 재생할 것인지에 대한 정의
        playeranim.CrossFade(characterName + "_" + state.ToString() + "_" + anglepos.ToString(), 0.3f);
    }

    void Enemy_AnimationController_CastingCurrentAnim(AnimationClip[] animationbundle, State state)
    {
        // clip = playeranim.GetCurrentAnimatorClipInfo(0)[0].clip;
        for (int i = 0; i < animationbundle.Length; i++)
        {
            //앵글 각도를 0으로 찍고 각 스트링에 따라 애니메이션 결합(Resoures.Load를 통함)
            anglepos++;
            animationbundle[i] = Resources.Load<AnimationClip>("Animation/" + characterName + "/" + characterName + "_" + EnemyNumber+ "_" + state.ToString() + "_" + anglepos.ToString());
            overrideController[characterName + "_" + state + "_" + anglepos.ToString()] = animationbundle[i];
            playeranim.runtimeAnimatorController = overrideController;
        }
        anglepos = AnglePos.None;
        //playeranim.runtimeAnimatorController = overrideController;
        //타임 스피드 변경
        playeranim.speed = 0.1f;
        //들고 있는 애니메이션 중에서 무엇을 재생할 것인지에 대한 정의
        playeranim.CrossFade(characterName + "_" + state.ToString() + "_" + anglepos.ToString(), 0.3f);
    }

    protected virtual AnimationClip[] animationValueChanger(State state)
    {
        switch (state)
        {
            case State.Walk:
                return animWalk = new AnimationClip[4];
            case State.Dead:
                return animDead = new AnimationClip[4];
            case State.Hit:
                return animHit = new AnimationClip[4];
            case State.Skill:
                return animSkill = new AnimationClip[4];

            case State.Attack:
                return animAttack = new AnimationClip[4];
        }
        return animIdle;
    }
    void Animation_Setter(string animationName)
    {
        Debug.Log(animationName.ToString());
    }
    void AngleStringCast(string name)
    {
        playeranim.Play(name);
        //CrossFade(name, 0.3f);
    }
}
