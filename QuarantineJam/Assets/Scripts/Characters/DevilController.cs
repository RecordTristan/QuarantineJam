using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilController : CharacterController
{
    public float distanceTarget = 0.2f;

    private Room _objectiveRoom;
    private Vector3 _targetPosition;

    public float timeToUseStair;
    private bool _canMove = false;
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
            if (!_canMove)
                return;

            if (_targetPosition == Vector3.zero)
            {
                if (GetCurrentRoom().level == _objectiveRoom.level)
                {
                    _targetPosition = _objectiveRoom.transform.position;
                }
                else
                {
                    if (GetCurrentRoom().level > _objectiveRoom.level)
                    {
                        _stairUse = 1;
                        _targetPosition = HomeController.instance.GetNearUpStair().transform.position;
                    }
                    else
                    {
                        _stairUse = -1;
                        _targetPosition = HomeController.instance.GetNearDownStair().transform.position;
                    }
                }
            }
            else
            {
                
            }

        }
        else
        {
            _objectiveRoom = HomeController.instance.GetRandomRoom(this);
            _canMove = true;
            _targetPosition = Vector3.zero;
            _stairUse = 0;
        }
    }
}
