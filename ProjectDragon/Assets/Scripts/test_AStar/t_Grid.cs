/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-16////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_Grid : MonoBehaviour
{
    public bool displayGridGizmos = false;
    public LayerMask wallMask;
    public LayerMask objectMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distance;

  

    public t_Node[,] gridNode;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }


    private void Awake()
    {
        //wallMask = LayerMask.GetMask("Wall");
        objectMask = LayerMask.GetMask("Object");
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
    }

    private void Start()
    {
        CreateGrid();
    }


    public void CreateGrid()
    {
        gridNode = new t_Node[gridSizeX, gridSizeY];
        //Vector3 pivot = new Vector3(0.0f, 0.3f, 0.0f);
        Vector3 BottonLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        //BottonLeft -= pivot;
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = BottonLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = true;

                if (Physics2D.OverlapBox(worldPoint, new Vector2(nodeDiameter, nodeDiameter), 0.0f, wallMask))
                {
                    walkable = false;
                }

                gridNode[x, y] = new t_Node(walkable, worldPoint, x, y);

                if (Physics2D.OverlapBox(worldPoint, new Vector2(nodeDiameter, nodeDiameter), 0.0f, objectMask))
                {
                    gridNode[x, y].IsObject = true;
                }
            }
        }
    }
    //Object 파괴시 FinalPath를 재탐색한다.
    public void RescanPath(Collider2D _collider)
    {
        //collider에 있는 노드를 읽어온다.
        NodeFromWorldPosition(_collider.transform.position).IsObject = false;

        //콜라이더 종류에 따른 오버랩된 노드 계산
        int nodeOverlapCountX = 0, nodeOverlapCountY =0;

        if (_collider is BoxCollider2D)
        {
            BoxCollider2D boxCol = _collider as BoxCollider2D;
            nodeOverlapCountX = CalcOverlapNodeCount(boxCol.size.x);
            nodeOverlapCountY = CalcOverlapNodeCount(boxCol.size.y);
        }

        else if (_collider is CapsuleCollider2D)
        {
            CapsuleCollider2D capsuleCol = _collider as CapsuleCollider2D;

            //GetOverlapNodeCount
            nodeOverlapCountX = CalcOverlapNodeCount(capsuleCol.size.x);
            nodeOverlapCountY = CalcOverlapNodeCount(capsuleCol.size.y);

        }

        else if (_collider is CircleCollider2D)
        {
            CircleCollider2D circleCol = _collider as CircleCollider2D;

            //GetOverlapNodeCount
            nodeOverlapCountY = nodeOverlapCountX = CalcOverlapNodeCount(circleCol.radius * 2);
        }


        //오버된 노드 읽어오기
        foreach (t_Node OverlapNode in GetOverlapNodes(NodeFromWorldPosition(_collider.transform.position), nodeOverlapCountX+1, nodeOverlapCountY+1))
        {
            if (OverlapNode.IsObject)
            {
                OverlapNode.IsObject = false;
                //objectNodes.Add(OverlapNode);
            }
        }
    }

    public t_Node NodeFromWorldPosition(Vector3 _worldPosition)
    {
        float xPoint = (((_worldPosition.x - transform.parent.position.x) + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = (((_worldPosition.y - transform.parent.position.y) + gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return gridNode[x, y];
    }

   

    //오브젝트와 노드가 겹치는 노드갯수 계산
    public int CalcOverlapNodeCount(float objBoxSize)
    {
        return Mathf.RoundToInt((objBoxSize - nodeRadius) / nodeDiameter);
    }


    //대각선에 있는 각도 찾기
    public List<t_Node> GetOverlapNodes(t_Node _Node, int nodeOverlapCountX,int nodeOverlapCountY)  
    {
        List<t_Node> OverlapNodes = new List<t_Node>();
        int xCheck, yCheck;
        
        //Right Side
        xCheck = _Node.gridX + nodeOverlapCountX;
        yCheck = _Node.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            OverlapNodes.Add(gridNode[xCheck, yCheck]);
        }

        //Left Side
        xCheck = _Node.gridX - nodeOverlapCountX;
        yCheck = _Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            OverlapNodes.Add(gridNode[xCheck, yCheck]);
        }

        //Top Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY + nodeOverlapCountY;
        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            OverlapNodes.Add(gridNode[xCheck, yCheck]);
        }

        //Bottom Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY - nodeOverlapCountY;
        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            OverlapNodes.Add(gridNode[xCheck, yCheck]);
        }

        //Right TopSide
        xCheck = _Node.gridX + nodeOverlapCountX;
        yCheck = _Node.gridY + nodeOverlapCountY;
        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            OverlapNodes.Add(gridNode[xCheck, yCheck]);
        }

        //Left TopSide
        xCheck = _Node.gridX - nodeOverlapCountX;
        yCheck = _Node.gridY + nodeOverlapCountY;
        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            OverlapNodes.Add(gridNode[xCheck, yCheck]);
        }

        //Right BottomSide
        xCheck = _Node.gridX + nodeOverlapCountX;
        yCheck = _Node.gridY - nodeOverlapCountY;
        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            OverlapNodes.Add(gridNode[xCheck, yCheck]);
        }

        //Left BottomSide
        xCheck = _Node.gridX - nodeOverlapCountX;
        yCheck = _Node.gridY - nodeOverlapCountY;
        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            OverlapNodes.Add(gridNode[xCheck, yCheck]);
        }

        return OverlapNodes;
    }

    //주변노드만 찾기
    public List<t_Node> GetNeighboringNodes(t_Node _Node, int nodeOverlapCountX, int nodeOverlapCountY)    
    {
        List<t_Node> NeighboringNodes = new List<t_Node>();
        int xCheck, yCheck;

        //Right Side
        xCheck = _Node.gridX + nodeOverlapCountX + 1;
        yCheck = _Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            NeighboringNodes.Add(gridNode[xCheck, yCheck]);
        }

        //Left Side
        xCheck = _Node.gridX - nodeOverlapCountX - 1;
        yCheck = _Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            NeighboringNodes.Add(gridNode[xCheck, yCheck]);
        }

        //Top Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY + nodeOverlapCountY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            NeighboringNodes.Add(gridNode[xCheck, yCheck]);
        }

        //Bottom Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY - nodeOverlapCountY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
        {
            NeighboringNodes.Add(gridNode[xCheck, yCheck]);
        }

        return NeighboringNodes;
    }
    
    #region NodeDraw
    void OnDrawGizmos()
    {
        RoomManager RoomManager = GameObject.FindWithTag("RoomManager").GetComponent<RoomManager>();
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

#if UNITY_EDITOR

        if (gridNode != null && displayGridGizmos)
        {

            foreach (t_Node n in gridNode)
            {
                Gizmos.color = (n.Walkable&& !n.IsObject) ? Color.white : Color.red;
                

                for (int i = 0; i < RoomManager.PlayerLocationRoomMonsterData().Count; ++i)
                {
                    if (RoomManager.PlayerLocationRoomMonsterData()[i].transform.GetComponent<Tracking>().pathFinding.finalPath != null)
                    {
                        if (RoomManager.PlayerLocationRoomMonsterData()[i].transform.GetComponent<Tracking>().pathFinding.finalPath.Contains(n))
                        {
                            Gizmos.color = Color.blue;
                        }
                    }
                }


                Gizmos.DrawCube(n.Pos, Vector3.one * (nodeDiameter - 0.005f));

            }

        }
#endif
    }
    #endregion
    
}
