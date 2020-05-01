using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager2D : MonoBehaviour
{
    public static CameraManager2D instance;

    public float speedChangeFocus = 1;

    private Camera _cam;
    void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);

        _cam = GetComponent<Camera>();
    }

    public void FocusOnThisCam(GameObject target)
    {
        _cam.transform.DOMove(target.transform.position, speedChangeFocus).SetEase(Ease.OutExpo);
    }
}
