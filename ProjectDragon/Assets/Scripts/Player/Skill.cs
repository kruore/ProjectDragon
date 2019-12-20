using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skillName = "TestSkill";
    public Player player;
    public GameObject playerChar;
    public bool StopPlayer = false;
    float my_Timer = 0.0f;


    public Vector3 myAngle = new Vector3(0, 0, -90);
    public Vector3 nowPos;
    public float PlayerAngle;

    public void Start()
    {
        gameObject.transform.rotation = Quaternion.Euler(myAngle);
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(5, 1);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerChar = GameObject.FindGameObjectWithTag("Player");
        PlayerAngle = player.enemy_angle;
    }
    public void Update()
    {
        PlayerAngle = player.enemy_angle;
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, PlayerAngle - 90);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Enemy"))
        {
            collision.GetComponent<Character>().HPChanged(30);

        }
    }
}