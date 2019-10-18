using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FadeOut : MonoBehaviour
{

    [SerializeField] float disappearSpeed;
    [SerializeField] public float disappearTimer;
    [SerializeField] bool isVisible = true;

    [HideInInspector] public Color fadecolor;



    private void Awake()
    {
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            fadecolor = GetComponent<SpriteRenderer>().color;
        }
        else if (gameObject.GetComponent<TextMeshPro>() != null)
        {
            fadecolor = GetComponent<TextMeshPro>().color;
        }
    }
    public void TimeInitialize()
    {
        disappearSpeed = 7;
        disappearTimer = 0.3f;
        fadecolor.a=1.0f;

    }

    public IEnumerator FadeOut_Cor(SpriteRenderer spriteRenderer = null, TextMeshPro textMesh = null)
    {

        Debug.Log("코루틴");
        while (isVisible)
        {
            if (spriteRenderer != null)
            {
                if (spriteRenderer.color.a >= 0)
                {
                    isVisible = true;
                }
                else
                {
                    isVisible = false;

                }
            }
            else
            {
                if (textMesh.color.a >= 0)
                {
                    isVisible = true;
                }
                else
                {
                    isVisible = false;
                }
            }




            //Fade Out
            disappearTimer -= Time.deltaTime;
            if (disappearTimer < 0)
            {
                fadecolor.a -= disappearSpeed * Time.deltaTime;

                if (spriteRenderer != null)
                {
                    spriteRenderer.color = fadecolor;
                }
                else
                {
                    textMesh.color = fadecolor;
                }
            }

            yield return null;
        }

    }
}
