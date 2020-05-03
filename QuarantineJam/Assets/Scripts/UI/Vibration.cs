using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    public float amplify = 0.3f;
    public float timeVibration;

    private Vector3 _positionBase;

    void Start()
    {
        _positionBase = transform.position;
    }

    void Update()
    {
        if (timeVibration == -1)
        {
            float randX = UnityEngine.Random.Range(-amplify, amplify) * Time.deltaTime;
            float randY = UnityEngine.Random.Range(-amplify, amplify) * Time.deltaTime;

            transform.position = new Vector3(_positionBase.x + randX, _positionBase.y + randY, 0);
        }
        else
        {
            if (timeVibration > 0)
            {
                timeVibration -= Time.deltaTime;

                float randX = UnityEngine.Random.Range(-amplify, amplify) * Time.deltaTime;
                float randY = UnityEngine.Random.Range(-amplify, amplify) * Time.deltaTime;

                transform.position = new Vector3(_positionBase.x + randX, _positionBase.y + randY, 0);
            }
        }
    }

    public void Vibrate(float time = -1)
    {
        if (time == -1)
            return;
        timeVibration = time;
    }
}
