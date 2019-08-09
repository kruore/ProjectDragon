using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    public enum EnemyPos { None= 0, Front ,Right, RightSide , Left , LeftSide , Up};
    public float skillCoolDown = 10.0f;
    public bool IsSkillActive = false;
    public string monsterName;
    public Animator objectAnimator;
    protected EnemyPos enemyPos;
    public float moveDistance;
 

    //플레이어와 적과의 거리 캐스팅
    public float distanceOfPlayer;
    public float angleOfPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        objectAnimator = gameObject.GetComponent<Animator>();
        other = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Puppet_skill 01 is Move Back
    IEnumerator Puppet_Skill01()
    {
        //  if(IsSkillActive.Equals(true)) ? yield return new WaitForSeconds(skillCoolDown) :  yield return new WaitForSeconds(skillCoolDown);
        {
            yield return new WaitForSeconds(skillCoolDown);
        }
        yield return new WaitForSeconds(0);
    }
    public void MonsterAnimation(float angle)
    {
        if (myAttackType.Equals(AttackType.ShortRange))
        {
            switch (myState)
            {
                case State.Walk:
                    AnimatorCast(monsterName + AngleCalculate(angle));
                    break;
            }
        }
    }
    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(ATTACKSPEED);
    }
}
