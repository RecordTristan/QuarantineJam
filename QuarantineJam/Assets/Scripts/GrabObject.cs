using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

public class GrabObject : MonoBehaviour 
{
    public bool Grab;
    public GameObject Object;
    public Transform Teleport;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!Grab)
            {
                Object.transform.position = Teleport.position;
                //Grab
            }
            else
            { 

                //Lacher
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.name == "GrabItem")
        {

            bool Grab = false;
        }


    }
}
