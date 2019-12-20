using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mr_Gobunin : FSM_NormalEnemy
{
    public RuntimeAnimatorController projectileAnimator;
    protected override void Awake()
    {
        base.Awake();
        childDustParticle = transform.Find("DustParticle").gameObject;
        projectile = new Projectile();
    }



    ////Create Projectile 
    //protected Projectile Create()
    //{

    //    projectileObject = ObjectPool.Instance.PopFromPool("ProjectileObj",transform);
    //    projectile = projectileObject.transform.GetComponent<Projectile>();
    //    projectile.gameObject.SetActive(true);
    //    projectile.ProjectileInit(Angle, projectileSpeed, ATTACKDAMAGE, "ProjectileObj", true, transform.position);
    //    return projectile;

    //    //ObjectPool.Instance.PushToPool("ProjectileObj", projectileObject);

    //}

    Projectile projectile;
    private void Update()
    {
        DustParticleCheck();

        if (Input.GetKeyDown(KeyCode.D))
        {
            projectile.Create(Angle, 1, ATTACKDAMAGE, projectileAnimator, "ProjectileObj", true, transform.position, transform);
        }
    }
    //탄환 공격
    protected override IEnumerator Attack_On()
    {
        yield return null;

    }
}

