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

//RoomManager 이름 수정해야함
public class Room : MonoBehaviour
{
    public Vector2 gridPos;

    public RoomType roomType;

    public bool doorTop, doorBot, doorLeft, doorRight;
    public GameObject portal;

    public GameObject[] door_All = new GameObject[4] { null, null, null, null};
    public List<GameObject> monsters = new List<GameObject>();

    public RoomState roomState = RoomState.DeActivate;

    public int enemyCount = 0;

    public RoomManager roomManager;
    //public BattleManager battleManager;

    public int depth;

    public Player playerSet;

    public GameObject MiniMapPos
    {
        get { return miniMapPos; }
        set
        {
            miniMapPos = value;
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
                    break;
            }
        }
    }
    private GameObject miniMapPos;

    private void Awake()
    {
        InitRoom();
        playerSet = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        //룸의 상태 관리
        CheckRoomState();
    }

    //방의 데이터를 초기화 - 방 생성시 바로 작동합니다.
    private void InitRoom()
    {
        //battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        Monster[] temp_monsters = transform.GetComponentsInChildren<Monster>();
        foreach (Monster obj in temp_monsters)
        {
            monsters.Add(obj.gameObject);
        }
        enemyCount = monsters.Count;
    }

    //룸의 상태를 확인
    void CheckRoomState()
    {
        if (!roomState.Equals(RoomState.Clear))
        {
            //몬스터 리스트 관리
            MonsterCounting();
            if (enemyCount == 0 && roomState.Equals(RoomState.Activate))
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

        //테스트 필요
        monsters.Clear();
        monsters.AddRange(temp_monsters);
        /////////////
        enemyCount = monsters.Count;
    }

    void CheckPlayerPos()
    {
        Vector2 PlayerPos = new Vector2(roomManager.player_PosX, roomManager.player_PosY);
        if (gridPos == PlayerPos)
        {
            roomState = RoomState.Activate;

            //battleManager.EnemyFinder();
            //StartCoroutine(battleManager.CalculateDistanceWithPlayer());

            playerSet.EnemyArray = monsters;
            StartCoroutine(playerSet.CalculateDistanceWithPlayer());

            foreach (GameObject obj in monsters)
            {
                StartCoroutine(obj.GetComponent<FSM_NormalEnemy>().Start_On());
            }

            
        }
    }

    //방이 클리어되면 작동하는 함수
    void IsClear()
    {
        roomState = RoomState.Clear;
        OpenAllDoor();
        roomManager.miniMap.gameObject.SetActive(true);
        MiniMapPos.GetComponent<UISprite>().alpha = 1.0f;
        //CollectAll_Items();

        //계단방의 경우 문이 열립니다.
        if (roomType.Equals(RoomType.Stair))
        {
            gameObject.transform.Find("Stair").GetComponent<Stair>().IsOpen = true;
        }
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
    }
}
