﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public int level;

    public Transform upStair;
    public Transform downStair;

    void Awake()
    {
        if (!upStair.gameObject.activeSelf)
        {
            upStair = null;
        }
        if (!downStair.gameObject.activeSelf)
        {
            downStair = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameController.instance.player.SetStair(this);
        }
        else if (other.tag == "Devil")
        {
            GameController.instance.devil.SetStair(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameController.instance.player.ExitStair(this);
        }
        else if (other.tag == "Devil")
        {
            GameController.instance.devil.SetStair(this);
        }
    }
}
