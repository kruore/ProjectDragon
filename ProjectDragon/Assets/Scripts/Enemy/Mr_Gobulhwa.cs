using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mr_Gobulhwa : FSM_NormalEnemy
{
    protected override void Awake()
    {
        base.Awake();
        childDustParticle = transform.Find("DustParticle").gameObject;
    }



    void Update()
    {
        DustParticleCheck();
    }

    //탄환 공격
    protected override void Attack_On()
    {
    }
}
