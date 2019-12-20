/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-16////////////////////
//////////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_PathFinding : MonoBehaviour
{
    [HideInInspector] public t_Grid grid;
    [HideInInspector] public t_Node startNode;
    [HideInInspector] public List<t_Node> finalPath = new List<t_Node>();


    int nodeOverlapCountX, nodeOverlapCountY;       //오브젝트와 노드가 겹치는 노드갯수



    public void Create(Collider2D _collider , t_Grid _AStar)
    {
        grid = _AStar;

        //콜라이더 종류에 따라 겹치는 노드갯수 구하기
        if (_collider is BoxCollider2D)
        {
            BoxCollider2D boxCol = _collider as BoxCollider2D;

            //GetOverlapNodeCount
            nodeOverlapCountX = grid.CalcOverlapNodeCount(boxCol.size.x);
            nodeOverlapCountY = grid.CalcOverlapNodeCount(boxCol.size.y);
        }

        else if (_collider is CapsuleCollider2D)
        {
            CapsuleCollider2D capsuleCol = _collider as CapsuleCollider2D;

            //GetOverlapNodeCount
            nodeOverlapCountX = grid.CalcOverlapNodeCount(capsuleCol.size.x);
            nodeOverlapCountY = grid.CalcOverlapNodeCount(capsuleCol.size.y);

        }

        else if (_collider is CircleCollider2D)
        {
            CircleCollider2D circleCol = _collider as CircleCollider2D;

            //GetOverlapNodeCount
            nodeOverlapCountY = nodeOverlapCountX = grid.CalcOverlapNodeCount(circleCol.radius*2);
        }
    }

    //public void Create(float _objBoxSizeX, float _objBoxSizeY, t_Grid _AStar)
    //{
    //    //grid = GameObject.Find("AStar").transform.GetComponent<t_Grid>();
    //    grid = _AStar;

    //    //GetOverlapNodeCount
    //    nodeOverlapCountX = grid.CalcOverlapNodeCount(_objBoxSizeX);
    //    nodeOverlapCountY = grid.CalcOverlapNodeCount(_objBoxSizeY);
    //    //grid.GetOverlapNodeCount(objBoxSizeX, objBoxSizeY);
    //}

    t_Node targetNode, currentNode;
    public void FindPath(Vector3 _startPos,Vector3 _targetPos)
    {

        startNode = grid.NodeFromWorldPosition(_startPos);
        targetNode = grid.NodeFromWorldPosition(_targetPos);

        Heap<t_Node> OpenList = new Heap<t_Node>(grid.MaxSize);
        HashSet<t_Node> ClosedList = new HashSet<t_Node>();

        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            currentNode = OpenList.RemoveFirst();
            ClosedList.Add(currentNode);
            if (OpenList == null)
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
            if (nodeOverlapCountX > 0 || nodeOverlapCountY > 0)    //노드크기가 하나 노드보다 큰 경우
            {
                foreach (t_Node OverlapNode in grid.GetOverlapNodes(currentNode, nodeOverlapCountX, nodeOverlapCountY))
                {
                    if (OverlapNode == targetNode || currentNode == targetNode)
                    {
                        GetFinalPath(startNode, currentNode);
                        break;
                    }
                }
            }
            else                                                   //노드크기가 하나 노드인 경우
            {
                if (currentNode == targetNode)
                {
                    GetFinalPath(startNode, targetNode);
                    break;
                }
            }
            

            foreach (t_Node NeighborNode in grid.GetNeighboringNodes(currentNode, nodeOverlapCountX, nodeOverlapCountY))
            {
                //타겟노드가 갈 수 없는 노드로 되어있다면
                if (GetNotWalkNode(NeighborNode) == targetNode)  //갈수없는 곳인 이웃노드와 갈수없는 곳인 타겟노드가 같다면
                {
                    NeighborNode.gCost = currentNode.gCost + GetManhattenDistance(currentNode, NeighborNode);
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, targetNode);
                    NeighborNode.Parent = currentNode;
                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                    break;
                }


                if (!NeighborNode.Walkable || NeighborNode.IsObject || ClosedList.Contains(NeighborNode))
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
    t_Node GetNotWalkNode(t_Node NeiNode)
    {
        //이웃노드와 타겟노드 둘다 갈수없다면
        if (!NeiNode.Walkable && !targetNode.Walkable)
        {
            return NeiNode;
        }
        return null;
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
