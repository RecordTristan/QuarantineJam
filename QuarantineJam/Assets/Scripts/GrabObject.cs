using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

public class GrabObject : MonoBehaviour 
{
    public string nameGrabObject;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameController.instance.player.GiveObject(this);
        }
    }

    public void Take(Transform targetPos)
    {
        transform.position = targetPos.position;
    }

    public void Put()
    {

    }
}
