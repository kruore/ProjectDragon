using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{    
    public Transform pfDamagePopup;

    static GameAssets _i;
    public static GameAssets i
    {
        get
        {
            if (_i == null)
            {
                _i=Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return _i;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


}
