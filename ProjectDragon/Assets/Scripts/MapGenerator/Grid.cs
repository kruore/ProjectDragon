using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDimeter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDimeter = nodeRadius;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDimeter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDimeter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    //Grid 생성
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottonLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;


        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottonLeft + Vector3.right * (x * nodeDimeter + nodeRadius) + Vector3.up * (y * nodeDimeter + nodeRadius);
                Vector3 worldPoint2D = worldBottonLeft + Vector3.right * (x * nodeDimeter + nodeRadius) + Vector3.up * (y * nodeDimeter + nodeRadius);
                //walkable fasle이 되는 패널을 제외하고 움직임
                //bool walkable = !(Physics2D.OverlapCircle(worldPoint2D, nodeRadius, unwalkableMask));
                //bool walkable = !(Physics2D.OverlapCircle(worldPoint2D, new Vector2(nodeRadius,nodeRadius),0,unwalkableMask));
                bool walkable = !(Physics2D.OverlapBox(worldPoint2D, new Vector2(nodeRadius-0.1f, nodeRadius-0.1f), unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        //근처 노드값을 탐색
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x ,gridWorldSize.y,1));
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDimeter - .1f));
                
            }
        }
    }
}
