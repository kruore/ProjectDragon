using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mr_Goburin : FSM_NormalEnemy
{
    protected override void Awake()
    {
        base.Awake();
        childDustParticle = transform.Find("DustParticle").gameObject;
    }


    void Start()
    {
        StartCoroutine(Start_On());
    }

    void Update()
    {
        DustParticleCheck();
    }

    //탄환 공격
    protected override IEnumerator Attack_On()
    {

        yield return null;

    }
}
