using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;

public class Suitcase : GrabObject
{
    private bool chekWin = false;
    private GrabObject _grabObject;
    public List<GrabObject> objets = new List<GrabObject>();


    public override void UseObject(GrabObject actionGameObject)
    {
        actionGameObject.transform.SetParent(transform);

        actionGameObject.transform.position = transform.position;
        

        if (!objets.Contains(actionGameObject) && actionGameObject != this)
        {
            actionGameObject.GetComponent<BoxCollider2D>().enabled = false;
            objets.Add(actionGameObject);
            objets.Remove(this);
        }

        GameController.instance.CheckWin(objets);
        
    }
    
}
