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
            //map_Data = new GameObject[gridSizeX, gridSizeY];
            map_Data = value;
            miniMap.InitMiniMap();
        }
    }

    private GameObject[,] map_Data;

    public int player_PosX, player_PosY;
    public int gridSizeX_Cen, gridSizeY_Cen;
    public int gridSizeX, gridSizeY;
    public int playerGridPosX, playerGridPosY;

    public MiniMap miniMap; 

    private void Awake()
    {
        player_PosX = 0;
        player_PosY = 0;
        playerGridPosX = 0;
        playerGridPosY = 0;
        miniMap = GameObject.FindGameObjectWithTag("MiniMap").GetComponent<MiniMap>();
        miniMap.RoomManager = GetComponent<RoomManager>();
    }

    public void SetPlayerPos(int _player_PosX,int _player_PosY)
    {
        player_PosX = _player_PosX;
        player_PosY = _player_PosY;
        playerGridPosX = gridSizeX_Cen + player_PosX;
        playerGridPosY = gridSizeY_Cen + player_PosY;
        miniMap.UpdateMiniMap();
    }

    public void SetGridData(int _gridSizeX_Cen, int _gridSizeY_Cen, int _gridSizeX, int _gridSizeY)
    {
        gridSizeX_Cen = _gridSizeX_Cen;
        gridSizeY_Cen = _gridSizeY_Cen;
        gridSizeX = _gridSizeX;
        gridSizeY = _gridSizeY;
        playerGridPosX = _gridSizeX_Cen;
        playerGridPosY = _gridSizeY_Cen;
    }

    public Room[] PlayerLocationAroundRoomInMap()
    {
        Room[] temp = new Room[4];
        Room room = Map_Data[playerGridPosX, playerGridPosY].GetComponent<Room>();

        temp[0] = room.doorBot ? Map_Data[playerGridPosX, playerGridPosY - 1].GetComponent<Room>() : null;
        temp[1] = room.doorRight ? Map_Data[playerGridPosX + 1, playerGridPosY].GetComponent<Room>() : null;
        temp[2] = room.doorTop ? Map_Data[playerGridPosX, playerGridPosY + 1].GetComponent<Room>() : null;
        temp[3] = room.doorLeft ? Map_Data[playerGridPosX - 1, playerGridPosY].GetComponent<Room>() : null;

        return temp;
    }

    public Room PlayerLocationInMap()
    {
        return Map_Data[playerGridPosX, playerGridPosY].GetComponent<Room>();
    }

    public List<GameObject> PlayerLocationRoomMonsterData()
    {
        return Map_Data[playerGridPosX, playerGridPosY].GetComponent<Room>().monsters;
    }

    public bool PlayerIsNPCRoom()
    {
        return Map_Data[playerGridPosX, playerGridPosY].GetComponent<Room>().roomType == RoomType.NPC ? true : false;
    }

    public bool PlayerIsClearRoom()
    {
        return Map_Data[playerGridPosX, playerGridPosY].GetComponent<Room>().roomState == RoomState.Clear ? true : false;
    }

    public int PortalRoomClearCount()
    {
        int count = 0;

        foreach(GameObject obj in map_Data)
        {
            if (obj == null) continue;

            Room temp = obj.GetComponent<Room>();
            if (!temp.roomState.Equals(RoomState.Clear) || temp.roomType.Equals(RoomType.Normal)) continue;

            count++;
        }

        return count;
    }

    public void PortalOn()
    {
#if UNITY_EDITOR
        Debug.Log(PortalRoomClearCount());
        Debug.Log("portal on");
#endif
        if(PortalRoomClearCount() >= 2)
        {
            foreach(GameObject obj in map_Data)
            {
                if (obj == null) continue;

                Room temp = obj.GetComponent<Room>();
                if (temp.roomType.Equals(RoomType.Normal)) continue;
                if (temp.roomState.Equals(RoomState.Clear)) temp.portal.GetComponent<Portal>().IsPortalOn = true;
            }
        }
    }

    public void MiniMapMaximalize()
    {
        if(!miniMap.button.onClick[0].methodName.Equals("Minimalize")) miniMap.Maximalize();
        miniMap.button.GetComponent<BoxCollider>().enabled = false;
    }

    public void MiniMapMinimalize()
    {
        if(!miniMap.button.onClick[0].methodName.Equals("Maximalize")) miniMap.Minimalize();
        miniMap.button.GetComponent<BoxCollider>().enabled = true;
    }
}
