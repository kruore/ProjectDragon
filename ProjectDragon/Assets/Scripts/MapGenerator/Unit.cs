
using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    // public Grid grid;
     Vector3[] path;
     int targetIndex;

    [SerializeField]
     Vector3 preTarget;
    [SerializeField]
     Transform target;
    public float moveSpeed=1.0f;
    //float speed = 0.2f;

    //추가함!
    [SerializeField]
     Vector3 currentWaypoint;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        //StartCoroutine(FindPathAgain());    
    }

    public IEnumerator FindPathAgain()
    {
        
        while (true)
        {
            preTarget = target.position;
            //1초마다 찾는다.
            yield return new WaitForSeconds(1.0f);

            if (target.position != preTarget)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            }
            yield return null;

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
        currentWaypoint = path[0];

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
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
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