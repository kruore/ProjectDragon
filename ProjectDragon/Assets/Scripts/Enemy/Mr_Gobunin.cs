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
    }

    Projectile projectile;
    float projectileSpeed;
    //Create Projectile 
    protected Projectile Create()
    {
        Debug.Log("Create Projectile");
        GameObject projectileObject = ObjectPool.Instance.PopFromPool("ProjectileObj");
        projectile = projectileObject.transform.GetComponent<Projectile>();
        projectile.gameObject.SetActive(true);
        projectile.ProjectileInit(0, projectileSpeed, ATTACKDAMAGE, "ProjectileObj", true, gameObject.transform.position);
        return projectile;

        //ObjectPool.Instance.PushToPool("ProjectileObj", projectileObject);

    }

    //탄환 생성
    protected override IEnumerator Attack_On()
    {

        yield return null;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {

        Create();
        }
    }
}
