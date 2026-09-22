using System;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : Singleton<GameManager>
{
    [SerializeField] private UnityEvent initialGameEvent;
    public GameStateMachine GameStateMachine { get; private set; } = new();

    protected override void Awake()
    {
        GameStateMachine = new GameStateMachine();
        ChangeState(new InGame());
    }

    private void Start()
    {
        initialGameEvent?.Invoke();
    }

    private void Update()
    {
        GameStateMachine.Update();
    }

    public void ChangeState(IGameState newState)
    {
        GameStateMachine.ChangeState(newState);
    }
    
    public void PushState(IGameState newState)
    {
        GameStateMachine.PushState(newState);
    }

    public void PopState()
    {
        GameStateMachine.PopState();
    }
}