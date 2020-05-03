using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDoor : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameController.instance.CheckWin();
        }
    }
}
