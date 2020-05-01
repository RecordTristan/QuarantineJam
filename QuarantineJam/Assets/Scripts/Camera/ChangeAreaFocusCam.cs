﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAreaFocusCam : MonoBehaviour
{
    public GameObject camToFocus;

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player")
        {
            CameraManager2D.instance.FocusOnThisCam(camToFocus);
        }
    }
}
