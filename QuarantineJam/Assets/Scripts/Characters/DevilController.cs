using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class DevilController : CharacterController
{
    public float distanceTarget = 0.2f;

    private Room _objectiveRoom;
    private Vector3 _targetPosition;

    [Header("Wait Time")]
    public float timeToUseStair;
    public float timeActionRoom;
    public float timeActionEvent;

    private int _stairUse = 0;

    private Event _objectiveEvent;
    private bool _canEvent = false;
    private bool _eventIsReady = true;
    private bool _coffeGood = false;
    private bool _journalGood = false;
    private bool _waitCoffe = false;
    private bool _waitJournal = false;
    private bool _eventFailed = false;
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
            //Choose A random room
            _objectiveRoom = HomeController.instance.GetRandomRoom(this);
            canMove = true;
            _targetPosition = Vector3.zero;
            _stairUse = 0;
        }


        if (_eventIsReady)
        {
            StartCoroutine(MakeEvent()); 
        }

        if (_canEvent)
        {
            if (_coffeGood || _journalGood)
            {
                Debug.Log("Event Win");
            }
            else if(_eventFailed)
            {
                //defeat
                Debug.Log("Event Defeat");
            }

        }
        
    }

    private IEnumerator UseStair()
    {
        canMove = false;
        Stairs stairs = GetStairs();

        List<Vector3> positionTrans = new List<Vector3>();
        if (_stairUse > 0)
        {
            positionTrans.Add(stairs.currentPlacement.position);
            positionTrans.AddRange(stairs.displacementPosition.Select(s => s.position).ToArray());
            positionTrans.Add(stairs.upStair.currentPlacement.position);
        }
        else
        {
            positionTrans.Add(stairs.downStair.currentPlacement.position);
            positionTrans.AddRange(stairs.downStair.displacementPosition.Select(s => s.position).ToArray());
            positionTrans.Add(stairs.currentPlacement.position);
            positionTrans.Reverse();
        }
        currentLevel += _stairUse;
        transform.DOPath(positionTrans.ToArray(), speedMove).SetSpeedBased(true).OnComplete(()=> {
            _stairUse = 0;
            _targetPosition = Vector3.zero;
            canMove = true;
        });
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

    private IEnumerator MakeEvent()
    {
        
        _canEvent = false;
        _eventIsReady = false;
        _coffeGood = false;
        _journalGood = false;
        _waitCoffe = false;
        _waitJournal = false;
        _eventFailed = false;
        //random temps event
        yield return new WaitForSeconds(timeActionEvent);
        Event();
        _canEvent = true;
        yield return new WaitForSeconds(timeActionEvent);
        _eventFailed = true;
        _eventIsReady = true;
        //_objectiveEvent = null;
        yield break;
    }

    public void Event()
    {
       
        switch (Random.Range(1, 2))
        {
            case 1:
                _waitCoffe = true;
                
                //bulle affichant le coffe 
                break;
            case 2:
                _waitJournal = true;

                //bulle affichant le journal
                break;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_waitCoffe && other.tag == "Coffe")
        {
            _coffeGood = true;
        }
        
        if (_waitJournal && other.tag == "Journal")
        {
            _journalGood = true;
        }
    }
    
}
