using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Suitcase : GrabObject
{

    private GrabObject _grabObject;
    
    public List<GameObject> objets = new List<GameObject>();

    public override void UseObject(GameObject actionGameObject)
    {
        actionGameObject.transform.SetParent(transform);
        objets.Add(actionGameObject);
    }

    //protected override void OnTriggerEnter2D(Collider2D other)
    //{
    //    base.OnTriggerEnter2D(other);

    //    if (other.tag == "Item")
    //    {
           
    //    }
    //}

    
}
