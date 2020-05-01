using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speedMove = 4;
    private float _baseSpeedMove;
    public float deadZoneVertical = 0.5f;

    private Stairs _stairs;
    private bool _usedStair = false;

    private bool _isOk = false;
    private bool Grab = true;
    public GameObject Object;
    public Transform objectPos;

    void Awake()
    {
        _baseSpeedMove = speedMove;
    }

    void Start()
    {
        GameController.instance.player = this;
    }

    void Update()
    {
        Move();
        Interact();
        DisplaceVertical();
    }

    private void Move()
    {
        float Horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speedMove;

        if (Horizontal == 0)
            return;

        transform.Translate(Horizontal, 0, 0);
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.parent = objectPos;
        }
        else
        {
            transform.parent = null;
        }

        if (!Input.GetButtonDown("Interact"))
        {

            return;
        }
           
    }

    private void DisplaceVertical()
    {
        float Vertical = Input.GetAxis("Vertical");
        if (Vertical > -deadZoneVertical && Vertical < deadZoneVertical)
        {
            _usedStair = false;
            return;
        }
        if (!_stairs)
            return;
        if (Vertical > 0 && !_stairs.upStair)
            return;
        if (Vertical < 0 && !_stairs.downStair)
            return;
        if (_usedStair)
            return;

        _usedStair = true;
        //ToDo displace on stairs
        if (Vertical > 0)
        {
            transform.position = _stairs.upStair.transform.position;
        }
        else
        {
            transform.position = _stairs.downStair.transform.position;
        }
    }

    public void SetStair(Stairs stairs)
    {
        _stairs = stairs;
    }

    void OnTriggerStay2D(Collider2D collider2D)
    {
        

        if (collider2D.gameObject.tag == "GrabItem")
        {

          
            GameController.instance.player.GrabObject(this);
        }


    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "GrabItem")
        {

            _isOk = false;
        }


    }
}
