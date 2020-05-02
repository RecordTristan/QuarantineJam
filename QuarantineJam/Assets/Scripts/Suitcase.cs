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
    public float multiplyWeight = 2;


    public override void UseObject(GrabObject actionGameObject)
    {
        actionGameObject.transform.SetParent(transform);

        actionGameObject.transform.position = transform.position;
        

        if (!objets.Contains(actionGameObject) && actionGameObject != this)
        {
            actionGameObject.canBeRecup = false;
            actionGameObject.GetComponent<BoxCollider2D>().enabled = false;
            objets.Add(actionGameObject);
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
        }

        GameController.instance.CheckWin(objets);
        
    }
    
}
