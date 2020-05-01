using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;
    public int nbrObjectsToRecup;

    private Dictionary<string, GameObject> _objectList = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }

    #region ObjectTaker
    [System.Obsolete("Replace to grab object")]
    public void AddObject(GameObject objectGrab)
    {
        _objectList.Add("", objectGrab);
    }
    [System.Obsolete("Replace to grab object")]
    public void RemoveObject(GameObject removeObjectGrab)
    {
        _objectList.Remove("");
    }
    public GameObject GetInteractObject(string nameObject)
    {
        return _objectList[nameObject];
    }
    #endregion

    public void GetObjects()
    {

    }
}
