﻿using System.Collections;
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

    public void GiveObject(GrabObject grabObject)
    {
        _grabObjectDetection = grabObject;
    }

    #region Stair
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
    #endregion
}
