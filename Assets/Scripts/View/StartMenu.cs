using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    private UnityAction _startAction;

    public void Init(UnityAction startAction)
    {
        _startAction = startAction;
        _startButton.onClick.AddListener(_startAction);
    }

    private void OnEnable()
    {
        if(_startAction != null)
            _startButton.onClick.AddListener(_startAction);
    }

    private void OnDisable()
    {
        if(_startAction != null)
            _startButton.onClick.RemoveListener(_startAction);
    }
}