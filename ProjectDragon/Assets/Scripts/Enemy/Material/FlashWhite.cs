using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashWhite : MonoBehaviour
{
    float delay = 0.05f;
    Material originalMaterial;
    public Material effectMaterial;
    SpriteRenderer render;

    private void Awake()
    {
        render = this.GetComponent<SpriteRenderer>();
        originalMaterial = render.material;

    }

    public IEnumerator Flash()
    {
        render.material = effectMaterial;

        yield return new WaitForSeconds(delay);

        render.material = originalMaterial;
        yield return null;
    }


}
