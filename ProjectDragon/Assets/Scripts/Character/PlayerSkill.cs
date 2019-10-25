using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public UISprite My_sprite;
    public UIButton My_button;
    public UILabel My_Label;

    Ray ray;
    RaycastHit raycastHit;

    private void Awake()
    {
        My_button = gameObject.GetComponent<UIButton>();
        My_sprite = gameObject.GetComponent<UISprite>();
        My_sprite.fillAmount = 1;
        My_button.tweenTarget = null;
        My_Label = gameObject.transform.GetChild(1).GetComponent<UILabel>();
    }
    IEnumerator CoolTime(float cool)
    {
        print("쿨타임 코루틴 실행");
        float i = 0;
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

    }
    public void OnClick()
    {
        StartCoroutine(CoolTime(3.0f));
    }
}