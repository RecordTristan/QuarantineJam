using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public int level;

    public Transform upStair;
    public Transform downStair;

    void Start()
    {
        if (!upStair.gameObject.activeSelf)
        {
            upStair = null;
        }
        else
        {
            upStair.position = new Vector3(upStair.position.x, 
                LDController.instance.levelValue[level + 1], 
                upStair.position.z);
        }
        if (!downStair.gameObject.activeSelf)
        {
            downStair = null;
        }
        else
        {
            downStair.position = new Vector3(downStair.position.x,
                LDController.instance.levelValue[level - 1],
                downStair.position.z);
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
