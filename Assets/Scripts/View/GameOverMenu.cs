using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    private UnityAction _restartAction;

    public void Init(UnityAction restartAction)
    {
        _restartAction = restartAction;
        _restartButton.onClick.AddListener(_restartAction);
    }

    private void OnEnable()
    {
        if(_restartAction != null)
            _restartButton.onClick.AddListener(_restartAction);
    }

    private void OnDisable()
    {
        if(_restartAction != null)
            _restartButton.onClick.RemoveListener(_restartAction);
    }
}
