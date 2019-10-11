using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : FSM_NormalEnemy
{
    private void Awake()
    {
        objectAnimator = gameObject.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        other = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();



        ////Normal Enemy 초기화
        //HP = maxHp = 100;
        //ATTACKDAMAGE = 10;
        //ATTACKSPEED = 1;
        //MoveSpeed = 1;
        //AtkRange = 0.5f;
        //name = "Slime";
        //waitTime = 2.0f;
        //cooltime = 3.0f;
    }
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Start_On());

    }

    //애니메이션 프레임에 넣기
    protected override IEnumerator Attack_On()
    {
        if(inAtkDetectionRange)
        {
            Debug.Log("Enemy Attack_On");
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE);
        }
        yield return null;

    }

    



}
