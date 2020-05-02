using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDController : MonoBehaviour
{
    public static LDController instance;

    public float[] levelValue;
    public float[] levelCamValue;

    void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }

}
