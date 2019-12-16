using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TILE_TYPE
    {
        RAND,
        WALL,
        TRAP,
        PLAYER
    }
    [SerializeField]
    private TILE_TYPE type { set { type = value; } get { return type; } }
    [SerializeField]
    public TILE_TYPE index { set { type = value; } get { return type; } }
    public float f { set; get; }
    public float g { set; get; }
    public float h { set; get; }
    public Tile nextTile { set; get; }
    private MeshRenderer randerer = null;

    private void Awake()
    {
        randerer = GetComponent<MeshRenderer>();
    }
}
