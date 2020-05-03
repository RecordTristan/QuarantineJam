using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    public Suitcase suitcase;

    private Dictionary<string, GrabObject> _objectList = new Dictionary<string, GrabObject>();

    void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;

        GrabObject[] objectsIG = FindObjectsOfType<GrabObject>();
        for (int i = objectsIG.Length; i-- > 0;)
        {
            if (!(objectsIG[i] as Suitcase))
            {
                if (objectsIG[i].canBeSelect)
                {
                    AddObject(objectsIG[i]);
                }
            }
            else
            {
                suitcase = objectsIG[i] as Suitcase;
            }
        }
    }

    #region ObjectTaker
    public void AddObject(GrabObject objectGrab)
    {
        _objectList.Add(objectGrab.nameGrabObject, objectGrab);
    }
    public void RemoveObject(GrabObject removeObjectGrab)
    {
        _objectList.Remove(removeObjectGrab.nameGrabObject);
    }
    public GrabObject GetInteractObject(string nameObject)
    {
        return _objectList[nameObject];
    }
    public GrabObject GetInteractObject(int indexOfObject)
    {
        return _objectList.Values.ToList()[indexOfObject];
    }
    public int GetNumberOfObject()
    {
        return _objectList.Count;
    }
    #endregion
    
}
