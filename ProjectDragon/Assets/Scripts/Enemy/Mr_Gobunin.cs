using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mr_Gobunin : FSM_NormalEnemy
{
    private void Awake()
    {
        objectAnimator = gameObject.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        other = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //탄환 생성
    protected override IEnumerator Attack_On()
    {

        yield return null;

    }
}
