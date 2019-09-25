﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareEnemy : Monster
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
        if (isSkillActive)
        {
            //상태 1 -> 상태2
            //Ex) Burrow Anim 시작
            isSkillActive = false;
            yield return new WaitForSeconds(skillCooltime);

        }
        else
        {
            yield return new WaitForSeconds(cooltime);

        }
        Debug.Log("RareEnemy Cooltime");
        myState = State.None;
        yield return null;
    }



}
