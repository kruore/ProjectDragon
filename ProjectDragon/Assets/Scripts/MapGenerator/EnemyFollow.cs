using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed;
    private Transform target;
    public float stopDistance;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine("StartToMove");
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag== "Enemy")
        {
            StartCoroutine("StopMovement");
        }

    }
    public void MoveToPlayer()
    {
        if (Vector2.Distance(transform.position, target.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        }
    }
    IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator StartToMove()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
        }
    }
}
