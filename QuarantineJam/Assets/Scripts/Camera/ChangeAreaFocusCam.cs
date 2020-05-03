using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAreaFocusCam : MonoBehaviour
{
    public GameObject camToFocus;
    public int level;

    void Start()
    {
        camToFocus.transform.position = new Vector3(
            camToFocus.transform.position.x,
            LDController.instance.levelCamValue[level],
            -10);
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player")
        {
            CameraManager2D.instance.FocusOnThisCam(camToFocus);
        }
    }
    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player")
        {
            CameraManager2D.instance.FocusOnThisCam(camToFocus);
        }
    }
}
