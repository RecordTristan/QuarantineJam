using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speedMove = 4;
    private float _baseSpeedMove;
    public float deadZoneVertical = 0.5f;

    void Awake()
    {
        _baseSpeedMove = speedMove;
    }

    void Update()
    {
        Move();
        Interact();
        DisplaceVertical();
    }

    private void Move()
    {
        float Horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speedMove;

        if (Horizontal == 0)
            return;

        transform.Translate(Horizontal, 0, 0);
    }

    private void Interact()
    {
        if (!Input.GetButtonDown("Interact"))
            return;

        //ToDo interact with an object
    }

    private void DisplaceVertical()
    {
        float Vertical = Input.GetAxis("Vertical");
        if (Vertical > -deadZoneVertical && Vertical < deadZoneVertical)
            return;

        //ToDo displace on stairs
    }
}
