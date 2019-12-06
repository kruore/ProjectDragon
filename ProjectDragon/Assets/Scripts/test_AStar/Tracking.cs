﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    [HideInInspector]
    public t_PathFinding pathFinding;

    t_Node[] findPathNode;
    [HideInInspector]
    public Vector3 currentWaypoint;
    int pathNextIndex;

    //Vector3 moveDirection;

    //임시
    [HideInInspector]
    public Transform targetPos;


    private void Awake()
    {
        pathFinding = new t_PathFinding();

        //임시
        targetPos = GameObject.FindGameObjectWithTag("Player").transform;

    }

    //find path
    public void FindPathManager(Rigidbody2D _rb2d, float _moveSpeed)
    {
        pathFinding.FindPath(transform.position, targetPos.position);
        if (pathFinding.grid != null && pathFinding.finalPath.Count > 0)
        {
            findPathNode = pathFinding.finalPath.ToArray();
            StartCoroutine(Move(_rb2d, _moveSpeed));
        }
    }
    bool isArriveStartNode = false;
    IEnumerator Move(Rigidbody2D _rb2d, float _moveSpeed)
    {

        currentWaypoint = findPathNode[0].Pos;

        if (Vector3.Distance(transform.position, currentWaypoint) == 0.0f)  //오차범위 0.1
        {
            pathNextIndex++;
            if (pathNextIndex >= findPathNode.Length)
            {
                yield break;
            }
            currentWaypoint = findPathNode[pathNextIndex].Pos;
        }

        if (transform.position == pathFinding.startNode.Pos)
        {
            isArriveStartNode = true;
        }
     

        if (!isArriveStartNode) //FianlPath로 이동전에 노드위치로 먼저이동
        {
            transform.position = Vector3.MoveTowards(transform.position, pathFinding.startNode.Pos, 10.0f * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, _moveSpeed * Time.deltaTime);
            
            //Velocity Move
            // _rb2d.velocity = Vector3.zero;
            //Vector3 moveDirection = (currentWaypoint - pathFinding.startNode.Pos).normalized;
            //_rb2d.velocity = moveDirection * _moveSpeed * 10.0f * Time.deltaTime;
        }

        yield return null;
    }

    

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.CompareTag("Object"))
    //    {
    //        RescanPath(collision);
    //    }
    //}
    //IsObject = false; 를 언제 할것인가가 문제로다!
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Object"))
    //    {
    //        pathFinding.grid.NodeFromWorldPosition(collision.transform.position).IsObject = false;
    //    }
    //}

}
