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
    }

    [System.Obsolete("need to return up stair")]
    public Stairs GetNearUpStair()
    {
        return null;
    }
    [System.Obsolete("need to return down stair")]
    public Stairs GetNearDownStair()
    {
        return null;
    }

    public Room GetRandomRoom(DevilController devil)
    {
        Room[] roomsAvailable = rooms.Where(room => room != devil.GetCurrentRoom()).ToArray();

        int rand = UnityEngine.Random.Range(0, roomsAvailable.Length);
        Debug.Log("Devil go to "+ roomsAvailable[rand].name);
        return roomsAvailable[rand];
    }
}
