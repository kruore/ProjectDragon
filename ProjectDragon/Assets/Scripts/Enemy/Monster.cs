using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : PlayerCharacter
{
    // Start is called before the first frame update
    void Start()
    {
        PLAYERHP = 100;
        ATKSPEED = 10;
        ATKChanger(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
