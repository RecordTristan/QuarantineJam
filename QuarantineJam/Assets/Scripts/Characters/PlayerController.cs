using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    private float _baseSpeedMove;
    public float deadZoneVertical = 0.5f;

    private bool _usedStair = false;

    //Grab
    [Header("Levitation")]
    public float limitLevitation = 2;
    public float speedLevitation = 2;
    public Transform objectGrabPos;
    private Vector3 _basePoseGrab;
    private GrabObject _grabObjectDetection;
    private GrabObject _currentGrab;

    [Header("DeplacementInStairs")]
    protected float currentDeplacement = 0;
    protected int currentMovement = 0;
    private Stairs _startVertical;

    protected override void Awake()
    {
        base.Awake();
        _baseSpeedMove = speedMove;
        _basePoseGrab = objectGrabPos.localPosition;
        canMove = true;
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
        MoveObjectPosition();
    }

    private void Move()
    {
        float Horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speedMove;

        if (Horizontal == 0)
        {
            anim.SetBool("Walk", false);
            return;
        }
        if (!canMove)
            return;

        anim.SetBool("Walk", true);
        if (Horizontal > 0)
        {
            float scale = Mathf.Abs(display.transform.localScale.x);
            display.transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            float scale = Mathf.Abs(display.transform.localScale.x);
            display.transform.localScale = new Vector3(-scale, scale, scale);
        }

        transform.Translate(Horizontal, 0, 0);
    }

    private void Interact()
    {
        _currentGrab?.Take(objectGrabPos);

        if (!Input.GetButtonDown("Interact"))
            return;

        anim.SetTrigger("TakeObject");

        if (_currentGrab)
        {
            _grabObjectDetection?.UseObject(_currentGrab.gameObject);
            _currentGrab.Put();
            _currentGrab = null;
        }
        else
        {
            _currentGrab = _grabObjectDetection;
        }
    }

    public void Loose()
    {
        anim.SetTrigger("Loose");
        canMove = false;
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
    public void MoveObjectPosition()
    {
        objectGrabPos.transform.localPosition = new Vector3(objectGrabPos.transform.localPosition.x, _basePoseGrab.y + Mathf.Sin(Time.time * speedLevitation)* limitLevitation, _basePoseGrab.z);
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
        if (_usedStair)
            return;

        anim.SetBool("Walk", true);

        //ToDo displace on stairs
        if (Vertical > 0)
        {
            if (currentDeplacement >= 1)
            {
                currentLevel++;
                display.transform.localPosition = Vector3.zero;
                transform.position = stairs.upStair.currentPlacement.transform.position;
                _usedStair = true;
                _startVertical = null;
                currentMovement = 0;
            }
            else
            {
                if (!_startVertical)
                {
                    _startVertical = stairs;
                }
                currentDeplacement = (float)currentMovement / (_startVertical.displacementPosition.Length+1);
                Vector3 positionToGo;
                if (currentMovement >= _startVertical.displacementPosition.Length)
                {
                    positionToGo = _startVertical.upStair.currentPlacement.transform.position;
                }
                else
                {
                    positionToGo = _startVertical.displacementPosition[currentMovement].transform.position;
                }

                if (display.transform.position.x < positionToGo.x)
                {
                    float scale = Mathf.Abs(display.transform.localScale.x);
                    display.transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    float scale = Mathf.Abs(display.transform.localScale.x);
                    display.transform.localScale = new Vector3(-scale, scale, scale);
                }

                display.transform.position = Vector3.MoveTowards(
                        display.transform.position,
                        positionToGo,
                        speedMove * Time.deltaTime * (currentMovement == 0 ? 10f : 0.5f));
                if (display.transform.position == positionToGo)
                {
                    currentMovement++;
                }
            }
        }
        else
        {
            
            if (currentDeplacement <= 0)
            {
                currentLevel--;
                display.transform.localPosition = Vector3.zero;
                transform.position = _startVertical.currentPlacement.transform.position;
                _usedStair = true;
                currentMovement = 0;
                _startVertical = null;
            }
            else
            {
                if (!_startVertical)
                {
                    _startVertical = stairs.downStair;
                    currentMovement = _startVertical.displacementPosition.Length;
                }
                currentDeplacement = (float)currentMovement / (_startVertical.displacementPosition.Length + 1);
                Vector3 positionToGo;
                if (currentMovement <= 0)
                {
                    positionToGo = _startVertical.currentPlacement.transform.position;
                }
                else
                {
                    positionToGo = _startVertical.displacementPosition[currentMovement-1].transform.position;
                }

                if (display.transform.position.x < positionToGo.x)
                {
                    float scale = Mathf.Abs(display.transform.localScale.x);
                    display.transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    float scale = Mathf.Abs(display.transform.localScale.x);
                    display.transform.localScale = new Vector3(-scale, scale, scale);
                }

                display.transform.position = Vector3.MoveTowards(
                        display.transform.position,
                        positionToGo,
                        speedMove * Time.deltaTime * (currentMovement == 0 ? 10f : 0.5f));
                if (display.transform.position == positionToGo)
                {
                    currentMovement--;
                }
            }
        }
    }

    
    #endregion
}
