using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilController : CharacterController
{
    public float distanceTarget = 0.2f;

    private Room _objectiveRoom;
    private Vector3 _targetPosition;

    [Header("Wait Time")]
    public float timeToUseStair;
    public float timeActionRoom;

    private int _stairUse = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.devil = this;
    }

    void Update()
    {
        if (_objectiveRoom)
        {
            if (!canMove)
                return;

            if (_targetPosition == Vector3.zero)
            {
                if (currentLevel == _objectiveRoom.level)
                {
                    _targetPosition = _objectiveRoom.transform.position;
                }
                else
                {
                    if (currentLevel > _objectiveRoom.level)
                    {
                        _stairUse = -1;
                        _targetPosition = HomeController.instance.GetNearDownStair(currentLevel, gameObject).transform.position;
                    }
                    else
                    {
                        _stairUse = 1;
                        _targetPosition = HomeController.instance.GetNearUpStair(currentLevel, gameObject).transform.position;
                    }
                }
                _targetPosition = new Vector3(_targetPosition.x, transform.position.y, transform.position.z);
            }
            else
            {
                if (Vector3.Distance(transform.position, _targetPosition) < distanceTarget)
                {
                    if (_stairUse != 0)
                    {
                        StartCoroutine(UseStair());
                    }
                    else
                    {
                        StartCoroutine(MakeAction());
                    }
                }
                else
                {
                    if (_targetPosition.x > transform.position.x)
                    {
                        transform.Translate(speedMove * Time.deltaTime,0,0);
                    }
                    else
                    {
                        transform.Translate(-speedMove * Time.deltaTime, 0, 0);
                    }
                }
            }

        }
        else
        {
            _objectiveRoom = HomeController.instance.GetRandomRoom(this);
            canMove = true;
            _targetPosition = Vector3.zero;
            _stairUse = 0;
        }
    }

    private IEnumerator UseStair()
    {
        canMove = false;
        Stairs stairs = GetStairs();

        if (_stairUse > 0)
        {
            transform.position = stairs.upStair.transform.position;
        }
        else
        {
            transform.position = stairs.downStair.transform.position;
        }
        currentLevel += _stairUse;

        yield return new WaitForSeconds(timeToUseStair);
        _stairUse = 0;
        _targetPosition = Vector3.zero;
        canMove = true;
        yield break;
    }
    private IEnumerator MakeAction()
    {
        canMove = false;

        yield return new WaitForSeconds(timeActionRoom);
        canMove = true;

        _objectiveRoom = null;
        yield break;
    }
}
