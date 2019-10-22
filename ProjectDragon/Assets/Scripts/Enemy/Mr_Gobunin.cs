using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mr_Gobunin : FSM_NormalEnemy
{

    private void Awake()
    {
        objectAnimator = gameObject.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        other = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Effect
        fadeOut = GetComponent<FadeOut>();
        damagePopup = new DamagePopup();
        flashWhite = GetComponent<FlashWhite>();
        childDustParticle = transform.Find("DustParticle").gameObject;
    }

    private void Start()
    {
        StartCoroutine(Start_On());
    }

    Projectile projectile;
    float projectileSpeed = 1;
    GameObject projectileObject;
    //Create Projectile 
    protected Projectile Create()
    {

        projectileObject = ObjectPool.Instance.PopFromPool("ProjectileObj",transform);
        projectile = projectileObject.transform.GetComponent<Projectile>();
        projectile.gameObject.SetActive(true);
        projectile.ProjectileInit(Angle, projectileSpeed, ATTACKDAMAGE, "ProjectileObj", true, transform.position);
        return projectile;

        //ObjectPool.Instance.PushToPool("ProjectileObj", projectileObject);

    }


    private void Update()
    {
        DustParticleCheck();

        if (Input.GetKeyDown(KeyCode.D))
        {
            Create();
        }
    }
    //탄환 공격
    protected override IEnumerator Attack_On()
    {
        yield return null;

    }

}
