using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
