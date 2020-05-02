using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : Room
{
    public Transform upStair;
    public Transform downStair;

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
        if (!upStair.gameObject.activeSelf)
        {
            upStair = null;
        }
        else
        {
            upStair.position = new Vector3(upStair.position.x, 
                LDController.instance.levelValue[level + 1], 
                upStair.position.z);
        }
        if (!downStair.gameObject.activeSelf)
        {
            downStair = null;
        }
        else
        {
            downStair.position = new Vector3(downStair.position.x,
                LDController.instance.levelValue[level - 1],
                downStair.position.z);
        }
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
