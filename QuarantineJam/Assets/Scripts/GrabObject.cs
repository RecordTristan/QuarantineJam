using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using DG.Tweening;

public class GrabObject : MonoBehaviour 
{
    public string nameGrabObject;
    public float speedFollow = 2;
    private Material _mat;

    private Sequence _anim;

    void Awake()
    {
        _mat = GetComponent<SpriteRenderer>().material;
        _anim = DOTween.Sequence();
    }

    public virtual void UseObject(GrabObject actionGameObject)
    {
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameController.instance.player.GiveObject(this);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameController.instance.player.GiveObject(null);
        }
    }

    public void Take(Transform targetPos)
    {
        _anim.Kill();
        transform.position = Vector3.Lerp(transform.position, targetPos.position, speedFollow*Time.deltaTime);
    }

    public void Put(int level)
    {
        _anim.Kill();
        _anim = DOTween.Sequence()
            .Append(transform.DOMoveY(LDController.instance.levelValue[level],0.3f).SetEase(Ease.OutExpo));
    }
}
