using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Player playerObject;
    public string poolItemName = "Skill";
    public float moveSpeed = 10f;
    public float lifeTime = 3f;
    public float _elapsedTime = 0f;
    public float skill_angle;

    public Vector3 normalVec = new Vector3(1, 1, 1);
    public float Timer
    {
        get
        {
            gameObject.SetActive(true);
            return (_elapsedTime += Time.deltaTime);
        }
        set
        {
            _elapsedTime = 0;
            if(_elapsedTime.Equals(0))
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void Start()
    {
        skill_angle = playerObject.enemy_angle;
        transform.rotation = Quaternion.Euler(0, 0, skill_angle);
    }
    void Update()
    { 
        transform.position += -(transform.up) * moveSpeed * Time.deltaTime;

        if (Timer > lifeTime)
        {
            Timer = 0;
        }
       
    }
}