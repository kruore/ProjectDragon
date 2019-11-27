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

    public int nodeOverlapCountX;       //오브젝트와 노드가 겹치는 노드갯수 X축
    public int nodeOverlapCountY;       //오브젝트와 노드가 겹치는 노드갯수 Y축

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
        Vector3 BottonLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

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

    //오브젝트와 노드가 겹치는 노드갯수 구하기
    int CalcOverlapNodeCount(float objBoxSize)
    {
        return Mathf.RoundToInt((objBoxSize - nodeRadius) / nodeDiameter);
    }

    public void GetOverlapNodeCount(float objBoxSizeX, float objBoxSizeY)
    {
        nodeOverlapCountX = CalcOverlapNodeCount(objBoxSizeX);
        nodeOverlapCountY = CalcOverlapNodeCount(objBoxSizeY);
    }

    public List<t_Node> GetOverlapNodes(t_Node _Node)
    {
        List<t_Node> NeighboringNodes = new List<t_Node>();
        int xCheck, yCheck;

        //Right Side
        xCheck = _Node.gridX + nodeOverlapCountX;
        yCheck = _Node.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);

            }
        }

        //Left Side
        xCheck = _Node.gridX - nodeOverlapCountX;
        yCheck = _Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);

            }
        }

        //Top Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY + nodeOverlapCountY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);

            }
        }

        //Bottom Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY - nodeOverlapCountY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);

            }
        }

        //Right TopSide
        xCheck = _Node.gridX + nodeOverlapCountX;
        yCheck = _Node.gridY + nodeOverlapCountY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);

            }
        }

        //Left TopSide
        xCheck = _Node.gridX - nodeOverlapCountX;
        yCheck = _Node.gridY + nodeOverlapCountY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);

            }
        }

        //Right BottomSide
        xCheck = _Node.gridX + nodeOverlapCountX;
        yCheck = _Node.gridY - nodeOverlapCountY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);

            }
        }

        //Left BottomSide
        xCheck = _Node.gridX - nodeOverlapCountX;
        yCheck = _Node.gridY - nodeOverlapCountY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);

            }
        }

        return NeighboringNodes;
    }

    public List<t_Node> GetNeighboringNodes(t_Node _Node)
    {
        List<t_Node> NeighboringNodes = new List<t_Node>();
        int xCheck, yCheck;

        //Right Side
        xCheck = _Node.gridX + nodeOverlapCountX + 1;
        yCheck = _Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);
            }
        }

        //Left Side
        xCheck = _Node.gridX - nodeOverlapCountX - 1;
        yCheck = _Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);

            }
        }

        //Top Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY + nodeOverlapCountY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);

            }
        }

        //Bottom Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY - nodeOverlapCountY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(gridNode[xCheck, yCheck]);

            }
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
