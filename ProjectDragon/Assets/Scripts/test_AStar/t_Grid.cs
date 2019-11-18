using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distance;

    public int nodeOverlapCountX;       //오브젝트와 노드가 겹치는 노드갯수 X축
    public int nodeOverlapCountY;       //오브젝트와 노드가 겹치는 노드갯수 Y축

    t_Node[,] grid;
    //public List<t_Node> finalPath = new List<t_Node>();

    float nodeDiameter;
    int gridSizeX, gridSizeY;


    GameObject[] Enemies;
    List<GameObject> EnemiesArray;
    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

        Enemies = GameObject.FindGameObjectsWithTag("temp");

    }

    // Start is called before the first frame update
    void Start()
    {
        if (Enemies.Length > 1)
        {
            for (int i = 0; i < Enemies.Length; ++i)
            {
                //EnemiesArray.Add(Enemies[i]);
                //Debug.Log(EnemiesArray[i].name);
            }
        }
    }

    void CreateGrid()
    {
        grid = new t_Node[gridSizeX, gridSizeY];
        Vector3 BottonLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = BottonLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool wall = true;
                if (Physics2D.OverlapBox(worldPoint, new Vector2(nodeRadius, nodeRadius), 0.0f, wallMask))
                {
                    wall = false;
                }

                grid[x, y] = new t_Node(wall, worldPoint, x, y);
            }
        }

    }


    public t_Node NodeFromWorldPosition(Vector3 _worldPosition)
    {
        float xPoint = ((_worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = ((_worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];
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

        Debug.Log(nodeOverlapCountX);
        Debug.Log(nodeOverlapCountY);
    }
    public List<t_Node> GetOverlapNodes(t_Node _Node)
    {
        List<t_Node> NeighboringNodes = new List<t_Node>();
        int xCheck, yCheck;

        //Right Side
        xCheck = _Node.gridX + 1;
        yCheck = _Node.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);

            }
        }

        //Left Side
        xCheck = _Node.gridX - 1;
        yCheck = _Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);

            }
        }

        //Top Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);

            }
        }

        //Bottom Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);

            }
        }

        //Right TopSide
        xCheck = _Node.gridX + 1;
        yCheck = _Node.gridY + 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);

            }
        }

        //Left TopSide
        xCheck = _Node.gridX - 1;
        yCheck = _Node.gridY + 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);

            }
        }

        //Right BottomSide
        xCheck = _Node.gridX + 1;
        yCheck = _Node.gridY - 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);

            }
        }

        //Left BottomSide
        xCheck = _Node.gridX - 1;
        yCheck = _Node.gridY - 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);

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
                NeighboringNodes.Add(grid[xCheck, yCheck]);
                Debug.Log("xCheck" + xCheck);
            }
        }

        //Left Side
        xCheck = _Node.gridX - nodeOverlapCountX - 1;
        yCheck = _Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);

            }
        }

        //Top Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY + nodeOverlapCountY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);

            }
        }

        //Bottom Side
        xCheck = _Node.gridX;
        yCheck = _Node.gridY - nodeOverlapCountY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);

            }
        }

        return NeighboringNodes;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        //#if UNITY_EDITOR

        if (grid != null && displayGridGizmos)
        {

            foreach (t_Node n in grid)
            {
                Gizmos.color = (n.IsWall) ? Color.white : Color.red;
                for (int i = 0; i < Enemies.Length; ++i)
                {
                    if (Enemies[i].transform.GetComponent<Tracking>().pathFinding.finalPath != null)
                    {
                        if (Enemies[i].transform.GetComponent<Tracking>().pathFinding.finalPath.Contains(n))
                        {
                            Gizmos.color = Color.blue;
                        }
                    }
                }
                Gizmos.DrawCube(n.Pos, Vector3.one * (nodeDiameter - .1f));

            }

        }
        //#endif
    }

}
