using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HomeController : MonoBehaviour
{
    public static HomeController instance;

    [Header("House")]
    public Room[] rooms;
    public Stairs[] stairsHouse;

    void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;

        rooms = FindObjectsOfType<Room>();
        stairsHouse = FindObjectsOfType<Stairs>();
    }


    public Stairs GetNearUpStair(int level, GameObject refObject)
    {
        Stairs[] stairs = stairsHouse.Where(st => st.level == level).ToArray();
        float dist = 1000;
        int index = 0;
        for (int i = stairs.Length; i-- > 0;)
        {
            if (stairs[i].upStair)
            {
                if (Vector3.Distance(stairs[i].transform.position, refObject.transform.position) < dist)
                {
                    dist = Vector3.Distance(stairs[i].transform.position, refObject.transform.position);
                    index = i;
                }
            }
        }
        return stairs[index];
    }

    public Stairs GetNearDownStair(int level, GameObject refObject)
    {
        Stairs[] stairs = stairsHouse.Where(st => st.level == level).ToArray();
        float dist = 1000;
        int index = 0;
        for (int i = stairs.Length; i-- > 0;)
        {
            if (stairs[i].downStair)
            {
                if (Vector3.Distance(stairs[i].transform.position, refObject.transform.position) < dist)
                {
                    dist = Vector3.Distance(stairs[i].transform.position, refObject.transform.position);
                    index = i;
                }
            }
        }
        return stairs[index];
    }

    public Room GetNearRoom(Room room)
    {
        Room[] roomsAvailable = rooms.Where(r => !(r as Stairs) && r.level == room.level).ToArray();
        float dist = 1000;
        int index = 0;
        for (int i = roomsAvailable.Length; i-- > 0;)
        {
            if (Vector3.Distance(roomsAvailable[i].transform.position, room.transform.position) < dist)
            {
                dist = Vector3.Distance(roomsAvailable[i].transform.position, room.transform.position);
                index = i;
            }
        }
        return roomsAvailable[index];
    }

    public Room GetRandomRoom(DevilController devil)
    {
        Room[] roomsAvailable = rooms.Where(room => room != devil.GetCurrentRoom() && !(room as Stairs)).ToArray();

        int rand = UnityEngine.Random.Range(0, roomsAvailable.Length);
        Debug.Log("Devil go to "+ roomsAvailable[rand].name);
        return roomsAvailable[rand];
    }
}
