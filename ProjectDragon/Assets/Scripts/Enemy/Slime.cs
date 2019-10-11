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


        //Normal Enemy 초기화
        HP = maxHp = 100;
        ATTACKDAMAGE = 10;
        ATTACKSPEED = 1;
        MoveSpeed = 1;
        AtkRange = 0.5f;
        name = "Slime";
        waitTime = 2.0f;
        cooltime = 3.0f;
    }
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Start_On());
    }

    protected override IEnumerator Attack_On()
    {
        if(inAtkDetectionRange)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE);
            Debug.Log("Player hit!!");
        }
        yield return null;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }



}
