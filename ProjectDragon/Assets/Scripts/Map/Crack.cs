using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crack : MonoBehaviour
{
    public Room room;

    private Sprite sprite;

    private void Awake()
    {
        switch (GetComponentInParent<Door>().Name)
        {
            case DoorName.East:
                sprite = Resources.Load<Sprite>("Object/Door_East");
                break;
            case DoorName.North:
                sprite = Resources.Load<Sprite>("Object/Door_North");
                break;
            case DoorName.South:
                sprite = Resources.Load<Sprite>("Object/Door_South");
                break;
            case DoorName.West:
                sprite = Resources.Load<Sprite>("Object/Door_West");
                break;
        }

    }
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(room.roomState.Equals(RoomState.Clear) && collision.CompareTag("Skill"))
        {

        }
    }
}
