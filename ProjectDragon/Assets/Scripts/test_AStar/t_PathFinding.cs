﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_PathFinding : MonoBehaviour
{
    public t_Grid grid;

    public List<t_Node> finalPath = new List<t_Node>();
    private void Awake()
    {
    }

    private void Start()
    {
        //FindPath(startPos.position, targetPos.position);
        
    }
    private void Update()
    {
        
    }

    void Create()
    {
        GameObject AStar = GameObject.Find("AStar");
        grid = AStar.transform.GetComponent<t_Grid>();
    }

    public void FindPath(Vector3 _startPos,Vector3 _targetPos)
    {
        Create();

        t_Node startNode = grid.NodeFromWorldPosition(_startPos);
        t_Node targetNode = grid.NodeFromWorldPosition(_targetPos);


        List<t_Node> OpenList = new List<t_Node>();
        HashSet<t_Node> ClosedList = new HashSet<t_Node>();

        OpenList.Add(startNode);

        while(OpenList.Count>0)
        {
            t_Node currentNode = OpenList[0];
            for(int i=1;i<OpenList.Count;i++)
            {
                if(OpenList[i].FCost<currentNode.FCost|| OpenList[i].FCost== currentNode.FCost&&OpenList[i].hCost<currentNode.hCost)
                {
                    currentNode = OpenList[i];
                }
            }

            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
            }

            foreach (t_Node NeighborNode in grid.GetNeighboringNodes(currentNode))
            {
                if (!NeighborNode.IsWall || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, NeighborNode);

                //moveCost is Neighbors of the current node
                //NeighborNode are In an OpenList 
                //moveCost's F value < NeighborNode's F value 
                if (moveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = moveCost;
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, targetNode);
                    NeighborNode.Parent = currentNode;

                    if(!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }


    }

    void GetFinalPath(t_Node _startingNode,t_Node _endNode)
    {
        List<t_Node> FinalPath = new List<t_Node>();
        t_Node currentNode = _endNode;

        while(currentNode!=_startingNode)
        {
            FinalPath.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        FinalPath.Reverse();
        finalPath = FinalPath;
    }

    int GetManhattenDistance(t_Node _nodeA, t_Node _nodeB)
    {
        int ix = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int iy= Mathf.Abs(_nodeA.gridY - _nodeB.gridY);

        return ix + iy;
    }
}
