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
    // Start is called before the first frame update
    void Start()
    {
        objectAnimator = gameObject.GetComponent<Animator>();
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
    public void MonsterAnimaionSetting(string monsterName)
    {
        switch(enemyPos)
        {
            case EnemyPos.Front :
                objectAnimator.Play(monsterName + "Front");
                break;
            case EnemyPos.Right:
                objectAnimator.Play(monsterName + "Right");
                break;
            case EnemyPos.RightSide:
                objectAnimator.Play(monsterName + "RightSide");
                break;
            case EnemyPos.Left:
                objectAnimator.Play(monsterName + "Left");
                break;
            case EnemyPos.LeftSide:
                objectAnimator.Play(monsterName + "LeftSide");
                break;
            case EnemyPos.Up:
                objectAnimator.Play(monsterName + "Up");
                break;
        }
    }
}
