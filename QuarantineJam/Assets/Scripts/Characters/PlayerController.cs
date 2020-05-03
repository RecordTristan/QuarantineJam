using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CharacterController
{

    [Header("Settings")]
    private float _baseSpeedMove;
    public float deadZoneVertical = 0.5f;
    public ParticleSystem effectDead;
    public GameObject UIPause;

    private bool _usedStair = false;
    private BoxCollider2D _collider;

    public Animation winAnim;

    //Grab
    [Header("Levitation")]
    public float limitLevitation = 2;
    public float speedLevitation = 2;
    public Transform objectGrabPos;
    private Vector3 _basePoseGrab;
    private List<GrabObject> _grabObjectDetection = new List<GrabObject>();
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

        if (Input.GetButtonDown("ActiveUI"))
        {
            UIController.instance.ActiveList();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("TristanScene");
        }
        if (Input.GetButtonDown("Cancel"))
        {
            UIPause.SetActive(true);
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
        if (_startVertical)
            return;
        if (!Input.GetButtonDown("Interact"))
            return;

        anim.SetTrigger("TakeObject");

        if (_currentGrab)
        {
            GetObject()?.UseObject(_currentGrab);
            _currentGrab.Put(currentLevel);
            _currentGrab = null;
            speedMove = _baseSpeedMove;
            anim.speed = 1;
        }
        else
        {
            _currentGrab = GetObject();
            if (_currentGrab)
            {
                anim.speed = 1 / _currentGrab.weight;
                speedMove = _baseSpeedMove / _currentGrab.weight;
            }
        }
        if (_grabObjectDetection.Contains(ObjectManager.instance.suitcase))
        {
            if (GetObjectInHand() != null && GetObjectInHand() != ObjectManager.instance.suitcase)
            {
                ObjectManager.instance.suitcase.anim.SetBool("Open", true);
            }
            else
            {
                ObjectManager.instance.suitcase.anim.SetBool("Open", false);
            }
        }
        else
        {
            ObjectManager.instance.suitcase.anim.SetBool("Open", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Devil")
        {
            effectDead.Play();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Devil")
        {
            effectDead.Stop();
        }
    }

    #region End
    public void Loose()
    {
        if (GameController.instance.isEnd)
            return;
        anim.SetTrigger("Loose");
        canMove = false;
    }
    public void Win()
    {
        anim.SetTrigger("Win");
        winAnim.Play();
        canMove = false;
    }
    #endregion

    #region Object
    public void GiveObject(GrabObject grabObject)
    {
        _grabObjectDetection.Add(grabObject);
        float dist = 1000;
        int index = -1;
        for (int i = 0; i < _grabObjectDetection.Count; i++)
        {
            if (Vector3.Distance(_grabObjectDetection[i].transform.position, transform.position) < dist)
            {
                dist = Vector3.Distance(_grabObjectDetection[i].transform.position, transform.position);
                index = i;
            }
            else
            {
                _grabObjectDetection[i].SetOutline(0);
            }
        }
        if (_grabObjectDetection.Contains(ObjectManager.instance.suitcase))
        {
            if (GetObjectInHand() != null)
            {
                ObjectManager.instance.suitcase.anim.SetBool("Open", true);
            }
            else
            {
                ObjectManager.instance.suitcase.anim.SetBool("Open", false);
            }
        }
        else
        {
            ObjectManager.instance.suitcase.anim.SetBool("Open", false);
        }
        if (index != -1)
        {
            _grabObjectDetection[index].SetOutline(0.02f);
        }
    }
    public void RemoveObject(GrabObject grabObject)
    {
        _grabObjectDetection.Remove(grabObject);
        float dist = 1000;
        int index = -1;
        for (int i = 0; i < _grabObjectDetection.Count; i++)
        {
            if (Vector3.Distance(_grabObjectDetection[i].transform.position, transform.position) < dist)
            {
                dist = Vector3.Distance(_grabObjectDetection[i].transform.position, transform.position);
                index = i;
            }
            else
            {
                _grabObjectDetection[i].SetOutline(0);
            }
        }
        if (_grabObjectDetection.Contains(ObjectManager.instance.suitcase))
        {
            if (GetObjectInHand() != null)
            {
                ObjectManager.instance.suitcase.anim.SetBool("Open", true);
            }
            else
            {
                ObjectManager.instance.suitcase.anim.SetBool("Open", false);
            }
        }
        else
        {
            ObjectManager.instance.suitcase.anim.SetBool("Open", false);
        }
        if (index != -1)
        {
            _grabObjectDetection[index].SetOutline(0.02f);
        }
    }
    public GrabObject GetObject()
    {
        float dist = 1000;
        int index = -1;
        for (int i = 0; i < _grabObjectDetection.Count; i++)
        {
            if (Vector3.Distance(_grabObjectDetection[i].transform.position, transform.position) < dist)
            {
                dist = Vector3.Distance(_grabObjectDetection[i].transform.position, transform.position);
                index = i;
            }
            else
            {
                _grabObjectDetection[i].SetOutline(0);
            }
        }
        if (index == -1)
        {
            return null;
        }
        else
        {
            _grabObjectDetection[index].SetOutline(0.02f);
            return _grabObjectDetection[index];
        }
    }
    public bool GrabOjbect()
    {
        return _currentGrab ? true : false;
    }
    public GrabObject GetObjectInHand()
    {
        return _currentGrab;
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
