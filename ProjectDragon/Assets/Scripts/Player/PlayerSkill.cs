//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public UISprite My_sprite;
    public UIButton My_button;
    public UILabel My_Label;
    public Player My_Player;
    public GameObject skill;
    Ray ray;
    RaycastHit raycastHit;
    IEnumerator co;
    IEnumerator sk;

    private void Awake()
    {
        My_button = gameObject.GetComponent<UIButton>();
        My_sprite = gameObject.GetComponent<UISprite>();
        My_sprite.fillAmount = 1;
        My_button.tweenTarget = null;
        My_Label = gameObject.transform.GetChild(1).GetComponent<UILabel>();
        My_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        co = CoolTime(3);
        sk = SkillDamaged();
        //skill = GameObject.Find("TestSkill").GetComponent<Skill>();
    }
    IEnumerator CoolTime(float cool)
    {
        print("쿨타임 코루틴");
        float i = 0;
        My_Player.StopPlayer = true;
        while (cool > i)
        {
            My_Label.gameObject.SetActive(true);
            i += Time.deltaTime;
            My_sprite.fillAmount = ((i / cool));

            My_button.isEnabled = false;
            My_Label.text = Mathf.FloorToInt(1 + (cool - i)).ToString();
            yield return new WaitForFixedUpdate();
        }
        print("쿨타임 코루틴 완료");
        My_button.isEnabled = true;
        My_Label.gameObject.SetActive(false);
        StopCoroutine(co);
        StopCoroutine(sk);
    }
    public void OnClick()
    {
        if(My_Player.mp/10>0)
        {
            My_Player.MPChanged(5);
            co = CoolTime(3);
            sk = SkillDamaged();
            StartCoroutine(sk);
        }
        else
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
    public void OnPress(bool isPressed)
    {
       if(isPressed)
        {
            Debug.Log("TMLQM");
        }
    }

    IEnumerator SkillDamaged()
    {
        skill.gameObject.SetActive(true);
        MPControll();
        StartCoroutine(co);
        yield return new WaitForSeconds(0.2f);
        skill.gameObject.SetActive(false);
    }
    void MPControll()
    {
        float maxMp = My_Player.maxMp;
        float myMp = My_Player.mp;
        float Per = myMp / maxMp;
        Debug.Log(myMp+">>>"+maxMp+":"+Per);
    }
}