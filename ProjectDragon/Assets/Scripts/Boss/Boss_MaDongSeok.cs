﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BossState { Ready, Idle, Phase1, Phase2 }
public class Boss_MaDongSeok : Monster
{
    [SerializeField]
    GameObject objectPool, armright, armLeft;
    public BossState currentstate;
    public int projectiledamage;
    public float animationtimecheck, projectilespeed, projectiletime, Idletime;
    [SerializeField]
    Targetpoint[] projectileStone;
    [SerializeField]
    Projectile projectile;
    Targetpoint targetpoint;
    int random, projectilenum = 0;
    Vector3 viewportposition0, viewportposition1;
    IEnumerator bossphase;
    Camera MCamera;
    public ParticleSystem[] explosion;
    IEnumerator Bossphasechange, PhseState;
    public RuntimeAnimatorController projectileanim;
    public GameObject EndSprite;
    //플레이어진입,warning,boss phase1,탄떨어지기,탄 낙하위치 지정하기,내려찍기, 내려찍는 낙하위치 지정하기
    // Start is called before the first frame update
    protected override void Awake()
    {
        HP = maxHp;
        Bossphasechange = BossPhase();
        Debug.Log(HP);
            
        base.Awake();
    }
    protected override void Start()
    {
        damagePopup = new DamagePopup();
        targetpoint = new Targetpoint();
        //애니메이션 작동,플레이어 작동 불가,
        currentstate = BossState.Idle;
        explosion = GetComponentsInChildren<ParticleSystem>();
        Random.InitState((int)System.DateTime.Now.Ticks);
        armright = gameObject.transform.Find("MaDongSeokRightArm").gameObject;
        armLeft = gameObject.transform.Find("MaDongSeokLeftArm").gameObject;
        objectPool = GameObject.Find("MaDongSeokObjectPool").gameObject;
        //projectileStone = objectPool.transform.GetComponentsInChildren<Targetpoint>();
        projectile = new Projectile();
        random = 0;
        projectiletime = 0.5f;
        Idletime = 3f;
        MCamera = GameObject.Find("Camera").GetComponent<Camera>();
        viewportposition0 = MCamera.ViewportToWorldPoint(new Vector3(0, 0, 1));
        viewportposition1 = MCamera.ViewportToWorldPoint(new Vector3(1, 1, 1));
        Debug.Log("0번" + viewportposition0 + "1번" + viewportposition1);
        for (int i = 0; i < explosion.Length; i++)
        {
            explosion[i].Pause();
        }
        
        base.Start();
    }
    protected override void SetState(State newState)
    {
        if(newState.Equals(State.Dead))
        {
            objectAnimator.Play("MaDongSeokDead");
            StopCoroutine(PhseState);
            StopCoroutine(Bossphasechange);
            EndSprite.SetActive(true);
            Explosion();
        }
    }
    public void StartBoss()
    {
        StartCoroutine(Bossphasechange);
    }
    public override int HPChanged(int ATK)
    {
        //damagePopup.Create(transform.position, ATK, false, transform);
        if (HP < 50&& currentstate.Equals(BossState.Phase1))
        {

            currentstate = BossState.Phase2;
            StopCoroutine(PhseState);
            StopCoroutine(Bossphasechange);
            StartCoroutine(Bossphasechange);
        }
        if (ATK > 0&&HP>0)
        {
            IEnumerator flash = GetComponentInChildren<FlashWhite>().Flash();
            StartCoroutine(flash);
            Debug.Log("flash");
        }
        base.HPChanged(ATK);
        return HP;
    }
    IEnumerator BossPhase()//보스 패턴이 일어날 phase구현
    {
        int count = 0;
        if (HP > 0)
        {

            currentstate++;
            Debug.Log(HP);
            yield return new WaitForSeconds(1f);
            
        }
        while (0 < HP)
        {
            //현재챕터확인할것
            if (currentstate.Equals(BossState.Phase1))//phase1지정
            {
                Debug.Log(currentstate.ToString());

                if (PhseState != null && count.Equals(0))
                {
                    count = 1;
                    PhseState = State3();
                }
                else
                {
                    count = 0;
                    PhseState = State4();
                }
            }
            else//phase2지정
            {
                if (PhseState != null && count.Equals(0))
                {
                    count = 1;
                    PhseState = State4();
                }
                else
                {
                    count = 0;
                    PhseState = State5();
                }
                Debug.Log(currentstate.ToString());
            }
            yield return StartCoroutine(PhseState);
        }
        yield return null;
    }
    void Explosion()
    {
        for (int i = 0; i < explosion.Length; i++)
        {
            explosion[i].Play();
        }
    }
        //내려찍기 구현
        IEnumerator State1()
    {
        GameObject arm;
        int random = Random.Range(0, 2);
        if (random.Equals(0))
        {
            arm = armLeft;
        }
        else
        {
            arm = armright;
        }
        arm.GetComponent<Animator>().Play("MaDongSeokArmAttackReady");
        Debug.Log("phase1");
        yield return new WaitForSeconds(Idletime);
    }
    IEnumerator State2(Vector3 projectileposition)
    {

        targetpoint.Create(projectilespeed,projectiledamage, "TargetPointObj", projectileposition);
        yield return new WaitForSeconds(projectiletime);

    }
    //탄환발사1
    IEnumerator State3()
    {
        int randomposition = Random.Range(1, 6);
        int randomsite = Random.Range(0, 2);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                int angle = 120 + (j * 20);
                if (!j.Equals(randomposition))
                {
                    Vector3 bossmouth = gameObject.transform.position;
                    //해당 벡터 위치는 보스 입
                    projectile.Create(angle, projectilespeed, projectiledamage, projectileanim, "ProjectileObj", true, bossmouth);
                }

            }
            if (randomsite.Equals(0))
            {
                if (randomposition.Equals(1))
                {
                    randomsite = 1;
                    randomposition++;
                }
                else
                {
                    randomposition--;
                }
            }
            else
            {
                if (randomposition.Equals(5))
                {
                    randomsite = 0;
                    randomposition--;
                }
                else
                {
                    randomposition++;
                }
            }
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(Idletime);
    }
    public void TargeExplosion(Vector3 _targetpoint)
    {

        for (int i = 0; i < 5; i++)
        {
            projectile.Create(i * 72, projectilespeed, projectiledamage, projectileanim, "ProjectileObj", true, _targetpoint);
        }

    }
    //~투척
    IEnumerator State4()
    {
        objectAnimator.Play("BossSlimePattern");
        yield return new WaitForSeconds(3.0f);
        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D[] hits;
            Vector3 targetposition;
            bool ishit = false;
            do
            {
                ishit = false;
                Random.InitState((int)System.DateTime.Now.Ticks);

                //targetposition = new Vector3(Random.Range(viewportposition0.x, viewportposition1.x) * 0.1f, Random.Range(viewportposition0.y, viewportposition1.y) * 0.1f);
                targetposition = new Vector3(Random.Range(-1200, +1200) * 0.01f, Random.Range(-400, +400) * 0.01f);
                hits = Physics2D.RaycastAll(targetposition, transform.forward);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.GetComponent<Targetpoint>() != null)
                    {
                        Debug.Log("겹침");
                        ishit = true;
                    }
                }
            }
            while (ishit);
            yield return StartCoroutine("State2", targetposition);
        }
        yield return new WaitForSeconds(Idletime);
    }
    //탄환발사2
    IEnumerator State5()
    {
        int randomsite = Random.Range(0, 2);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int angle = 95 + (j * 20) + 10 * randomsite;
                Vector3 bossmouth = gameObject.transform.position;
                //해당 벡터 위치는 보스 입
                projectile.Create(angle, projectilespeed, projectiledamage, projectileanim, "ProjectileObj", true, bossmouth);
            }
            if (randomsite.Equals(0))
            {
                randomsite = 1;
            }
            else
            {
                randomsite = 0;
            }
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(Idletime);
    }
}
