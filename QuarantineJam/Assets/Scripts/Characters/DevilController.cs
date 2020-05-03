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
    public ParticleSystem effectAngry;
    public ParticleSystem effectCall;

    private Room _objectiveRoom;
    private Vector3 _targetPosition;

    [Header("ShakeStep")]
    public float amplifyStep = 0.1f;
    public float timeStep = 0.1f;

    [Header("ShakeCall")]
    public float amplifyCall = 2.5f;
    public float timeCall = 0.1f;

    [Header("Wait Time")]
    public float timeToUseStair;
    public float timeActionRoom;
    public float timeActionEvent;
    public float timeToHappenDevil;

    private int _stairUse = 0;

    [Header("Event")]
    public GameObject buble;
    public SpriteRenderer displayBuble;
    public Sprite coffee;
    public Sprite newsPaper;

    private Event _objectiveEvent;
    private bool _canEvent = false;
    private bool _eventIsReady = true;
    private bool _coffeGood = false;
    private bool _journalGood = false;
    private bool _waitCoffe = false;
    private bool _waitJournal = false;
    private bool _eventFailed = false;
    private bool _alreadyActivate = false;

    public Animation animHappenDevil;

    private float _xValue;
    private bool _isHere = false;
    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.devil = this;
        StartCoroutine(HappenDevil());
    }

    void Update()
    {
        if (!_isHere)
            return;
        if (transform.position.x > _xValue)
        {
            anim.SetBool("Sit", false);

            anim.SetBool("Walk", true);
            float scale = Mathf.Abs(display.transform.localScale.x);
            display.transform.localScale = new Vector3(scale,scale,scale);
        }
        else if (transform.position.x < _xValue)
        {
            anim.SetBool("Sit", false);

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
        anim.SetBool("Sit", true);

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
        _alreadyActivate = false;
        //random temps event
        Event();
        yield return new WaitForSeconds(timeActionEvent);
        UIController.instance.EndEvent();
        buble.SetActive(false);
        CameraManager2D.instance.OutScreen();
        if (_coffeGood || _journalGood)
        {
            Debug.Log("Event Win");
            _objectiveRoom = null;
            yield return new WaitForSeconds(timeActionEvent);
        }
        else
        {
            //defeat
            Debug.Log("Event Defeat");
            effectAngry.Play();
            _objectiveRoom = HomeController.instance.GetNearRoom(GameController.instance.player.GetCurrentRoom());
            _targetPosition = Vector3.zero;
            _stairUse = 0;
        }
        canMove = true;
        _eventIsReady = true;
        //_objectiveEvent = null;
        yield break;
    }

    private IEnumerator HappenDevil()
    {
        yield return new WaitForSeconds(timeToHappenDevil);
        anim.SetBool("Walk", true);
        transform.DOMoveX(HomeController.instance.firstRoom.transform.position.x, 4).OnComplete(()=> _isHere = true);
        yield break;
    }

    public void Event()
    {
        if (_waitCoffe || _waitJournal)
            return;
        SoundController.instance.PlaySFX(callDevil);
        CameraManager2D.instance.ShakeCam(amplifyCall, timeCall);
        anim.SetTrigger("Call");
        effectCall.Play();

        buble.SetActive(true);
        switch (Random.Range(0, 2))
        {
            case 0:
                _waitCoffe = true;
                Debug.Log("Go Coffee");
                displayBuble.sprite = coffee;
                //bulle affichant le coffe 
                break;
            case 1:
                _waitJournal = true;
                displayBuble.sprite = newsPaper;
                Debug.Log("Go Journal");
                //bulle affichant le journal
                break;
        }
        UIController.instance.SetPanel(displayBuble.sprite);
    }

    public void OnTriggerStay2D(Collider2D other)
    {

        if (_waitCoffe && other.tag == "Coffee" && !_alreadyActivate)
        {
            buble.SetActive(false);
            CameraManager2D.instance.OutScreen();
            _coffeGood = true;
            _alreadyActivate = true;
        }
        
        if (_waitJournal && other.tag == "NewsPaper" && !_alreadyActivate)
        {
            buble.SetActive(false);
            CameraManager2D.instance.OutScreen();
            _journalGood = true;
            anim.SetTrigger("NewsPaper");
            _alreadyActivate = true;
        }
    }

    public override void SoundStep()
    {
        base.SoundStep();
        CameraManager2D.instance.ShakeCam(amplifyStep, timeStep);
    }

}
