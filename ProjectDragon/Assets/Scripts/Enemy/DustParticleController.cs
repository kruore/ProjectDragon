using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticleController : MonoBehaviour
{
    [SerializeField]
    ParticleSystemRenderer particleSystemRenderer;

    private void Awake()
    {

        particleSystemRenderer = transform.GetComponentInChildren<ParticleSystemRenderer>();
      
        particleSystemRenderer.gameObject.SetActive(false);
    }


    void KnockBackParticle()
    {
        particleSystemRenderer.sortingOrder = 2;
    }
    void MoveParticle()
    {
        particleSystemRenderer.sortingOrder = 0;
    }

    public void DustParticleCheck(bool Actuation , bool isHit)
    {
        if(isHit)
        {
            KnockBackParticle();
        }
        else
        {
            MoveParticle();
        }
        particleSystemRenderer.gameObject.SetActive(Actuation);
    }
}
