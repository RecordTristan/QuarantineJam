using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;

public class Suitcase : GrabObject
{
    private bool chekWin = false;
    private GrabObject _grabObject;
    public List<GameObject> objets = new List<GameObject>();


    public override void UseObject(GameObject actionGameObject)
    {
        actionGameObject.transform.SetParent(transform);

        actionGameObject.transform.position = transform.position;

        actionGameObject.GetComponent<BoxCollider2D>().enabled = false;

        if (!objets.Contains(actionGameObject))
        {
            objets.Add(actionGameObject);
            objets.Remove(this.gameObject);


        }

        List<GameObject> compareToListFinish = objets.Where(i => GameController.instance.winItems.Contains(i)).ToList();

        if (compareToListFinish.Count == GameController.instance.winItems.Count)
        {
            Debug.Log("Win");
            //chekWin;
        }
    }
    
}
