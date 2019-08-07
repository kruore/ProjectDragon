using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public enum SKILLTYPE { None= 0, DASHATTACK,SHOOTINGATTACK,CHARGINGATTACK};
    public enum SKILLSTATE { None = 0, START,RUN,COOLDOWN,END}
    public string skillName;
    public SKILLSTATE skillState;
    public int skillDamage;
    public Vector3 skillRange;
    public int skillSpeed;
    public int skillCoolDown;
    public GameObject player;
    public GameObject skillPref;
    //public bool skill
    // Start is called before the first frame update
    void Start()
    {
        //장비 장착은 여기서
        //skillName = Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum].name.ToString();
        skillName = "BladeSkill01";
        player = GameObject.FindGameObjectWithTag("Player");
        skillPref = Resources.Load(skillName) as GameObject;
    }

    //TODO: 스킬 제작하기

    public void SkillSetUp()
    {
        //Instantiate(skillPref,player.transform.position,Quaternion.identity);
        GameObject skill = ObjectPool.Instance.PopFromPool(skillName);
        skill.transform.position = transform.position + transform.up;
        skill.SetActive(true);
    }
    // Update is called once per frame
    //void Update()
    //{
    //    switch (skillState)
    //    {
    //        case SKILLSTATE.START :
    //            // skillCoolDown = 
    //            break;
    //        case SKILLSTATE.RUN: 
    //            //Start Animation

    //            break;
    //        case SKILLSTATE.COOLDOWN:
    //            if(skillCoolDown <=0)
    //            {
    //                skillState=SKILLSTATE.END;
    //            }
    //            break;
    //        case SKILLSTATE.END:
    //            Destroy(this);
    //            break;
    //    }
    //}
}
