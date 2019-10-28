using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//RoomManager 이름 수정해야함
public class RoomManager : MonoBehaviour
{
    public GameObject[,] Map_Data
    {
        get { return map_Data; }
        set
        {
            map_Data = value;
            miniMap.InitMiniMap();
        }
    }

    private GameObject[,] map_Data;

    public int player_PosX, player_PosY;
    public int gridSizeX_Cen, gridSizeY_Cen;

    public MiniMap miniMap;

    private void Awake()
    {
        player_PosX = 0;
        player_PosY = 0;
        miniMap = GameObject.FindGameObjectWithTag("MiniMap").GetComponent<MiniMap>();
        miniMap.RoomManager = GetComponent<RoomManager>();
    }

    public void SetPlayerPos(int _player_PosX,int _player_PosY)
    {
        player_PosX = _player_PosX;
        player_PosY = _player_PosY;
        miniMap.UpdateMiniMap();
    }

    public bool PlayerIsNPCRoom()
    {
        return Map_Data[player_PosX, player_PosY].GetComponent<Room>().roomType == RoomType.NPC ? true : false;
    }

    public bool PlayerIsClearRoom()
    {
        return Map_Data[player_PosX, player_PosY].GetComponent<Room>().roomState == RoomState.Clear ? true : false;
    }
}
