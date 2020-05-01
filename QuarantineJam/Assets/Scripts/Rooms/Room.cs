using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Obsolete("Give to player")]
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameController.instance.player.SetCurrentRoom(this);
        }else if (other.tag == "Devil")
        {
            GameController.instance.devil.SetCurrentRoom(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
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
