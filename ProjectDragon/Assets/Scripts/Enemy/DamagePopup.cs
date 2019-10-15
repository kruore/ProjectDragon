using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{

    [SerializeField] float moveYSpeed = 3.0f;
    [SerializeField] float disappearSpeed = 7.0f;
    [SerializeField] float disappearTimer = 0.3f;

    Color textColor;
    TextMeshPro textMesh;
    Transform damagePopupTransform;
    DamagePopup damagePopup;

    public DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        damagePopup = damagePopupTransform.GetComponent<DamagePopup>();

        damagePopup.Setup(damageAmount,isCriticalHit);

        return damagePopup;
    }

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount,bool isCriticalHit)
    {
        textMesh.SetText(damageAmount.ToString());
        if (!isCriticalHit)
        {
            //Normal Hit
            textMesh.fontSize = textMesh.fontSize;
            textColor = textMesh.color;  //예시
        }
        else
        {
            //Ciritical Hit
            textMesh.fontSize += 3;
            textColor = textMesh.color;  //예시
        }
        textMesh.color = textColor;
    }

    float increaseScaleAmount = 1f;
    float decreaseScaleAmount = 1f;

    private void Update()
    {
        //Y axis move
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;

        //if (disappearTimer > disappearTimer * 0.5f)
        //{
        //    transform.localScale += Vector3.Lerp();
        //}


        //Fade Out
        if (disappearTimer < 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Debug.Log("Destroy");
                Destroy(gameObject);
            }
        }
    }


}
