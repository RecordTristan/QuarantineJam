using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CharacterController
{
    private float _baseSpeedMove;
    public float deadZoneVertical = 0.5f;

    private bool _usedStair = false;
    private BoxCollider2D _collider;

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
        _collider = GetComponent<BoxCollider2D>();
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("TristanScene");
        }
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
            _grabObjectDetection?.UseObject(_currentGrab);
            _currentGrab.Put(currentLevel);
            _currentGrab = null;
            speedMove = _baseSpeedMove;
        }
        else
        {
            _currentGrab = _grabObjectDetection;
            speedMove = _baseSpeedMove / _currentGrab.weight;
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
        if (!stairs && !_startVertical)
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
                currentMovement = 0;
                transform.position = _startVertical.upStair.currentPlacement.transform.position;
                _usedStair = true;
                _startVertical = null;
                currentDeplacement = 0.5f;
                _collider.enabled = true;
                canMove = true;
                Physics2D.IgnoreLayerCollision(9, 10, false);

            }
            else
            {
                Physics2D.IgnoreLayerCollision(9, 10,true);
                canMove = false;
                MoveUpStair(stairs);
            }
        }
        else
        {
            
            if (currentDeplacement <= 0)
            {
                currentLevel--;
                currentMovement = 0;
                transform.position = _startVertical.currentPlacement.transform.position;
                _startVertical = null;
                _usedStair = true;
                currentDeplacement = 0.5f;
                _collider.enabled = true;
                canMove = true;
                Physics2D.IgnoreLayerCollision(9, 10, false);
            }
            else
            {
                Physics2D.IgnoreLayerCollision(9, 10, true);
                canMove = false;
                MoveDownStair(stairs);
            }
        }
    }
    private void MoveUpStair(Stairs stairs)
    {
        if (!_startVertical)
        {
            _startVertical = stairs;
            //_collider.enabled = !_startVertical.needToGhost;
            CameraManager2D.instance.StairMovement(currentLevel+1);
        }
        currentDeplacement = (float)currentMovement / (_startVertical.displacementPosition.Length + 1);
        Vector3 positionToGo;
        if (currentMovement >= _startVertical.displacementPosition.Length)
        {
            positionToGo = _startVertical.upStair.currentPlacement.transform.position;
        }
        else
        {
            positionToGo = _startVertical.displacementPosition[currentMovement].transform.position;
        }

        if (transform.position.x < positionToGo.x)
        {
            float scale = Mathf.Abs(display.transform.localScale.x);
            display.transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            float scale = Mathf.Abs(display.transform.localScale.x);
            display.transform.localScale = new Vector3(-scale, scale, scale);
        }
        transform.position = Vector3.MoveTowards(
                transform.position,
                positionToGo,
                speedMove * Time.deltaTime * (0.5f));

        if (transform.position == positionToGo)
        {
            currentMovement++;
        }
    }
    private void MoveDownStair(Stairs stairs)
    {
        if (!_startVertical)
        {
            _startVertical = stairs.downStair;
            currentMovement = _startVertical.displacementPosition.Length;
            //_collider.enabled = !_startVertical.needToGhost;

            CameraManager2D.instance.StairMovement(currentLevel - 1);
        }
        currentDeplacement = (float)currentMovement / (_startVertical.displacementPosition.Length + 1);
        Vector3 positionToGo;
        if (currentMovement <= 0)
        {
            positionToGo = _startVertical.currentPlacement.transform.position;
        }
        else
        {
            positionToGo = _startVertical.displacementPosition[currentMovement - 1].transform.position;
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

        transform.position = Vector3.MoveTowards(
                transform.position,
                positionToGo,
                speedMove * Time.deltaTime * (0.5f));
        if (transform.position == positionToGo)
        {
            currentMovement--;
        }
    }
    public float GetCurrentPercentStair()
    {
        return currentDeplacement;
    }
    #endregion
}
