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
        AtkRange = 3;
        myState = State.WALK;
        myAttackType = AttackType.SHORTRANGE;
        moveDistance = 5;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void MonsterAnimation(float angle)
    {
        if (myAttackType.Equals(AttackType.SHORTRANGE))
        {
            Debug.Log("check");
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
