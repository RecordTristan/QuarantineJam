using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int level;
    

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameController.instance.player.SetCurrentRoom(this);
        }else if (other.tag == "Devil")
        {
            GameController.instance.devil.SetCurrentRoom(this);
        }
        GameController.instance.CheckDefeat();
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameController.instance.player.RemoveRoom(this);
        }
        else if (other.tag == "Devil")
        {
            GameController.instance.devil.RemoveRoom(this);
        }
    }
}
