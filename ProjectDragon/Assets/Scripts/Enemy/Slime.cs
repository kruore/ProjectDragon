using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : FSM_NormalEnemy
{
    protected override void Awake()
    {
        base.Awake();
        childDustParticle = transform.Find("DustParticle").gameObject;
    }
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Start_On());
    }

    //애니메이션 프레임에 넣기
    protected override IEnumerator Attack_On()
    {
        if (inAtkDetectionRange)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE);
        }
        yield return null;

    }

    public void Update()
    {
        DustParticleCheck();

        //test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HPChanged(1);
        }

    }





}