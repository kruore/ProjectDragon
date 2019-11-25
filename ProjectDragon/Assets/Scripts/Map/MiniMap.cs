using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public RoomManager RoomManager;
    private GameObject[] room;
    private Transform roomRoot;
    public UIPanel panel;

    public UIButton button;
    public bool isMax = false;
    public EventDelegate mini = new EventDelegate();
    public EventDelegate maxi = new EventDelegate();

    private void Awake()
    {
        room = GameObject.FindGameObjectsWithTag("MiniMapRoom");
        roomRoot = transform.Find("RoomRoot");
        panel = GetComponentInParent<UIPanel>();

        button = transform.Find("Button").GetComponent<UIButton>();
    }

    private void Start()
    {
        mini = new EventDelegate(GetComponent<MiniMap>(), "Minimalize");
        maxi = new EventDelegate(GetComponent<MiniMap>(), "Maximalize");

        button.onClick.Add(maxi);
    }

    public void UpdateMiniMap()
    {
        int x = RoomManager.player_PosX;
        int y = RoomManager.player_PosY;
        roomRoot.localPosition = new Vector3(-x * 32.0f, -y * 32.0f, 0.0f);

        //이동한 방이 클리어되지 않았다면 미니맵을 끈다.
        if (!RoomManager.Map_Data[x + RoomManager.gridSizeX_Cen, y + RoomManager.gridSizeY_Cen].GetComponent<Room>().roomState.Equals(RoomState.Clear))
        {
            gameObject.SetActive(false);
        }

        foreach(Room obj in RoomManager.PlayerLocationAroundRoomInMap())
        {
            if (obj == null) continue;

            //클리어가 안된 방들 미니맵에서 연하게 표시
            if(!obj.roomState.Equals(RoomState.Clear))
            {
                obj.MiniMapPos.SetActive(true);
                obj.MiniMapPos.GetComponent<UISprite>().alpha = 0.5f;
            }
        }
    }

    public void InitMiniMap()
    {
        List<GameObject> temp = new List<GameObject>();
        GameObject[,] map_Data = RoomManager.Map_Data;
        GameObject portalImage = Resources.Load("Map/MiniMap/portal_Image") as GameObject;

        int i = 0;
        foreach (GameObject obj in map_Data)
        {
            if (obj == null) continue;

            //포탈 이미지 로드
            //방의 정보
            Room temp_room = obj.GetComponent<Room>();
            float x = temp_room.gridPos.x;
            float y = temp_room.gridPos.y;

            //미니맵 위치 세팅
            room[i].transform.localPosition = new Vector3(x * 32.0f, y * 32.0f, 0.0f);
            //방이 일반 방이 아니면 포탈 이미지 세팅
            if (!temp_room.roomType.Equals(RoomType.Normal))
            {
                room[i].AddComponent<BoxCollider2D>();
                room[i].GetComponent<BoxCollider2D>().enabled = false;
                //포탈 그림 붙이고, 안보이게 하기
                GameObject portal = Instantiate(portalImage, room[i].transform.localPosition, Quaternion.identity, room[i].transform);
                portal.transform.localPosition = Vector3.zero;
            }

            temp_room.MiniMapPos = room[i];

            //처음 시작시 시작방을 제외하고 미니맵에서 지웁니다.
            if (x != 0.0f || y != 0.0f)
            {
                room[i].SetActive(false);
            }

            i++;
        }

        //안쓰는 미니맵 오브젝트들 전부 끄기
        for(int j = i; j < room.Length; j++)
        {
            room[j].SetActive(false);
        }
    }

    public void Maximalize()
    {
        //panel의 위치와 크기 조절
        panel.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        panel.baseClipRegion = new Vector4(0.0f, 0.0f, 600.0f, 600.0f);

        GetComponent<UISprite>().SetDimensions(600, 600);
        button.GetComponent<UISprite>().SetDimensions(600, 600);
        transform.Find("PlayerPos").GetComponent<UISprite>().SetDimensions(60, 60);
        roomRoot.localScale = new Vector3(2.0f, 2.0f, 1.0f);

        button.onClick.RemoveAt(0);
        StartCoroutine(AddButton(mini));
        //button.onClick.Add(mini);
    }

    public void Minimalize()
    {

        panel.transform.localPosition = new Vector3(-700.0f, 350.0f, 0.0f);
        panel.baseClipRegion = new Vector4(0.0f, 0.0f, 150.0f, 150.0f);
        GetComponent<UISprite>().SetDimensions(150, 150);
        button.GetComponent<UISprite>().SetDimensions(150, 150);
        transform.Find("PlayerPos").GetComponent<UISprite>().SetDimensions(30, 30);
        roomRoot.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        button.onClick.RemoveAt(0);
        StartCoroutine(AddButton(maxi));
        //EventDelegate.Remove(button.onClick, mini);
        //EventDelegate.Add(button.onClick, maxi);
    }

    IEnumerator AddButton(EventDelegate _Event)
    {
        yield return new WaitForSeconds(0.3f);
        button.onClick.Add(_Event);
    }
}
