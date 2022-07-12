using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Keyboard _keyboard;

    private void Awake()
    {
        _keyboard = GetComponent<Keyboard>();

        _keyboard.Entered += () => { Debug.Log("Entered."); };
    }
    
}
