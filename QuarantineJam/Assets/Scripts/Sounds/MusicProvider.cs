using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicProvider : MonoBehaviour
{
    public TypeMusic type;
    // Start is called before the first frame update
    void Start()
    {
        SoundController.instance.PlayTypeMusic(type);
    }
}
