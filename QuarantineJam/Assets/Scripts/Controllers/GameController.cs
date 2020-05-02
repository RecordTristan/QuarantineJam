﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public PlayerController player;
    public DevilController devil;

    void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }

    public void CheckDefeat()
    {
        if (player.GrabOjbect() && player.GetCurrentRoom() == devil.GetCurrentRoom())
        {
            Debug.Log("Loose");
        }
    }
}
