/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-16////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mr_Gobulhwa : FSM_NormalEnemy
{
    public RuntimeAnimatorController projectileAnimator;
    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<BoxCollider2D>();
        childDustParticle = transform.Find("DustParticle").gameObject;
        projectile = new Projectile();
        //projectile = this.gameObject.AddComponent<Projectile>();

    }


    Projectile projectile;
    private void Update()
    {
        DustParticleCheck();
    }
    //애니메이션 프레임에 넣기 (탄환 공격)
    protected override void Attack_On()
    {
        projectile.Create(Angle-30, 3, ATTACKDAMAGE, projectileAnimator, "ProjectileObj", false, transform.position, transform);
        projectile.Create(Angle, 3, ATTACKDAMAGE, projectileAnimator, "ProjectileObj", false, transform.position, transform);
        projectile.Create(Angle+30, 3, ATTACKDAMAGE, projectileAnimator, "ProjectileObj", false, transform.position, transform);

    }
}
