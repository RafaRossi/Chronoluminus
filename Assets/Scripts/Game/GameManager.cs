using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private UnityEvent initialGameEvent;

    private void Start()
    {
        initialGameEvent?.Invoke();
    }
}
