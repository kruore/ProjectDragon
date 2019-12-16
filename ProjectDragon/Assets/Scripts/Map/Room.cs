
// ==============================================================
// Room Object
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-16
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Begin,
    Normal,
    Stair,
    NPC,
    Hidden
}

public enum RoomState
{
    DeActivate,
    Activate,
    Clear
}

public class Room : MonoBehaviour
{
    public Vector2 gridPos;     //방의 위치를 나타냅니다.
    public RoomType roomType;   //어떤 방인지를 나타냅니다.
    public RoomState roomState = RoomState.DeActivate; //현재 방의 상태
    public int depth; //방이 시작방에서 얼마나 먼 곳에 있는지

    public bool doorTop, doorBot, doorLeft, doorRight; //문이 해당 방향에 있는지 없는지 나타냅니다.
    public GameObject[] door_All = new GameObject[4] { null, null, null, null}; //문 오브젝트

    public GameObject portal; //포탈 오브젝트

    public List<GameObject> monsters = new List<GameObject>(); //방의 몬스터를 관리하기 위한 리스트

    public RoomManager roomManager; 

    public Player playerSet; 

    public GameObject MiniMapPos //미니맵 상에서의 방 오브젝트
    {
        get { return miniMapPos; }
        set
        {
            miniMapPos = value;
        }
    }
    private GameObject miniMapPos;

    private void Awake()
    {
        //데이터 초기화
        InitRoom();
    }

    void Update()
    {
        //룸의 상태 관리
        CheckRoomState();
    }

    //방의 데이터를 초기화 - 방 생성시 바로 작동합니다.
    private void InitRoom()
    {
        playerSet = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Monster[] temp_monsters = transform.GetComponentsInChildren<Monster>();
        foreach (Monster obj in temp_monsters)
        {
            monsters.Add(obj.gameObject);
        }
    }

    //룸의 상태를 확인
    void CheckRoomState()
    {
        if (!roomState.Equals(RoomState.Clear))
        {
            //몬스터 리스트 관리
            MonsterCounting();
            if (monsters.Count == 0 && roomState.Equals(RoomState.Activate))
            {
                //몬스터가 한 마리도 없다면 클리어입니다.
                IsClear();
            }
            else if (!roomState.Equals(RoomState.Activate)) CheckPlayerPos();
        }
    }

    //몬스터 리스트 관리
    void MonsterCounting()
    {
         List<GameObject> temp_monsters = new List<GameObject>();
        foreach (GameObject obj in monsters)
        {
            if (obj.GetComponent<Monster>().isDead) continue;
            else
            {
                temp_monsters.Add(obj);
            }
        }

        //리스트 값 재지정
        monsters.Clear();
        monsters.AddRange(temp_monsters);

        //몬스터가 없으면 플레이어에게 Null을 세팅
        if(monsters.Count==0)
        {
            playerSet.TempNullSet();
        }
    }

    //플레이어가 현재 방에 있는게 맞다면 배틀 시작
    void CheckPlayerPos()
    {
        Vector2 PlayerPos = new Vector2(roomManager.player_PosX, roomManager.player_PosY);
        if (gridPos == PlayerPos)
        {
            roomState = RoomState.Activate;

            //플레이어 배틀 시작
            playerSet.EnemyArray = monsters;
            StartCoroutine(playerSet.CalculateDistanceWithPlayer());

            //몬스터 배틀 시작
            foreach (GameObject obj in monsters)
            {
                StartCoroutine(obj.GetComponent<Enemy>().Start_On());
            }
        }
    }

    //방이 클리어되면 작동하는 함수
    void IsClear()
    {
        roomState = RoomState.Clear;
        OpenAllDoor(); //모든 문 열기

        roomManager.miniMap.gameObject.SetActive(true); //미니맵 켜기
        miniMapPos.GetComponent<UISprite>().alpha = 1.0f; //

        #region 수정 필요 -- 이미지 로드, 포탈 표시 방식
        //포탈이 있는 방이라면 미니맵 상에서 포탈 표시
        if (!roomType.Equals(RoomType.Normal))
        {
            miniMapPos.transform.Find("Portal").GetComponent<UISprite>().enabled = true;
        }

        //방이 normal 이 아니라면 미니맵에서 특별한 색으로 표시
        switch (roomType)
        {
            case RoomType.Begin:
                miniMapPos.GetComponent<UISprite>().color = Color.cyan;
                break;
            case RoomType.NPC:
                miniMapPos.GetComponent<UISprite>().color = Color.yellow;
                break;
            case RoomType.Stair:
                miniMapPos.GetComponent<UISprite>().color = Color.green;
                gameObject.transform.Find("Stair").GetComponent<Stair>().IsOpen = true;
                break;
            default:
                break;
        }
        #endregion

        //포탈 켜기
        roomManager.PortalOn();

        //아이템 획득 - 미구현 상태, 드랍 구현 필요
        //CollectAll_Items();

    }

    /// <summary>
    /// [구현중] 방에 존재하는 모든 아이템을 인벤토리에 넣습니다.
    /// 아이템 드랍이 어떻게 될 것인지 먼저 정해야한다.
    /// </summary>
    void CollectAll_Items()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("item");
        #region 아이템 전부 먹기
        //구현 필요
        #endregion
    }

    /// <summary>
    /// 방이 클리어되면 문이 열립니다.
    /// </summary>
    void OpenAllDoor()
    {
        GameObject wall = transform.Find("wall").gameObject;
        GameObject North = wall.transform.Find("North").gameObject;
        GameObject South = wall.transform.Find("South").gameObject;
        GameObject West = wall.transform.Find("West").gameObject;
        GameObject East = wall.transform.Find("East").gameObject;

        foreach (GameObject obj in door_All)
        {
            if (obj == null) continue;

            switch (obj.name)
            {
                case "North":
                    Destroy(North);
                    break;
                case "South":
                    Destroy(South);
                    break;
                case "West":
                    Destroy(West);
                    break;
                case "East":
                    Destroy(East);
                    break;
            }

            obj.SetActive(true);
        }
    }

 
    public void SetData(Vector2 _gridPos, RoomType _roomType, RoomManager _roomManager, int _depth, bool[] _door)
    {
        gridPos = _gridPos;
        roomType = _roomType;
        roomManager = _roomManager;
        depth = _depth;
        doorBot = _door[0];
        doorLeft = _door[1];
        doorRight = _door[2];
        doorTop = _door[3];
        if (!_roomType.Equals(RoomType.Begin))
        {
            gameObject.SetActive(false);
        }

        if (!roomType.Equals(RoomType.Normal))
        {
            portal = transform.GetComponentInChildren<Portal>().gameObject;
            portal.SetActive(false);
        }
        else portal = null;
    }

    public void SetData(Room _room)
    {
        gridPos = _room.gridPos;
        roomType = _room.roomType;
        roomManager = _room.roomManager;
        depth = _room.depth;
        doorBot = _room.doorBot;
        doorLeft = _room.doorLeft;
        doorRight = _room.doorRight;
        doorTop = _room.doorTop;
        if (!roomType.Equals(RoomType.Begin))
        {
            gameObject.SetActive(false);
        }

        if (!roomType.Equals(RoomType.Normal))
        {
            portal = transform.GetComponentInChildren<Portal>().gameObject;
            portal.SetActive(false);
        }
        else portal = null;
    }
}
