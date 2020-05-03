using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using DG.Tweening;

public class GrabObject : MonoBehaviour 
{
    public string nameGrabObject;
    public float speedFollow = 2;
    public bool canBeRecup = true;
    public float weight;
    protected Material mat;

    private Sequence _anim;
    protected BoxCollider2D colliderOfObject;

    public bool canBeSelect = true;

    [Header("Sounds")]
    public AudioClip takeObject;
    public AudioClip putObject;

    private bool _take = false;

    void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
        colliderOfObject = GetComponent<BoxCollider2D>();
        _anim = DOTween.Sequence();
    }

    public virtual void UseObject(GrabObject actionGameObject)
    {
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && canBeRecup)
        {
            GameController.instance.player.GiveObject(this);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && canBeRecup)
        {
            GameController.instance.player.RemoveObject(this);
            SetOutline(0);
        }
    }

    public void Take(Transform targetPos)
    {
        if (!_take)
        {
            _take = true;
            SoundController.instance.PlaySFX(takeObject);
        }
        _anim.Kill();
        transform.position = Vector3.Lerp(transform.position, targetPos.position, speedFollow*Time.deltaTime);
        colliderOfObject.enabled = false;
    }

    public void Put(int level)
    {
        if (_take)
        {
            _take = false;
            SoundController.instance.PlaySFX(putObject);
        }
        colliderOfObject.enabled = true;
        _anim.Kill();
        _anim = DOTween.Sequence()
            .Append(transform.DOMoveY(LDController.instance.levelValue[level],0.3f).SetEase(Ease.OutExpo));
    }

    public void SetOutline(float value)
    {
        mat.SetFloat("_Outline", value);
    }
}
