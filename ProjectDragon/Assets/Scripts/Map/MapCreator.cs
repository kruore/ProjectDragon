using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room
{
    public Vector2 gridPos;

    public RoomType type;

    public bool doorTop, doorBot, doorLeft, doorRight;

    public int depth;

    public room(Vector2 _gridPos, RoomType _type, int _depth)
    {
        gridPos = _gridPos;
        type = _type;
        depth = _depth;
    }
}

public class MapCreator : MonoBehaviour
{
    public Player playerSet;
    public GameObject[,] map_Data;
    public GameObject map_Base;
    public GameObject[] map_Prefabs;
    public int map_Prefabs_Count = 1;

    public int gridSizeX, gridSizeY, gridSizeX_Cen, gridSizeY_Cen, numberOfRooms = 15;
    public Vector2 stair_LocalPosition = new Vector2(0.0f, 0.0f);

    public Vector2 worldSize = new Vector2(2.5f, 2.5f);

    public room[,] rooms;
    public List<Vector2> takenPositions = new List<Vector2>();

    private void Awake()
    {
        map_Base = Resources.Load("Map/Map_Base") as GameObject;
        ResourceLoadMap("Forest");

        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        gridSizeX_Cen = (int)worldSize.x;
        gridSizeY_Cen = (int)worldSize.y;
        gridSizeX = (int)(worldSize.x * 2);
        gridSizeY = (int)(worldSize.y * 2);
        map_Data = new GameObject[gridSizeX, gridSizeY];
    }

    void ResourceLoadMap(string _mapType)
    {
        map_Prefabs = new GameObject[map_Prefabs_Count];

        for (int i = 0; i < map_Prefabs_Count; i++)
        {
            string name = "Map/Map_" + _mapType;
            name += "_" + (i+1).ToString();
            map_Prefabs[i] = Resources.Load(name) as GameObject;
        }
    }

    void Start()
    {
        //map_Data[player_PosX, player_PosY] = GameObject.Instantiate(map_Base, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        playerSet = GameObject.Find("테스터").GetComponent<Player>();
        CreateRooms(); //lays out the actual map
        SetRoomDoors(); //assigns the doors where rooms would connect
        DrawMap(); //instantiates objects to make up a map

    }

    /// <summary>
    /// A와 B를 그리드 사이즈가 홀수냐 짝수냐에 따라 비교식을 바꿔 계산합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    bool CompareAandB(int a, int b)
    {
        bool temp = false;
        if (gridSizeX % 2 == 1)
        {
            temp = a > b;
        }
        else
        {
            temp = a >= b;
        }

        return temp;
    }

    void CreateRooms()
    {
        int depth = 0;
        //setup
        rooms = new room[gridSizeX, gridSizeY];

        //중앙 방 생성
        rooms[gridSizeX_Cen, gridSizeY_Cen] = new room(Vector2.zero, RoomType.Begin, depth);

        //중앙 방 위치 값 넣기
        takenPositions.Insert(0, Vector2.zero);

        Vector2 checkPos = Vector2.zero;

        //magic numbers
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

        //add rooms
        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            float randomPerc = ((float)i) / (((float)numberOfRooms - 1));

            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc); // 왜 한거지?

            //grab new position
            checkPos = NewPosition();

            //test new position - 옆 방이 여러 개 있으면 위치를 다시 뽑음
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare) // 이해 안감
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100); // 이해 안감
                if (iterations >= 50)
                    Debug.Log("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
            }
            //방이 얼마나 멀리 있는지 검사 합니다.
            depth = DepthCheck((int)checkPos.x + gridSizeX_Cen, (int)checkPos.y + gridSizeY_Cen);
            //finalize position
            rooms[(int)checkPos.x + gridSizeX_Cen, (int)checkPos.y + gridSizeY_Cen] = new room(checkPos, RoomType.Normal, depth);
            takenPositions.Insert(0, checkPos);
        }
    }

    int DepthCheck(int x, int y)
    {
        List<int> temp = new List<int>();

        if(x + 1 < gridSizeX)
        {
            if (rooms[x + 1, y] != null)
            {
                temp.Add(rooms[x + 1, y].depth);
            }
        }
        if(x - 1 >= 0)
        {
            if(rooms[x - 1, y] != null)
            {
                temp.Add(rooms[x - 1, y].depth);
            }
        }
        if (y + 1 < gridSizeY)
        {
            if (rooms[x, y + 1] != null)
            {
                temp.Add(rooms[x, y + 1].depth);
            }
        }
        if(y - 1 >= 0)
        {
            if(rooms[x, y - 1] != null)
            {
                temp.Add(rooms[x, y - 1].depth);
            }
        }

        //if (temp.Count >= 2)
        //{
        //    int count = temp.Count - 1;
        //    do
        //    {
        //        for (int j = 0; j < temp.Count - 1; j++)
        //        {
        //            if (temp[j] > temp[j + 1])
        //            {
        //                int a = temp[j];
        //                temp[j] = temp[j + 1];
        //                temp[j + 1] = a;
        //            }
        //        }
        //        count--;
        //    } while (count > 0);
        //}
        temp.Sort();
        return temp[0] + 1;
    }

    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;

        do
        {
            //현재 생성된 방중에 하나를 랜덤으로 뽑는 식
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
            x = (int)takenPositions[index].x;//capture its x, y position
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
            bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
            if (UpDown)
            { //find the position bnased on the above bools
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || CompareAandB(x, gridSizeX_Cen) || x < -gridSizeX_Cen || CompareAandB(y, gridSizeY_Cen) || y < -gridSizeY_Cen); //make sure the position is valid

        return checkingPos;
    }

    Vector2 SelectiveNewPosition()
    { // method differs from the above in the two commented ways
        int index = 0, inc = 0;

        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;

        do
        {
            inc = 0;
            do
            {
                //instead of getting a room to find an adject empty space, we start with one that only 
                //as one neighbor. This will make it more likely that it returns a room that branches out
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);

            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || CompareAandB(x, gridSizeX_Cen) || x < -gridSizeX_Cen || CompareAandB(y, gridSizeY_Cen) || y < -gridSizeY_Cen);

        if (inc >= 100)
        { // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
            Debug.Log("Error: could not find position with only one neighbor");
        }
        return checkingPos;
    }

    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0; // start at zero, add 1 for each side there is already a room
        if (usedPositions.Contains(checkingPos + Vector2.right))
        { //using Vector.[direction] as short hands, for simplicity
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.left))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.up))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.down))
        {
            ret++;
        }
        return ret;
    }

    void DrawMap()
    {
        //전체 맵 최상위 생성
        GameObject Map_Root = new GameObject("Map_Root", typeof(RoomManager));
        Map_Root.tag = "RoomManager";
        RoomManager Manager = Map_Root.GetComponent<RoomManager>();
        Map_Root.transform.SetPositionAndRotation(new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        Manager.gridSizeX_Cen = gridSizeX_Cen;
        Manager.gridSizeY_Cen = gridSizeY_Cen;

        //모든 방 설정
        foreach (room room in rooms)
        {
            if (room == null)
            {
                continue; //skip where there is no room
            }
            Vector2 drawPos = room.gridPos;
            drawPos.x *= 25f;//aspect ratio of map sprite
            drawPos.y *= 15f;
            int x = (int)room.gridPos.x;
            int y = (int)room.gridPos.y;

            if (room.type.Equals(RoomType.Begin)) map_Data[x + gridSizeX_Cen, y + gridSizeY_Cen] = Instantiate(map_Base, drawPos, Quaternion.identity, Map_Root.transform);
            else
            {
                int rand = Random.Range(0, map_Prefabs_Count - 1);
                map_Data[x + gridSizeX_Cen, y + gridSizeY_Cen] = Instantiate(map_Prefabs[rand], drawPos, Quaternion.identity, Map_Root.transform);
            }
            map_Data[x + gridSizeX_Cen, y + gridSizeY_Cen].AddComponent<Room>();

            GameObject map = map_Data[x + gridSizeX_Cen, y + gridSizeY_Cen];

            Room roomManager = map.GetComponent<Room>(); //Room
            //방마다 데이터 저장
            bool[] door_dir  = { room.doorBot, room.doorLeft, room.doorRight, room.doorTop};
            roomManager.SetData(room.gridPos, room.type, Manager, room.depth, door_dir); //Room Data Set

            GameObject doorWall = map.transform.Find("DoorWall").gameObject;
            GameObject North = doorWall.transform.Find("North").gameObject;
            GameObject South = doorWall.transform.Find("South").gameObject;
            GameObject West = doorWall.transform.Find("West").gameObject;
            GameObject East = doorWall.transform.Find("East").gameObject;

            //문 없는거 삭제
            int i = 0;
            //북
            if (!room.doorTop) Destroy(North);
            else
            {
                roomManager.door_All[i] = North;
                i++;
                North.transform.Find("Door").gameObject.AddComponent<Door>();
                North.transform.Find("Door").gameObject.GetComponent<Door>().Name = DoorName.North;
                North.SetActive(false);
            }
            //남
            if (!room.doorBot) Destroy(South);
            else
            {
                roomManager.door_All[i] = South;
                i++;
                South.transform.Find("Door").gameObject.AddComponent<Door>();
                South.transform.Find("Door").gameObject.GetComponent<Door>().Name = DoorName.South;
                South.SetActive(false);
            }
            //서
            if (!room.doorLeft) Destroy(West);
            else
            {
                roomManager.door_All[i] = West;
                i++;
                West.transform.Find("Door").gameObject.AddComponent<Door>();
                West.transform.Find("Door").gameObject.GetComponent<Door>().Name = DoorName.West;
                West.SetActive(false);
            }
            //동
            if (!room.doorRight) Destroy(East);
            else
            {
                roomManager.door_All[i] = East;
                i++;
                East.transform.Find("Door").gameObject.AddComponent<Door>();
                East.transform.Find("Door").gameObject.GetComponent<Door>().Name = DoorName.East;
                East.SetActive(false);
            }
        }
        SetStairRoom(); //setting the stair room
        SetNpcRoom(Map_Root.transform);
        Manager.Map_Data = map_Data;
        Manager.SetPlayerPos(0, 0);
        playerSet.EnemyRoom = Manager;
}
    void SetRoomDoors()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (rooms[x, y] == null)
                {
                    continue;
                }

                Vector2 gridPosition = new Vector2(x, y);

                if (y - 1 < 0)
                { //check above
                    rooms[x, y].doorBot = false;
                }
                else
                {
                    rooms[x, y].doorBot = (rooms[x, y - 1] != null);
                }

                if (y + 1 >= gridSizeY)
                { //check bellow
                    rooms[x, y].doorTop = false;
                }
                else
                {
                    rooms[x, y].doorTop = (rooms[x, y + 1] != null);
                }

                if (x - 1 < 0)
                { //check left
                    rooms[x, y].doorLeft = false;
                }
                else
                {
                    rooms[x, y].doorLeft = (rooms[x - 1, y] != null);
                }

                if (x + 1 >= gridSizeX)
                { //check right
                    rooms[x, y].doorRight = false;
                }
                else
                {
                    rooms[x, y].doorRight = (rooms[x + 1, y] != null);
                }
            }
        }

    }

    //계단방 세팅
    void SetStairRoom()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach(GameObject obj in map_Data)
        {
            if (obj == null) continue;

            temp.Add(obj);
        }

        if (temp.Count >= 2)
        {
            int count = temp.Count - 1;
            do
            {
                for (int j = 0; j < temp.Count - 1; j++)
                {
                    if (temp[j].GetComponent<Room>().depth > temp[j + 1].GetComponent<Room>().depth)
                    {
                        GameObject a = temp[j];
                        temp[j] = temp[j + 1];
                        temp[j + 1] = a;
                    }
                }
                count--;
            } while (count > 0);
        }

        temp[temp.Count - 1].GetComponent<Room>().roomType = RoomType.Stair;

        GameObject stair = Resources.Load("Object/Stair") as GameObject;
        GameObject stairTemp = GameObject.Instantiate(stair, temp[temp.Count - 1].transform.position, Quaternion.identity, temp[temp.Count - 1].transform);
        stairTemp.name = "Stair";
        stairTemp.transform.localPosition = new Vector3(stair_LocalPosition.x, stair_LocalPosition.y, 0.0f);
    }

    //NPC방 생성
    void SetNpcRoom(Transform _parent)
    {
        SetNpc_Market(_parent);
        
        //다른 npc방 추가
    }

    void SetNpc_Market(Transform _parent)
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject obj in map_Data)
        {
            if (obj == null) continue;
            if (obj.GetComponent<Room>().depth <= 2) continue;
            if (!obj.GetComponent<Room>().roomType.Equals(RoomType.Normal)) continue;

            temp.Add(obj);
        }

        int rand = Random.Range(0, temp.Count - 1);

        GameObject map_Market = Resources.Load("Map/Map_Market") as GameObject;
        GameObject map_MarketTemp = GameObject.Instantiate(map_Market, temp[rand].transform.position, Quaternion.identity, _parent);
        map_MarketTemp.AddComponent<Room>();

        Room map = map_MarketTemp.GetComponent<Room>();
        map.SetData(temp[rand].GetComponent<Room>());
        map.roomType = RoomType.NPC;
        
        //door setting
        GameObject doorWall = map_MarketTemp.transform.Find("DoorWall").gameObject;
        GameObject North = doorWall.transform.Find("North").gameObject;
        GameObject South = doorWall.transform.Find("South").gameObject;
        GameObject West = doorWall.transform.Find("West").gameObject;
        GameObject East = doorWall.transform.Find("East").gameObject;

        int i = 0;
        if (!map.doorTop) Destroy(North);
        else
        {
            map.door_All[i] = North;
            i++;
            North.transform.Find("Door").gameObject.AddComponent<Door>();
            North.transform.Find("Door").gameObject.GetComponent<Door>().Name = DoorName.North;
            North.SetActive(false);
        }
        if (!map.doorBot) Destroy(South);
        else
        {
            map.door_All[i] = South;
            i++;
            South.transform.Find("Door").gameObject.AddComponent<Door>();
            South.transform.Find("Door").gameObject.GetComponent<Door>().Name = DoorName.South;
            South.SetActive(false);
        }
        if (!map.doorLeft) Destroy(West);
        else
        {
            map.door_All[i] = West;
            i++;
            West.transform.Find("Door").gameObject.AddComponent<Door>();
            West.transform.Find("Door").gameObject.GetComponent<Door>().Name = DoorName.West;
            West.SetActive(false);
        }
        if (!map.doorRight) Destroy(East);
        else
        {
            map.door_All[i] = East;
            i++;
            East.transform.Find("Door").gameObject.AddComponent<Door>();
            East.transform.Find("Door").gameObject.GetComponent<Door>().Name = DoorName.East;
            East.SetActive(false);
        }

        //map_Data change
        int x = (int)map.gridPos.x + gridSizeX_Cen;
        int y = (int)map.gridPos.y + gridSizeY_Cen;
        Destroy(map_Data[x, y].gameObject);
        map_Data[x, y] = map_MarketTemp;
    }
}
