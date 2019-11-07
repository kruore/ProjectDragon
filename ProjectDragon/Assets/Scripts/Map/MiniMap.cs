using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public RoomManager RoomManager;
    private GameObject[] room;
    private Transform roomRoot;
    public UIPanel panel;

    private void Awake()
    {
        room = GameObject.FindGameObjectsWithTag("MiniMapRoom");
        roomRoot = transform.Find("RoomRoot");
        panel = GetComponentInParent<UIPanel>();
    }
    
    public void UpdateMiniMap()
    {
        int x = RoomManager.player_PosX;
        int y = RoomManager.player_PosY;
        roomRoot.localPosition = new Vector3(-x * 32.0f, -y * 32.0f, 0.0f);

        //이동한 방이 클리어되지 않았다면 미니맵을 끈다.
        if(!RoomManager.Map_Data[x + RoomManager.gridSizeX_Cen, y + RoomManager.gridSizeY_Cen].GetComponent<Room>().roomState.Equals(RoomState.Clear))
        {
            gameObject.SetActive(false);
        }
    }

    public void InitMiniMap()
    {
        List<GameObject> temp = new List<GameObject>();
        GameObject[,] map_Data = RoomManager.Map_Data;
        GameObject portalImage = Resources.Load("Map/MiniMap/portal_Image") as GameObject;

        foreach (GameObject obj in map_Data)
        {
            if (obj == null) continue;

            temp.Add(obj);
        }

        for(int i = 0; i < temp.Count; i++)
        {
            //포탈 이미지 로드
            //방의 정보
            Room temp_room = temp[i].GetComponent<Room>();
            float x = temp_room.gridPos.x;
            float y = temp_room.gridPos.y;
            //미니맵 위치 세팅
            room[i].transform.localPosition = new Vector3(x * 32.0f, y * 32.0f, 0.0f);
            //방이 일반 방이 아니면 포탈 이미지 세팅
            if(!temp_room.roomType.Equals(RoomType.Normal))
            {
                room[i].AddComponent<BoxCollider2D>();
                room[i].GetComponent<BoxCollider2D>().enabled = false;
                //포탈 그림 붙이고, 안보이게 하기
                GameObject portal = Instantiate(portalImage, room[i].transform.localPosition, Quaternion.identity, room[i].transform);
            }
            temp_room.MiniMapPos = room[i];

            //처음 시작시 시작방을 제외하고 미니맵에서 지웁니다.
            if (x != 0.0f || y != 0.0f)
            {
                room[i].SetActive(false);
            }
        }
    }

    public void Maximalize()
    {
        panel.baseClipRegion = new Vector4(0.0f, 0.0f, 900.0f, 900.0f);
    }

    public void Minimalize()
    {

    }

}
