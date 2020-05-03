using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Suitcase : GrabObject
{
    private bool chekWin = false;
    private GrabObject _grabObject;
    public List<GrabObject> objets = new List<GrabObject>();
    public float multiplyWeight = 2;

    public Animator anim;
    public ParticleSystem effectItem;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public override void UseObject(GrabObject actionGameObject)
    {
        if (!objets.Contains(actionGameObject) && actionGameObject != this && actionGameObject.canBeSelect)
        {
            effectItem.Play();
            actionGameObject.transform.SetParent(transform);
            actionGameObject.transform.position = transform.position;
            actionGameObject.canBeRecup = false;
            actionGameObject.GetComponent<BoxCollider2D>().enabled = false;
            objets.Add(actionGameObject);
            actionGameObject.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
            weight = 0;
            for (int i = 0; i < objets.Count; i++)
            {
                weight += objets[i].weight;
            }
            weight /= multiplyWeight;
            if (weight < 1)
            {
                weight = 1;
            }
            GameController.instance.AddObject(actionGameObject);
            UIController.instance.ValidateObject(actionGameObject.nameGrabObject);
        }        
    }
    
}
