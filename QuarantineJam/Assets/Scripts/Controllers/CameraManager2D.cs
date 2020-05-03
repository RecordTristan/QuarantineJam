using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager2D : MonoBehaviour
{
    public static CameraManager2D instance;

    private GameObject _groupCam;

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

        if (_positionStair == Vector3.zero)
            return;
    }

    public void FocusOnThisCam(GameObject target)
    {
        _anim.Kill();
        _targetPos = target.transform.position;

        _anim = DOTween.Sequence()
            .Append(_cam.transform.DOMove(target.transform.position, speedChangeFocus ).SetEase(Ease.OutExpo));
        _positionStair = Vector3.zero;
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
}
