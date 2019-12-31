
// ==============================================================
// Map Objects parent class
// The parent of the interactive map object
// 
//  AUTHOR: Kim Dong Ha
// CREATED: 2019-12-31
// UPDATED: 2019-12-31
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public float hp = 1;
   
    protected virtual void HPChanged(float _damage)
    {
        hp -= _damage;
    }

    protected virtual IEnumerator Effect()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;

        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
