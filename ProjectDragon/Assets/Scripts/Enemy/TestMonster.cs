using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : Monster
{
    // Start is called before the first frame update
    void Start()
    {
        name = "TestMonster";
        enemyPos = EnemyPos.Front;
        AtkRange = 3;
        myState = State.Walk;
        myAttackType = AttackType.ShortRange;
        MoveSpeed = 3;
        moveDistance = 10;
    }

    // Update is called once per frame
    void Update()
    {
    }
    
}
