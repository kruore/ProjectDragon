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

    t_Node[,] grid;
    public List<t_Node> finalPath = new List<t_Node>();

    float nodeDiameter;
    int gridSizeX, gridSizeY;


    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new t_Node[gridSizeX, gridSizeY];
        Vector3 BottonLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for(int y=0;y<gridSizeY; y++)
        {
            for(int x=0;x<gridSizeX;x++)
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

    public List<t_Node> GetNeighboringNodes(t_Node _Node)
    {
        List<t_Node> NeighboringNodes = new List<t_Node>();
        int xCheck, yCheck;

        //Right Side
        xCheck = _Node.gridX + 1;
        yCheck = _Node.gridY;
        if(xCheck>=0&&xCheck < gridSizeX)
        {
            if(yCheck>=0&&yCheck<gridSizeY)
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
        yCheck = _Node.gridY+1;
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

        return NeighboringNodes;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        if (grid != null && displayGridGizmos)
        {
            foreach (t_Node n in grid)
            {
                Gizmos.color = (n.IsWall) ? Color.white : Color.red;
   
                if(finalPath!=null)
                {
                    if(finalPath.Contains(n))
                    {
                        Gizmos.color = Color.blue;
                    }
                }

                Gizmos.DrawCube(n.Pos, Vector3.one * (nodeDiameter - .1f));

            }
        }
    }

}
