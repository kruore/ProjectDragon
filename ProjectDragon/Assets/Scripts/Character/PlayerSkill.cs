using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public enum SKILLTYPE { None= 0, DASHATTACK,SHOOTINGATTACK,CHARGINGATTACK};

    public int skillDamage;
    public Vector3 skillRange;
    public int skillSpeed;
    public float skillCoolDown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
