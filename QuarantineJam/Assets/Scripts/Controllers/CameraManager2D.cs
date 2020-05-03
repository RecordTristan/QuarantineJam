using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager2D : MonoBehaviour
{
    public static CameraManager2D instance;

    private GameObject _groupCam;
    private List<GameObject> _targetList = new List<GameObject>();

    [Header("Shake")]
    public float amplify = 1;
    public float timeShake = 0;
    private Vector3 _placementGroup;

    [Header("Follow")]
    public float speedChangeFocus = 1;

    private Camera _cam;
    private Sequence _anim;

    private Vector3 _targetPos;
    private Vector3 _positionStair;
    private float _percent;

    void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);

        _groupCam = transform.parent.gameObject;
        _placementGroup = _groupCam.transform.position;

        _cam = GetComponent<Camera>();
        _anim = DOTween.Sequence();
    }

    void Update()
    {
        if (timeShake > 0)
        {
            timeShake -= Time.deltaTime;
            float posY = UnityEngine.Random.Range(-amplify, amplify);
            float posX = UnityEngine.Random.Range(-amplify, amplify);
            _groupCam.transform.position = _placementGroup;
            _groupCam.transform.position += new Vector3(posX*Time.deltaTime, posY*Time.deltaTime, 0);
        }
    }

    public void FocusOnThisCam(GameObject target)
    {
        _anim.Kill();
        _targetPos = target.transform.position;
        if (!_targetList.Contains(target))
        {
            _targetList.Add(target);
        }
        _anim = DOTween.Sequence()
            .Append(_cam.transform.DOMove(target.transform.position, speedChangeFocus ).SetEase(Ease.OutExpo));
    }
    public void DontFocusOnThisCam(GameObject target)
    {
        if(target == _targetList[_targetList.Count-1])
        {
            _anim.Kill();
            _targetList.Remove(target);
            _anim = DOTween.Sequence()
            .Append(_cam.transform.DOMove(_targetList[_targetList.Count - 1].transform.position, speedChangeFocus).SetEase(Ease.OutExpo));
        }
        else
        {
            _targetList.Remove(target);
        }
    }

    public void StairMovement(int level)
    {
        //_anim.Complete();
        //_anim.Kill();

        _positionStair = new Vector3(_targetPos.x, LDController.instance.levelCamValue[level], _targetPos.z);
    }

    public void ShakeCam(float amplify, float time)
    {
        this.amplify = amplify;
        timeShake = time;
    }

    public void ClampPositionToScreenPanel()
    {
        Vector3 bottomLeft = _cam.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = _cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, _cam.pixelHeight, 0));

        //Calculate screen size
        Vector2 screenSize = new Vector2(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        Vector2 halfScreenSize = screenSize / 2;

        Transform devilTrans = GameController.instance.devil.transform;
        bool inX = false;
        bool inY = false;
        bool left = false;
        int level;
        if (devilTrans.position.x > bottomLeft.x && devilTrans.position.x < topRight.x)
        {
            inX = true;
        }
        if (devilTrans.position.y > bottomLeft.y && devilTrans.position.y < topRight.y)
        {
            inY = true;
        }
        if(inX && inY)
        {
            OutScreen();
            return;
        }
        if (devilTrans.position.x < 0)
        {
            left = true;
        }
        OutScreen();

        int levelDevil = GameController.instance.devil.GetCurrentRoom().level;
        int levelPlayer = GameController.instance.player.GetCurrentRoom().level;
        if ( levelDevil < levelPlayer)
        {
            if (left)
            {
                UIController.instance.blPanel.SetActive(true);
            }
            else
            {
                UIController.instance.brPanel.SetActive(true);
            }
        }
        else if (levelPlayer == levelDevil)
        {
            if (left)
            {
                UIController.instance.lPanel.SetActive(true);
            }
            else
            {
                UIController.instance.rPanel.SetActive(true);
            }
        }
        else
        {
            if (left)
            {
                UIController.instance.tlPanel.SetActive(true);
            }
            else
            {
                UIController.instance.trPanel.SetActive(true);
            }
        }
    }
    public void OutScreen()
    {

        UIController.instance.tlPanel.SetActive(false);
        UIController.instance.trPanel.SetActive(false);
        UIController.instance.rPanel.SetActive(false);
        UIController.instance.brPanel.SetActive(false);
        UIController.instance.blPanel.SetActive(false);
        UIController.instance.lPanel.SetActive(false);
    }
}
