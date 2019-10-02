using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : FSM_NormalEnemy
{
    private void Awake()
    {
        //base.Awake();
        objectAnimator = gameObject.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        other = GameObject.FindGameObjectWithTag("Player").transform;

    }
    // Start is called before the first frame update
    private void Start()
    {
        //base.Start();
        StartCoroutine(Start_On());
    }

    protected override IEnumerator Attack_On()
    {

        yield return null;

    }

    protected override IEnumerator AttackCooltime()
    {
        yield return new WaitForSeconds(cooltime);

        if (CurrentState == State.None)
        {
            isAttackActive = true;
            CurrentState = State.Attack;

        }
        yield return null;
    }




}
