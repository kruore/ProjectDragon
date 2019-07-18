using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : Monster
{
    // Start is called before the first frame update
    void Start()
    {
        monsterName = gameObject.name.ToString();
        enemyPos = EnemyPos.Front;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
