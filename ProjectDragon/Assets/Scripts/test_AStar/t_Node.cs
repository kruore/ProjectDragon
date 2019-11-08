using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_Node
{

    public int gridX;
    public int gridY;

    public bool IsWall;
    public Vector3 Pos;

    public t_Node Parent;

    public int gCost;
    public int hCost;

    public int FCost { get { return gCost + hCost; } }

    public t_Node(bool _IsWall,Vector3 _pos,int _gridX,int _gridY)
    {
        IsWall = _IsWall;
        Pos = _pos;
        gridX = _gridX;
        gridY = _gridY;
    }

}
