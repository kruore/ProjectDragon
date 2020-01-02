using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScreenTransitions : MonoBehaviour
{
    public Material TransitionMaterial;
    public float transitionTime = 1.5f;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);
    }

    private void Start()
    {
        StartCoroutine(EffectStart("Textures/criss_cross_pattern"));
    }


    public IEnumerator EffectStart(string path)
    {
        TransitionMaterial.SetTexture("_TransitionTex", Resources.Load<Texture>(path));

        float cutoff = 0f;
        float time = 0;

        while (time <= transitionTime)
        {
            time += Time.deltaTime;
            cutoff = Mathf.Lerp(0.0f, 1.0f, time / transitionTime);
            TransitionMaterial.SetFloat("_Cutoff", cutoff);

            yield return null;
        }

    }
}
