using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    public float speedMove = 4;
    private float _baseSpeedMove;
    public float deadZoneVertical = 0.5f;

    private List<Stairs> _stairsList = new List<Stairs>();
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
        Stairs stairs = null;
        if (_stairsList.Count >0)
        {
            stairs = _stairsList[_stairsList.Count - 1];
        }

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
            transform.position = stairs.upStair.transform.position;
        }
        else
        {
            transform.position = stairs.downStair.transform.position;
        }
    }

    public void SetStair(Stairs stairs)
    {
        _stairsList.Add(stairs);
    }
    public void ExitStair(Stairs stairs)
    {
        _stairsList.Remove(stairs);
    }
    #endregion
}
