using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public CharacterController character;
    
    void Awake()
    {
        character = GetComponentInParent<CharacterController>();
    }

    public void Step()
    {
        character.SoundStep();
    }
}
