using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : Monster
{
    [SerializeField]
    private float AttackTimer;
    // Start is called before the first frame update
    void Start()
    {
        monsterName = "TestMonster";
        enemyPos = EnemyPos.Front;
        AtkRange = 5;
        myState = State.WALK;
        myAttackType = AttackType.SHORTRANGE;
        moveDistance = 5;
    }

    // Update is called once per frame
    void Update()
    {
    }
    //몬스터의 애니메이션 제어 angle 값에 따라 스테이트 case 종속 모습 변경
    public void MonsterAnimation(float angle)
    {
        if (myAttackType.Equals(AttackType.SHORTRANGE))
        {
            switch (myState)
            {
                case State.WALK:
                    AnimatorCast(monsterName + AngleCalculate(angle));
                    break;
            }
        }
    }
   IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(AttackTimer);
    }
    public void MoveEnemy()
    {
        if(distanceOfPlayer < moveDistance)
        {
           myPos=gameObject.GetComponent<Transform>().position;
        }
    }
    
}
