using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{

    t_PathFinding pathFinding;
    public t_Node [] findPathNode;
    int pathNextIndex;

    //임시
    float moveSpeed = 1.0f;
    bool isWalking = true;

    public Transform targetPos;
    private void Awake()
    {
        GameObject AStar = GameObject.Find("AStar");
        pathFinding = new t_PathFinding();
        targetPos = GameObject.FindGameObjectWithTag("Player").transform;
        
        //grid = AStar.transform.GetComponent<t_Grid>();
    }
    void Start()
    {
       pathFinding.FindPath(transform.position, targetPos.position);
       StartCoroutine(FindPath());
       StartCoroutine(Move());
    }


    //find path
    IEnumerator FindPath()
    {
        while(true)
        {
            if (pathFinding.grid != null && pathFinding.finalPath.Count > 0)
            {
                Debug.Log("FindPath");
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
            Debug.Log(findPathNode[0].Pos.ToString());

            while (isWalking)
            {
                if (Vector3.Distance(transform.position, currentWaypoint) < 0.1)  //오차범위 0.1
                {
                    pathNextIndex++;
                    if (pathNextIndex >= findPathNode.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = findPathNode[pathNextIndex].Pos;
                }

                Debug.Log("Move");
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

    }
}
