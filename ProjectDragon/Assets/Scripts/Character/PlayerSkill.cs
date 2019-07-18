using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public enum SKILLTYPE { None= 0, DASHATTACK,SHOOTINGATTACK,CHARGINGATTACK};
    public enum SKILLSTATE { None = 0, START,RUN,COOLDOWN,END}

    public SKILLSTATE skillState;
    public int skillDamage;
    public Vector3 skillRange;
    public int skillSpeed;
    public int skillCoolDown;
    //public bool skill
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (skillState)
        {
            case SKILLSTATE.START :
                // skillCoolDown = 
                break;
            case SKILLSTATE.RUN: 
                //Start Animation
                
                break;
            case SKILLSTATE.COOLDOWN:
                if(skillCoolDown <=0)
                {
                    skillState=SKILLSTATE.END;
                }
                break;
            case SKILLSTATE.END:
                Destroy(this);
                break;
        }
    }
}
