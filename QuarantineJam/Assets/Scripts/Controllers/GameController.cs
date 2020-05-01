using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public PlayerController player;

    void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }
}
