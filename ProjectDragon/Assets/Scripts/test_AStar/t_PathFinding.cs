using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_PathFinding : MonoBehaviour
{
    [HideInInspector]
    public t_Grid grid;
    [HideInInspector]
    public t_Node startNode;
    [HideInInspector]
    public List<t_Node> finalPath = new List<t_Node>();
    //public Vector3[] findPathNode;


    public void Create(float objBoxSizeX, float objBoxSizeY, t_Grid parentGrid)
    {
        //GameObject AStar = GameObject.Find("AStar");
        grid = parentGrid.transform.GetComponent<t_Grid>();

        grid.GetOverlapNodeCount(objBoxSizeX, objBoxSizeY);

    }

    public void FindPath(Vector3 _startPos,Vector3 _targetPos)
    {

        startNode = grid.NodeFromWorldPosition(_startPos);
        t_Node targetNode = grid.NodeFromWorldPosition(_targetPos);

        t_Node currentNode;

        Heap<t_Node> OpenList = new Heap<t_Node>(grid.MaxSize);
        HashSet<t_Node> ClosedList = new HashSet<t_Node>();

        OpenList.Add(startNode);

        while(OpenList.Count>0)
        {
            currentNode = OpenList.RemoveFirst();
            ClosedList.Add(currentNode);
            if(OpenList ==null)
            {
                 break;
            }
            //currentNode = OpenList[0];
            //for(int i=1;i<OpenList.Count;i++)
            //{
            //    if(OpenList[i].FCost<currentNode.FCost|| OpenList[i].FCost== currentNode.FCost&&OpenList[i].hCost<currentNode.hCost)
            //    {
            //        currentNode = OpenList[i];
            //    }
            //}

            //OpenList.Remove(currentNode);
            //ClosedList.Add(currentNode);

            //도착했는가?
            if (grid.nodeOverlapCountX > 0 || grid.nodeOverlapCountY > 0)    //Grid의 하나 노드보다 큰 경우
            {
                foreach (t_Node OverlapNode in grid.GetOverlapNodes(currentNode))
                {
                    if (OverlapNode == targetNode|| currentNode==targetNode)
                    {
                        GetFinalPath(startNode, currentNode);
                        break;
                    }
                }
            }
            else
            {
                if (currentNode == targetNode)
                { 
                    GetFinalPath(startNode, targetNode);
                    break;
                }
            }


            foreach (t_Node NeighborNode in grid.GetNeighboringNodes(currentNode))
            {
                if (!NeighborNode.IsWall || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, NeighborNode);

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
        //findPathNode = SimplifyPath(FinalPath);

    }
    //Vector3[] SimplifyPath(List<t_Node> path)
    //{
    //    List<Vector3> waypoints = new List<Vector3>();
    //    Vector2 directionOld = Vector2.zero;

    //    for (int i = 1; i < path.Count; i++)
    //    {
    //        Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
    //        if (directionNew != directionOld)
    //        {
    //            waypoints.Add(path[i].Pos);
    //        }
    //        directionOld = directionNew;
    //    }
    //    return waypoints.ToArray();
    //}


    int GetManhattenDistance(t_Node _nodeA, t_Node _nodeB)
    {
        int ix = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int iy= Mathf.Abs(_nodeA.gridY - _nodeB.gridY);

        return ix + iy;
    }
}
