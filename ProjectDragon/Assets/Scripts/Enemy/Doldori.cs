using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doldori : FSM_NormalEnemy
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
