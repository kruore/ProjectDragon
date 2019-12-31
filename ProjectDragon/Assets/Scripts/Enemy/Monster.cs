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
    IEnumerator StartONE;
    public virtual IEnumerator Start_On()
    {
        yield return null;
    }

    protected override void Awake()
    {
        objectAnimator = gameObject.GetComponent<Animator>();
        damagePopup = new DamagePopup();
        flashWhite = GetComponent<FlashWhite>();
        StartONE = Start_On();
        base.Awake();
     //   Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), true);
    }
    public void Starton()
    {
        StartCoroutine(StartONE);
    }
    public void StartonStop()
    {
        StopCoroutine(StartONE);
    }
    public override int HPChanged(int ATK)
    {
        if (!isDead)
        {
            //데미지 띄우기
            damagePopup.Create(transform.position + new Vector3(0.0f, 0.5f, 0.0f), ATK, false, transform);
            return base.HPChanged(ATK);
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
