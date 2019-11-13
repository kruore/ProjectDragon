using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    public t_PathFinding pathFinding;

    public t_Node [] findPathNode;
    int pathNextIndex;
    BoxCollider2D m_boxCollider;

    //임시
    float moveSpeed = 1.0f;
    bool isWalking = true;
    public Transform targetPos;


    private void Awake()
    {
        pathFinding = new t_PathFinding();
        m_boxCollider = GetComponent<BoxCollider2D>();

        //임시
        targetPos = GameObject.FindGameObjectWithTag("Player").transform;

    }
    void Start()
    {
       pathFinding.FindPath(transform.position, targetPos.position, m_boxCollider.size.x,m_boxCollider.size.y);
       StartCoroutine(FindPath());
       StartCoroutine(Move());
    }
    private void Update()
    {
        
    }


    //find path
    IEnumerator FindPath()
    {
        while(true)
        {
            if (pathFinding.grid != null && pathFinding.finalPath.Count > 0)
            {
                //Debug.Log("FindPath");
                findPathNode = pathFinding.finalPath.ToArray();
            }
                yield return new WaitForSeconds(0.35f);
        }
    }

    IEnumerator Move()
    {
        if (pathFinding.grid != null && pathFinding.finalPath.Count > 0)
        {
            Vector3 currentWaypoint = findPathNode[0].Pos;

            while (isWalking)
            {
                if (Vector3.Distance(transform.position, currentWaypoint) ==0.0f)  //오차범위 0.1
                {
                    pathNextIndex++;
                    if (pathNextIndex >= findPathNode.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = findPathNode[pathNextIndex].Pos;
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

    }
}
