
using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
   // public Grid grid;
    public Transform target;
    float speed = 1;
    Vector3[] path;
    int targetIndex;

    void Start()
    {
        StartCoroutine(FindPathAgain());    
    }
    IEnumerator FindPathAgain()
    {
        while(true)
        {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            //0.35초마다 찾는다.
            yield return new WaitForSeconds(0.35f);
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());

        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            //해당 위치로 이동
            //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }
    public void OnDrawGizmos()
    {
        //기즈모를 그려준다.
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}