//////////////////////////////////////////////////////////MADE BY Koo KyoSeok///2019-12-16/////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BossState { Ready, Idle, Phase1, Phase2, Phase3, Phase4 }
public class Boss_MaDongSeok : Monster
{
    [SerializeField]
    GameObject objectPool, armright, armLeft, arms;
    public BossState currentstate;
    public int projectiledamage;
    public float animationtimecheck, projectilespeed, projectiletime, Idletime;



    [SerializeField]
    Targetpoint[] projectileStone;
    [SerializeField]
    Projectile projectile;
    Targetpoint targetpoint;
    [SerializeField]
    int random, projectilenum = 0, count = 0;
    Vector3 viewportposition0, viewportposition1;
    IEnumerator bossphase;
    Camera MCamera;
    public ParticleSystem[] explosion;
    IEnumerator Bossphasechange, PhaseState,Phase2Timecheck;
    public RuntimeAnimatorController projectileanim;
    public GameObject EndSprite;
    public GameObject player,razer;
    GameObject Bossroom;
    GameObject[] manastone;
    GameObject[] hitDownplace;
    public int damage;
    [Header("HandTime")]
    public float handuptime;
    public float handdowntime;
    public float handwaittime;
    ParticleSystem roar;
    //플레이어진입,warning,boss phase1,탄떨어지기,탄 낙하위치 지정하기,내려찍기, 내려찍는 낙하위치 지정하기
    // Start is called before the first frame update
    protected override void Awake()
    {
        HP = maxHp;
        objectAnimator = gameObject.GetComponent<Animator>();
        Bossphasechange = BossPhase();
        Debug.Log(HP);

        base.Awake();
    }
    private void BossInit()
    {
        Bossroom = gameObject.transform.parent.transform.Find("보스방").gameObject;
        manastone = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            manastone[i] = Bossroom.transform.Find(string.Format("ManaStonePlace{0}", i + 1)).gameObject;
        }
        hitDownplace = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            hitDownplace[i] = Bossroom.transform.Find(string.Format("HitDownPlace{0}", i + 1)).gameObject;
        }
        armright = gameObject.transform.Find("MaDongSeokArms/MaDongSeokRightArm").gameObject;
        armLeft = gameObject.transform.Find("MaDongSeokArms/MaDongSeokLeftArm").gameObject;
        arms = gameObject.transform.Find("MaDongSeokArms").gameObject;
        objectPool = GameObject.Find("EnemyObjectPool").gameObject;
        MCamera = GameObject.Find("Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        razer = gameObject.transform.Find("RazerBeam").gameObject;
        razer.GetComponent<LazerBeam>().stoptag.Add("Stone");
        explosion = gameObject.transform.Find("ExplosionParticles").GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < explosion.Length; i++)
        {
            explosion[i].Pause();
        }
        roar = gameObject.transform.Find("RoarParticle").GetComponent<ParticleSystem>();
        roar.Pause();
    }
    public void PlayRoar()
    {
        roar.Play();
    }
    protected override void Start()
    {
        BossInit();
        damagePopup = new DamagePopup();
        targetpoint = new Targetpoint();
        //애니메이션 작동,플레이어 작동 불가,
        currentstate = BossState.Idle;
        
        Random.InitState((int)System.DateTime.Now.Ticks);

        //projectileStone = objectPool.transform.GetComponentsInChildren<Targetpoint>();
        projectile = new Projectile();
        random = 0;
        projectiletime = 0.5f;
        Idletime = 3f;

        viewportposition0 = MCamera.ViewportToWorldPoint(new Vector3(0, 0, 1));
        viewportposition1 = MCamera.ViewportToWorldPoint(new Vector3(1, 1, 1));
        Debug.Log("0번" + viewportposition0 + "1번" + viewportposition1);
        

        base.Start();
    }
    
    public override IEnumerator Start_On()
    {
        StartBoss();
        yield return null;
    }
    protected override void SetState<T>(T newState)
    {
        if (newState.Equals(State.Dead))
        {
            objectAnimator.Play("MaDongSeokDead");
            StopCoroutine(PhaseState);
            StopCoroutine(Bossphasechange);
            EndSprite.SetActive(true);
            Explosion();
            Debug.Log(hp + "::" + HP);
        }
    }
    /// <summary>
    /// 보스시작시 해야할것
    /// </summary>
    public void StartBoss()
    {
        StartCoroutine(Bossphasechange);
    }
    /// <summary>
    /// 보스의 HP가 변할때 콜할것
    /// </summary>
    /// <param name="ATK">보스가 공격당함을 기본 상정</param>
    /// <returns></returns>

    public override int HPChanged(int ATK, bool isCritical, int NukBack)
    {
        //damagePopup.Create(transform.position, ATK, false, transform);
        //if (HP < 50 && currentstate.Equals(BossState.Phase1))
        //{
        //    currentstate = BossState.Phase2;
        //    StopCoroutine(PhaseState);
        //    StopCoroutine(Bossphasechange);
        //    StartCoroutine(Bossphasechange);
        //}
        if (ATK > 0 && HP > 0)
        {
            IEnumerator flash = GetComponentInChildren<FlashWhite>().Flash();
            StartCoroutine(flash);
            Debug.Log("flash");
        }
        return base.HPChanged(ATK, isCritical, NukBack);
    }

    /// <summary>
    /// 보스 패턴이 일어날 phase구현
    /// </summary>
    /// <returns></returns>
    IEnumerator BossPhase()
    {

        if (HP > 0)
        {
            currentstate++;
            Debug.Log(HP);
            yield return new WaitForSeconds(1f);
            GameObject.Find("Main Camera").GetComponent<CameraFollow>().BossFollow(gameObject);
            yield return StartCoroutine(Roar());
        }
        while (0 < HP)
        {
            //현재챕터확인할것
            switch (currentstate)
            {
                case BossState.Phase1:
                    switch (count)
                    {
                        case 0:
                            {
                                count++;
                                PhaseState = ArmsChasingPlayer();
                                //PhseState = State3();
                                //레프트 라이트 랜덤
                                Debug.Log("lol");
                                break;
                            }
                        case 1:
                            {
                                count++;
                                if (Random.Range(0, 2).Equals(0))
                                {
                                    PhaseState = LeftHook();
                                }
                                else
                                {
                                    PhaseState = RightHook();
                                }
                                //PhseState = State4();
                                //내려찍기 이후 패턴2로 보내기
                                break;
                            }
                        case 2:
                            {
                                count = 0;
                                PhaseState = HitDown();
                                currentstate++;
                                break;
                            }
                    }
                    break;
                case BossState.Phase2:
                    switch (count)
                    {
                        case 0:
                            {
                                Debug.Log("lol12");
                                count++;
                                PhaseState = ManaStoneSumon();
                                //PhseState = State4();
                                //마나석 소환
                                break;
                            }
                        case 1:
                            {
                                count++;
                                PhaseState = ManaShoot1();
                                //PhseState = State5();
                                //마나탄 발사 1,2 반복 15초 내 파괴성공시 패턴 4 실패시 패턴 3
                                break;
                            }
                        case 2:
                            {
                                count = 1;
                                PhaseState = ManaShoot2();
                                break;
                            }
                    }
                    break;
                case BossState.Phase3:
                    switch (count)
                    {
                        case 0:
                            {
                                count++;
                                PhaseState = Roar();
                                break;
                            }
                        case 1:
                            {
                                count++;
                                PhaseState = RockSumon();
                                break;
                            }
                        case 2:
                            {
                                count = 0;
                                PhaseState = RazerBeam();
                                currentstate++;
                                break;
                            }
                    }
                    break;
                case BossState.Phase4:
                    if (Random.Range(0, 1).Equals(0))
                    {
                        PhaseState = LeftSweep();
                    }
                    else
                    {
                        PhaseState = RightSweep();
                    }
                    currentstate = BossState.Phase1;
                    break;
                default:
                    break;
            }
            if (PhaseState != null)
            {
                yield return StartCoroutine(PhaseState);
            }
            else
            {
                yield return null;
            }
        }
        yield return null;
    }
    /// <summary>
    /// 파티클 재생 
    /// </summary>
    void Explosion()
    {
        for (int i = 0; i < explosion.Length; i++)
        {
            explosion[i].Play();
        }
    }
    IEnumerator ArmsChasingPlayer()
    {
        float time = 0;
        Vector3 armsposition = arms.transform.position;
        while (time <= 1)
        {
            time += Time.deltaTime;
            arms.transform.position = Vector3.Lerp(armsposition, player.transform.position, time);
            yield return null;
        }
        while (time <= 3)
        {
            time += Time.deltaTime;
            arms.transform.position = Vector3.Lerp(arms.transform.position, player.transform.position, 0.1f);
            yield return null;
        }
    }
    IEnumerator RightHook()
    {
        arms.GetComponent<Animator>().Play("RightHook");
        Debug.Log("RightHook");
        yield return new WaitForSeconds(1);
        StartCoroutine(ArmsReset());
    }
    IEnumerator LeftHook()
    {
        arms.GetComponent<Animator>().Play("LeftHook");
        Debug.Log("LeftHook");
        yield return new WaitForSeconds(1);
        StartCoroutine(ArmsReset());
        Debug.Log(arms.transform.position);
    }
    IEnumerator ArmsReset()
    {
        float time = 0;
        Vector3 localPosition = arms.transform.localPosition;
        while (time <= 1)
        {
            time += Time.deltaTime;
            arms.transform.localPosition = Vector3.Lerp(localPosition, Vector3.zero, time);
            yield return null;
        }
        yield return new WaitForSeconds(Idletime);
    }
    IEnumerator HitDown()
    {
        int count = 0;
        IEnumerator HandHitDown = null;
        int random = Random.Range(0, 6);
        while (count < 6)
        {
            int temprandom;
            do
            {
                temprandom = Random.Range(0, 6);
            }
            while (random.Equals(temprandom));
            random = temprandom;
            switch (count % 2)
            {
                case 0:
                    {
                        HandHitDown = LeftHandHitDown(random);
                        break;
                    }
                case 1:
                    {
                        HandHitDown = RightHandHitDown(random);
                        break;
                    }
            }
            count++;
            Debug.Log("Phase2" + count);
            yield return StartCoroutine(HandHitDown);
        }
        Debug.Log("HitDown");
        yield return null;
    }
    IEnumerator RightHandHitDown(int placenum)
    {
        
        Debug.Log("Righthand");
        float time = 0;
        Vector3 hitposition = hitDownplace[placenum].transform.position;
        hitposition.y += 10;
        while (time < 0.1)
        {
            armright.transform.position = Vector3.Lerp(armright.transform.position, hitposition, time * 10);
            time += Time.deltaTime;
            yield return null;
        }
        armright.transform.position = hitposition;
        time = 0;
        hitDownplace[placenum].SetActive(true);
        while (time < 1)
        {
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;
        Vector3 firstarmposition = armright.transform.position;
        while (time < 0.1)
        {
            armright.transform.GetChild(0).gameObject.SetActive(false);
            armright.transform.position = Vector3.Lerp(firstarmposition, hitDownplace[placenum].transform.position, (time * 10));
            time += Time.deltaTime;
            yield return null;
        }
        LayerMask playerlayer = LayerMask.GetMask("Player");
        Vector2 boxcollidersize = hitDownplace[placenum].GetComponent<BoxCollider2D>().size;
        Debug.Log(boxcollidersize);
        boxcollidersize.x = boxcollidersize.x * hitDownplace[placenum].transform.localScale.x;
        boxcollidersize.y = boxcollidersize.y * hitDownplace[placenum].transform.localScale.y;
        Debug.Log(Physics2D.OverlapBox(hitDownplace[placenum].transform.position, boxcollidersize, 0, playerlayer));
        if (Physics2D.OverlapBox(hitDownplace[placenum].transform.position, boxcollidersize, 0, playerlayer))
        {
            player.GetComponent<Character>().HPChanged(damage,false,0);
        }
        armright.transform.GetChild(0).gameObject.SetActive(true);
        hitDownplace[placenum].SetActive(false);
        armright.transform.position = hitDownplace[placenum].transform.position;
        yield return null;
    }
    IEnumerator LeftHandHitDown(int placenum)
    {
        
        Debug.Log("Lefthand");
        float time = 0;
        Vector3 hitposition = hitDownplace[placenum].transform.position;
        hitposition.y += 10;
        while (time < 0.1)
        {
            armLeft.transform.position = Vector3.Lerp(armLeft.transform.position, hitposition, time * 10);
            time += Time.deltaTime;
            yield return null;
        }
        armLeft.transform.position = hitposition;
        time = 0;
        hitDownplace[placenum].SetActive(true);
        while (time < 1)
        {
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;
        Vector3 firstarmposition = armLeft.transform.position;
        while (time < 0.05)
        {
            armLeft.transform.GetChild(0).gameObject.SetActive(false);
            time += Time.deltaTime;
            armLeft.transform.position = Vector3.Lerp(firstarmposition, hitDownplace[placenum].transform.position, (time * 20));
            yield return null;
        }
        LayerMask playerlayer = LayerMask.GetMask("Player");
        Vector2 boxcollidersize = hitDownplace[placenum].GetComponent<BoxCollider2D>().size;
        boxcollidersize.x = boxcollidersize.x * hitDownplace[placenum].transform.localScale.x;
        boxcollidersize.y = boxcollidersize.y * hitDownplace[placenum].transform.localScale.y;
        Debug.Log(hitDownplace[placenum].transform.localScale.x);
        if (Physics2D.OverlapBox(hitDownplace[placenum].transform.position, boxcollidersize, 0, playerlayer))
        {
            player.GetComponent<Character>().HPChanged(damage,false,0);
        }
        Debug.Log(Physics2D.OverlapBox(hitDownplace[placenum].transform.position, boxcollidersize, 0, playerlayer)); 
        armLeft.transform.position = hitDownplace[placenum].transform.position;
        hitDownplace[placenum].SetActive(false);
        armLeft.transform.GetChild(0).gameObject.SetActive(true);
        yield return null;
    }
    IEnumerator ManaStoneSumon()
    {
        float time = 0;
        for(int i=0;i<4;i++)
        {
            gameObject.transform.parent.transform.Find(string.Format("보스방/ManaStonePlace{0}", i+1)).GetComponent<Animator>().Play("ManaStoneIdle");
        }
        Debug.Log("ManaStoneSumon");
        Phase2Timecheck = COPhase2Timecheck();
        StartCoroutine(Phase2Timecheck);
        while (time < 0.5)
        {
            armright.transform.localPosition = Vector3.Lerp(armright.transform.localPosition, new Vector3(-5, 0, 0), time * 2);
            armLeft.transform.localPosition = Vector3.Lerp(armLeft.transform.localPosition, new Vector3(5, 0, 0), time * 2);
            time += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
    IEnumerator ManaShoot1()
    {
        for(int i=0;i<4;i++)
        {
            float angle = GetAngle(player.transform.position, transform.position);
            Vector3 bossmouth = gameObject.transform.position;
            //해당 벡터 위치는 보스 입
            projectile.Create(angle, projectilespeed, projectiledamage, projectileanim, "ProjectileObj", true, bossmouth);
            yield return new WaitForSeconds(0.25f);
        }
        Debug.Log("ManaShoot1");
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator ManaShoot2()
    {
        Debug.Log("ManaShoot2");
        for (int j = 0; j < 9; j++)
        {
            float angle = 80 - (j * 20);
            Vector3 bossmouth = gameObject.transform.position;
            //해당 벡터 위치는 보스 입
            projectile.Create(angle, projectilespeed, projectiledamage, projectileanim, "ProjectileObj", true, bossmouth);
        }
        yield return new WaitForSeconds(1.0f);
    }
    
    IEnumerator COPhase2Timecheck()
    {
        for (int i = 0; i < 4; i++)
        {
            gameObject.transform.parent.transform.Find(string.Format("보스방/ManaStonePlace{0}/ManaStone", i + 1)).gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(15.0f);
        StopCoroutine(PhaseState);
        currentstate++;
        count = 0;
        StartCoroutine(Bossphasechange);
        Debug.Log("COPhase2Timecheck");
    }
    private void Update() {
        Vector3 bossroomvector;
        bossroomvector=Bossroom.transform.Find("배경임시작업").transform.position;
        Debug.DrawLine(bossroomvector+new Vector3(-10,3),bossroomvector+new Vector3(10,3));   
        Debug.DrawLine(bossroomvector+new Vector3(10,3),bossroomvector+new Vector3(10,-3));   
        Debug.DrawLine(bossroomvector+new Vector3(10,-3),bossroomvector+new Vector3(-10,-3));   
        Debug.DrawLine(bossroomvector+new Vector3(-10,-3),bossroomvector+new Vector3(-10,3));    
    }
    IEnumerator RockSumon()
    {
        //objectAnimator.Play("BossSlimePattern");
        Vector3 bossroomvector;
        bossroomvector=Bossroom.transform.Find("배경임시작업").transform.position;
        yield return new WaitForSeconds(3.0f);
        int random = Random.Range(0, 5);
        for (int i = 0; i < 5; i++)
        {
            RaycastHit2D[] hits;
            Vector3 targetposition;
            bool ishit = false;
            do
            {
                ishit = false;
                Random.InitState((int)System.DateTime.Now.Ticks);
                    
                //targetposition = new Vector3(Random.Range(viewportposition0.x, viewportposition1.x) * 0.1f, Random.Range(viewportposition0.y, viewportposition1.y) * 0.1f);
                targetposition = bossroomvector+new Vector3(Random.Range(-1000, +1000)*0.01f, (Random.Range(-300, +300) * 0.01f));
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
            yield return StartCoroutine(Projectile(targetposition, !i.Equals(random)));
        }
        yield return new WaitForSeconds(Idletime);
        Debug.Log("RockSumon");
        //StartCoroutine(Bossphasechange);
    }
    IEnumerator Chargingmana()
    {
        yield return null;
        Debug.Log("Chargingmana");
    }
    IEnumerator RazerBeam()
    {
        float time = 0;
        razer.SetActive(true);
        while (time < 3)
        {
            razer.transform.rotation = Quaternion.Euler(0,0,90+(time*60));
            time += Time.deltaTime;
            yield return null;
        }
        razer.SetActive(false);
        yield return null;
        Debug.Log("RazerBeam" + Bossphasechange.Current.ToString());
    }
    IEnumerator LeftSweep()
    {
        yield return null;
        Debug.Log("LeftSweep");
    }
    IEnumerator RightSweep()
    {
        yield return null;
        Debug.Log("RightSweep");
    }
    IEnumerator Roar()
    {
        yield return new WaitForSeconds(1.0f);
        objectAnimator.Play("MaDongSeokRoar");
        arms.GetComponent<Animator>().Play("Roar");
        yield return new WaitForSeconds(6.0f);
        Debug.Log("Roar");
    }
    public void TargeExplosion(Vector3 _targetpoint)
    {

        for (int i = 0; i < 5; i++)
        {
            projectile.Create(i * 72, projectilespeed, projectiledamage, projectileanim, "ProjectileObj", true, _targetpoint);
        }

    }
    IEnumerator Projectile(Vector3 projectileposition, bool _week)
    {
        targetpoint.Create(projectilespeed, projectiledamage, "TargetPointObj", projectileposition, _week);
        Debug.Log(_week);
        yield return new WaitForSeconds(projectiletime);
    }
    public float GetAngle(Vector3 Start, Vector3 End)
    {
        Vector3 v = End - Start;
        return Quaternion.FromToRotation(Vector3.up, End - Start).eulerAngles.z;
    }
    /// <summary>
    /// 1번상태
    /// </summary>
    /// <returns></returns>
    //IEnumerator State1()
    //{
    //    GameObject arm;
    //    int random = Random.Range(0, 2);
    //    if (random.Equals(0))
    //    {
    //        arm = armLeft;
    //    }
    //    else
    //    {
    //        arm = armright;
    //    }
    //    arm.GetComponent<Animator>().Play("MaDongSeokArmAttackReady");
    //    Debug.Log("phase1");
    //    yield return new WaitForSeconds(Idletime);
    //}
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="projectileposition"></param>
    ///// <returns></returns>

    ////탄환발사1
    //IEnumerator State3()
    //{
    //    int randomposition = Random.Range(1, 6);
    //    int randomsite = Random.Range(0, 2);
    //    for (int i = 0; i < 10; i++)
    //    {
    //        for (int j = 0; j < 7; j++)
    //        {
    //            int angle = 60 - (j * 20);
    //            if (!j.Equals(randomposition))
    //            {
    //                Vector3 bossmouth = gameObject.transform.position;
    //                //해당 벡터 위치는 보스 입
    //                projectile.Create(angle, projectilespeed, projectiledamage, projectileanim, "ProjectileObj", true, bossmouth);
    //            }

    //        }
    //        if (randomsite.Equals(0))
    //        {
    //            if (randomposition.Equals(1))
    //            {
    //                randomsite = 1;
    //                randomposition++;
    //            }
    //            else
    //            {
    //                randomposition--;
    //            }
    //        }
    //        else
    //        {
    //            if (randomposition.Equals(5))
    //            {
    //                randomsite = 0;
    //                randomposition--;
    //            }
    //            else
    //            {
    //                randomposition++;
    //            }
    //        }
    //        yield return new WaitForSeconds(1f);
    //    }
    //    yield return new WaitForSeconds(Idletime);
    //}

    ///// <summary>
    ///// 보스패턴4
    ///// </summary>
    ///// <returns></returns>
    //IEnumerator State4()
    //{
    //    objectAnimator.Play("BossSlimePattern");
    //    yield return new WaitForSeconds(3.0f);
    //    for (int i = 0; i < 4; i++)
    //    {
    //        RaycastHit2D[] hits;
    //        Vector3 targetposition;
    //        bool ishit = false;
    //        do
    //        {
    //            ishit = false;
    //            Random.InitState((int)System.DateTime.Now.Ticks);

    //            //targetposition = new Vector3(Random.Range(viewportposition0.x, viewportposition1.x) * 0.1f, Random.Range(viewportposition0.y, viewportposition1.y) * 0.1f);
    //            targetposition = new Vector3(Random.Range(-1200, +1200) * 0.01f, Random.Range(-400, +400) * 0.01f);
    //            hits = Physics2D.RaycastAll(targetposition, transform.forward);
    //            foreach (RaycastHit2D hit in hits)
    //            {
    //                if (hit.collider.GetComponent<Targetpoint>() != null)
    //                {
    //                    Debug.Log("겹침");
    //                    ishit = true;
    //                }
    //            }
    //        }
    //        while (ishit);
    //        yield return StartCoroutine("State2", targetposition);
    //    }
    //    yield return new WaitForSeconds(Idletime);
    //}
    ///// <summary>
    ///// 보스패턴 5
    ///// </summary>
    ///// <returns></returns>
    //IEnumerator State5()
    //{
    //    int randomsite = Random.Range(0, 2);
    //    for (int i = 0; i < 10; i++)
    //    {
    //        for (int j = 0; j < 9; j++)
    //        {
    //            int angle = 95 + (j * 20) + 10 * randomsite;
    //            Vector3 bossmouth = gameObject.transform.position;
    //            //해당 벡터 위치는 보스 입
    //            projectile.Create(angle, projectilespeed, projectiledamage, projectileanim, "ProjectileObj", true, bossmouth);
    //        }
    //        if (randomsite.Equals(0))
    //        {
    //            randomsite = 1;
    //        }
    //        else
    //        {
    //            randomsite = 0;
    //        }
    //        yield return new WaitForSeconds(1f);
    //    }
    //    yield return new WaitForSeconds(Idletime);
    //}
}
