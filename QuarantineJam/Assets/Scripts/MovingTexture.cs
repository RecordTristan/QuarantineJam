using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTexture : MonoBehaviour
{
    public float speedX = 1;
    public float speedY = 1;

    Material _mat;
    // Start is called before the first frame update
    void Start()
    {
        _mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        _mat.mainTextureOffset += new Vector2(speedX * Time.deltaTime, speedY * Time.deltaTime);
    }
}
