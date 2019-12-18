/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-18////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mr_Gobulhwa : FSM_NormalEnemy
{
    public RuntimeAnimatorController projectileAnimator;
    BoxCollider2D boxCol;

    protected override void Awake()
    {
        base.Awake();
        boxCol = GetComponent<BoxCollider2D>();
        col = boxCol;
        childDustParticle = transform.Find("DustParticle").gameObject;
        projectile = new Projectile();
        //projectile = this.gameObject.AddComponent<Projectile>();

    }

    protected override RaycastHit2D[] GetRaycastType()
    {
        //BoxCast
        return Physics2D.BoxCastAll(startingPosition, boxCol.size, 0, direction, AtkRange - originOffset, m_viewTargetMask);
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
