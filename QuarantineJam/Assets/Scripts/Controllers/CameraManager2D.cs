using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager2D : MonoBehaviour
{
    public static CameraManager2D instance;

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

        _cam = GetComponent<Camera>();
        _anim = DOTween.Sequence();
    }

    void Update()
    {
        if (_positionStair == Vector3.zero)
            return;

        //if (_percent > GameController.instance.player.GetCurrentPercentStair())
        //{
        //    _percent -= Time.deltaTime;
        //}
        //else if(_percent < GameController.instance.player.GetCurrentPercentStair())
        //{
        //    _percent += Time.deltaTime;
        //}

        //_cam.transform.position = Vector3.Lerp(_targetPos, _positionStair, _percent);
    }

    public void FocusOnThisCam(GameObject target)
    {
        _anim.Kill();
        _targetPos = target.transform.position;
        _anim = DOTween.Sequence()
            .Append(_cam.transform.DOMove(target.transform.position, speedChangeFocus).SetEase(Ease.OutExpo));
        _positionStair = Vector3.zero;
    }

    public void StairMovement(int level)
    {
        //_anim.Complete();
        //_anim.Kill();

        _positionStair = new Vector3(_targetPos.x, LDController.instance.levelCamValue[level], _targetPos.z);
    }
}
