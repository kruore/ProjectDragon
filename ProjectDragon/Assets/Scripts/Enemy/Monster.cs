/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-13////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Monster : Character
{

    protected Animator objectAnimator;
    //Effect
    protected FlashWhite flashWhite;
    protected DamagePopup damagePopup;


    protected override void Awake()
    {
        objectAnimator = gameObject.GetComponent<Animator>();
        damagePopup = new DamagePopup();
        flashWhite = GetComponent<FlashWhite>();

        base.Awake();

     //   Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), true);
    }
    public override int HPChanged(int ATK, int NukBack, bool isNukBack)
    {
        if (!isDead)
        {
            //데미지 띄우기
            damagePopup.Create(transform.position + new Vector3(0.0f, 0.5f, 0.0f), ATK, false, transform);
            return base.HPChanged(ATK, NukBack, isNukBack);
        }
        return 0;
    }


    //삭제할것
    //플레이어와 적과의 거리 캐스팅

    public float distanceOfPlayer;
    [HideInInspector]
    public float angleOfPlayer;
    [HideInInspector]
    public float moveDistance;

}
