using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class DevilController : CharacterController
{
    public AudioClip callDevil;

    [Header("Settings")]
    public float distanceTarget = 0.2f;

    private Room _objectiveRoom;
    private Vector3 _targetPosition;

    [Header("Wait Time")]
    public float timeToUseStair;
    public float timeActionRoom;
    public float timeActionEvent;
    public float timeToHappenDevil;

    private int _stairUse = 0;

    private Event _objectiveEvent;
    private bool _canEvent = false;
    private bool _eventIsReady = true;
    private bool _coffeGood = false;
    private bool _journalGood = false;
    private bool _waitCoffe = false;
    private bool _waitJournal = false;
    private bool _eventFailed = false;

    public Animation animHappenDevil;

    private float _xValue;
    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.devil = this;
        StartCoroutine(HappenDevil());
    }

    void Update()
    {
        if (transform.position.x > _xValue)
        {
            anim.SetBool("Walk", true);
            float scale = Mathf.Abs(display.transform.localScale.x);
            display.transform.localScale = new Vector3(scale,scale,scale);
        }
        else if (transform.position.x < _xValue)
        {
            anim.SetBool("Walk", true);
            float scale = Mathf.Abs(display.transform.localScale.x);
            display.transform.localScale = new Vector3(-scale, scale, scale);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
        _xValue = transform.position.x;

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
        transform.DOPath(positionTrans.ToArray(), speedMove).SetSpeedBased(false).OnComplete(()=> {
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
        switch (Random.Range(0, 2))
        {
            case 0:
                yield return MakeEvent();
                break;
            case 1:
                canMove = true;
                _objectiveRoom = null;
                break;
        }
        
        yield break;
    }

    private IEnumerator MakeEvent()
    {
        _eventIsReady = false;
        _coffeGood = false;
        _journalGood = false;
        _waitCoffe = false;
        _waitJournal = false;
        _eventFailed = false;
        //random temps event
        Event();
        yield return new WaitForSeconds(timeActionEvent);
        if (_coffeGood || _journalGood)
        {
            Debug.Log("Event Win");
            _objectiveRoom = null;
        }
        else
        {
            //defeat
            Debug.Log("Event Defeat");
            _objectiveRoom = HomeController.instance.GetNearRoom(GameController.instance.player.GetCurrentRoom());
            _targetPosition = Vector3.zero;
            _stairUse = 0;
            Debug.Log(_objectiveRoom.name);
        }
        yield return new WaitForSeconds(timeActionEvent);
        canMove = true;
        _eventIsReady = true;
        //_objectiveEvent = null;
        yield break;
    }

    private IEnumerator HappenDevil()
    {
        yield return new WaitForSeconds(timeToHappenDevil);
        this.gameObject.SetActive(true);
        //animHappenDevil?.Play();
        yield break;
    }

    public void Event()
    {
        if (_waitCoffe || _waitJournal)
            return;
        SoundController.instance.PlaySFX(callDevil);

        switch (Random.Range(0, 2))
        {
            case 0:
                _waitCoffe = true;
                Debug.Log("Go Coffee");
                //bulle affichant le coffe 
                break;
            case 1:
                _waitJournal = true;
                Debug.Log("Go Journal");
                //bulle affichant le journal
                break;
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {

        if (_waitCoffe && other.tag == "Coffee")
        {
            _coffeGood = true;
        }
        
        if (_waitJournal && other.tag == "NewsPaper")
        {
            _journalGood = true;
        }
    }
    
}
