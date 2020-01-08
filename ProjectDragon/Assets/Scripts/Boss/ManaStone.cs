using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaStone : Monster
{
    public Boss_MaDongSeok boss;
    // Start is called before the first frame update
    public void ManaStoneSumon()
    {
        boss.transform.parent.GetComponent<Room>().AddMonster(gameObject);
        //Room.addMonster(gameObject.transform.Find("ManaStone").gameObject);
        gameObject.transform.parent.GetComponent<Animator>().Play("ManaStoneIdle");
        maxHp=30;
        HP=30;
    }
    public override void Dead()
    {
        gameObject.SetActive(false);
        bool active = false;
        if (boss.currentstate.Equals(BossState.Phase2))
        {
            for (int i = 0; i < 4; i++)
            {
                if (gameObject.transform.parent.parent.transform.Find(string.Format("ManaStonePlace{0}", i + 1)).Find("ManaStone").gameObject.activeSelf)
                {
                    active = true;
                }
            }
            if (!active)
            {
                boss.HPChanged(25, false, 0);
            }
            base.Dead();
        }
    }
    public override int HPChanged(int ATK, bool isCritical, int NukBack)
    {
        if (ATK > 0 && HP > 0)
        {
            IEnumerator flash = GetComponentInChildren<FlashWhite>().Flash();
            StartCoroutine(flash);
            Debug.Log("flash");
        }
        return base.HPChanged(ATK, isCritical, NukBack);
    }
}
