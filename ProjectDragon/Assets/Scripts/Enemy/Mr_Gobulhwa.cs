// ==============================================================
// Cracked Mr_Gobulhwa
//
//  AUTHOR: Yang SeEun
// CREATED: 
// UPDATED: 2019-12-18
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mr_Gobulhwa : FSM_NormalEnemy
{

    CircleCollider2D circleCol;

    //object
    Projectile projectile;
    public RuntimeAnimatorController projectileAnimator;

    protected override void Awake()
    {
        base.Awake();
        circleCol = GetComponent<CircleCollider2D>();
        col = circleCol;
        childDustParticle = transform.Find("DustParticle").gameObject;
        projectile = new Projectile();
        //projectile = this.gameObject.AddComponent<Projectile>();

    }

    protected override RaycastHit2D[] GetRaycastType()
    {
        //CircleCast
        return Physics2D.CircleCastAll(startingPosition, circleCol.radius, direction, AtkRange - originOffset, m_viewTargetMask);
    }

    private void Update()
    {
        DustParticleCheck();
    }

    /// <summary>
    /// 탄환 생성 (애니메이션 프레임에 넣기)
    /// </summary>
    ///  In this case you choose event based on the clip weight
    public void Attack_On()
    {

        projectile.Create(Angle - 30, 3.0f, ATTACKDAMAGE, projectileAnimator, "ProjectileObj", false, transform.position);
        projectile.Create(Angle, 3.0f, ATTACKDAMAGE, projectileAnimator, "ProjectileObj", false, transform.position);
        projectile.Create(Angle + 30, 3.0f, ATTACKDAMAGE, projectileAnimator, "ProjectileObj", false, transform.position);

    }
   
}
