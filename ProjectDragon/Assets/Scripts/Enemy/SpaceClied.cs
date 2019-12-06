using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceClied : MonoBehaviour
{
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }
#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.HP += 3000;
            player.maxHp = 3000;
        }
    }
#endif
    // Update is called once per frame
    public void HPUP()
    {
        player.HP = 50;
        Debug.Log("HP");
    }
}
