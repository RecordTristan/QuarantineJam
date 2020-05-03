using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDesigner : MonoBehaviour
{
    public static GameDesigner instance;

    public List<int> difficulty = new List<int>();

    void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }
}
