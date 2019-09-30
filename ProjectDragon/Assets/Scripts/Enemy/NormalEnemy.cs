using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Monster
{
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override IEnumerator AttackCooltime()
    {
        yield return new WaitForSeconds(cooltime);

        if (myState==State.None)
        {
            objectAnimator.SetTrigger("Attack");
            objectAnimator.SetBool("isAttackActive", true);
            objectAnimator.SetBool("Walk", false);
            myState = State.Attack;


        }
        yield return null;
    }

    protected void Invincibility()
    {

    }


}
