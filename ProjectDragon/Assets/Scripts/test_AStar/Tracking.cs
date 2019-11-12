using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{

    t_Grid grid;
    float moveSpeed = 1.0f;
    t_Node [] findPathNode;
    int pathNextIndex;
    bool isWalking = true;

    private void Awake()
    {
        GameObject AStar = GameObject.Find("AStar");
        grid = AStar.transform.GetComponent<t_Grid>();
    }
    void Start()
    {
       StartCoroutine(FindPath());
       StartCoroutine(Move());
    }


    //find path
    IEnumerator FindPath()
    {
        while(true)
        {
            if (grid != null && grid.finalPath.Count > 0)
            {
                findPathNode = grid.finalPath.ToArray();
            }
                yield return new WaitForSeconds(0.35f);
        }
    }

    IEnumerator Move()
    {
        if (grid != null && grid.finalPath.Count > 0)
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

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

    }
}
