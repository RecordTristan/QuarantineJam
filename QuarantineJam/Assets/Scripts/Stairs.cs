using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : Room
{
    public Stairs upStair;
    public Transform currentPlacement;
    public Stairs downStair;
    public Transform[] displacementPosition;

    public SpriteRenderer display;
    private Material _mat;

    void Start()
    {
        if (display)
        {
            _mat = display.material;
        }
        else
        {
            _mat = GetComponent<SpriteRenderer>().material;
        }

        currentPlacement.transform.position = new Vector3(
            currentPlacement.transform.position.x,
            LDController.instance.levelValue[level],
            currentPlacement.transform.position.z);

    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == "Player")
        {
            GameController.instance.player.SetStair(this);
            _mat.SetFloat("_Outline", 0.02f);

        }
        else if (other.tag == "Devil")
        {
            GameController.instance.devil.SetStair(this);
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);

        if (other.tag == "Player")
        {
            GameController.instance.player.ExitStair(this);
            _mat.SetFloat("_Outline", 0f);
        }
        else if (other.tag == "Devil")
        {
            GameController.instance.devil.SetStair(this);
        }
    }

    public void Enter()
    {
        //_mat.SetFloat("_Outline", 0.02f);
    }
    public void Leave()
    {
        //_mat.SetFloat("_Outline", 0f);
    }
}
