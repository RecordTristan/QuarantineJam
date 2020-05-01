using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager2D : MonoBehaviour
{
    public static CameraManager2D instance;
    
    void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }

    public void FocusOnThisCam(GameObject target)
    {

    }
}
