using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracking : MonoBehaviour
{
    t_PathFinding pathFinding;
    t_Grid grid;
    Vector3 targetPosition;

    private void Awake()
    {
        pathFinding = new t_PathFinding();
        grid = GetComponent<t_Grid>();
    }

    //public static void PathManager(Vector3 _startPos, Vector3 _targetPos)
    //{
    //    pathFinding.FindPath(_startPos, _targetPos);
    //}

    public void Tracking(Vector3 _startPos, float _moveSpeed)
    {
        if (grid != null && grid.finalPath.Count > 0)
        {

            Debug.Log(_startPos.ToString());
            targetPosition = grid.finalPath[0].Pos;
            transform.position = Vector3.MoveTowards(_startPos, targetPosition, _moveSpeed * Time.deltaTime);
            Debug.Log("targetPosition   " + targetPosition.ToString());

        }
    }

    private void Update()
    {
        
    }
}
