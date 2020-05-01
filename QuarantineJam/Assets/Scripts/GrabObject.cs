using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

public class GrabObject : MonoBehaviour 
{
    private bool _isOk = false;
    public bool Grab = true;
    public GameObject Object;
    public Transform Teleport;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isOk)
        {
            bool Grab = false;
        }

        if (!Grab)
        {
            Object.transform.position = Teleport.position;
            //Grab
        }
        else
        {

            bool _isOk = false;
            //Lacher
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.name == "GrabItem")
        {

            bool _isOk = true;
        }


    }
}
