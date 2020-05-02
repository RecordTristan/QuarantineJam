using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    private float _baseSpeedMove;
    public float deadZoneVertical = 0.5f;

    private bool _usedStair = false;

    private bool _isOk = false;
    private bool Grab = true;
    public GameObject Object;

    //Grab
    public Transform objectGrabPos;
    private GrabObject _grabObjectDetection;
    private GrabObject _currentGrab;

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
        _currentGrab?.Take(objectGrabPos);

        if (!Input.GetButtonDown("Interact"))
            return;

        if (_currentGrab)
        {
            _currentGrab.Put();
            _currentGrab = null;
        }
        else
        {
            _currentGrab = _grabObjectDetection;
        }

    }

    #region Object
    public void GiveObject(GrabObject grabObject)
    {
        _grabObjectDetection = grabObject;
    }
    public bool GrabOjbect()
    {
        return _currentGrab ? true : false;
    }
    #endregion

    #region Stair
    private void DisplaceVertical()
    {
        float Vertical = Input.GetAxis("Vertical");
        Stairs stairs = GetStairs();

        if (Vertical > -deadZoneVertical && Vertical < deadZoneVertical)
        {
            _usedStair = false;
            return;
        }
        if (!stairs)
            return;
        if (Vertical > 0 && !stairs.upStair)
            return;
        if (Vertical < 0 && !stairs.downStair)
            return;
        if (_usedStair)
            return;

        _usedStair = true;
        //ToDo displace on stairs
        if (Vertical > 0)
        {
            currentLevel++;
            transform.position = stairs.upStair.transform.position;
        }
        else
        {
            currentLevel--;
            transform.position = stairs.downStair.transform.position;

        }
    }

    
    #endregion
}
