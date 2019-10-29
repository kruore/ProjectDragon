using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{

    public string poolItemName = "DamagePopupObj";

    [SerializeField] float moveYSpeed = 2.0f;
    [SerializeField] float disappearSpeed = 7.0f;
    [SerializeField] float disappearTimer = 0.2f;

    Color textColor;
    TextMeshPro textMesh;
    DamagePopup damagePopup;

    Transform _parent;

    void Initialize()
    {
        moveYSpeed = 3.0f;
        disappearSpeed = 7.0f;
        disappearTimer = 0.3f;
        textColor.a = 1.0f;
        textMesh.color = Color.white ;
    }

    public DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit, Transform parent = null)
    {
        //damageObject.transform.position = position;
        //damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        //damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        
        _parent = parent;
        GameObject damageObject = ObjectPool.Instance.PopFromPool(poolItemName, parent);
        damagePopup = damageObject.transform.GetComponent<DamagePopup>();
        damagePopup.Initialize();
        damagePopup.transform.position = position;
        damagePopup.Setup(damageAmount,isCriticalHit);
        damageObject.SetActive(true);

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
                //Object
                ObjectPool.Instance.PushToPool(poolItemName, gameObject, _parent);
                //Destroy(gameObject);
            }
        }
    }


}
