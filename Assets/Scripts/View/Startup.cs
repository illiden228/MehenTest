using System;
using System.Collections;
using System.Collections.Generic;
using Logic;
using UnityEngine;
using UnityEngine.Serialization;

public class Startup : MonoBehaviour
{
    private Game _game;
    private void Start()
    {
        _game = new Game();
    }

    void Update()
    {
        _game.Update();
    }
}