using System.Collections.Generic;

public abstract class StateMachine<T> where T : class, IState
{
    private readonly Stack<T> _stateStack = new();
    
    public T CurrentState => _stateStack.Count > 0 ? _stateStack.Peek() : null;

    public void ChangeState(T newState)
    {
        while (_stateStack.Count > 0)
        {
            _stateStack.Pop().Exit();
        }

        _stateStack.Push(newState);
        newState.Enter();
    }
    
    public void Update()
    {
        CurrentState?.Tick();
    }
    
    public void PushState(T newState)
    {
        if (CurrentState == newState) return;

        CurrentState?.Pause();
        
        _stateStack.Push(newState);
        newState.Enter();
    }
    
    public void PopState()
    {
        if (_stateStack.Count > 0)
        {
            _stateStack.Pop().Exit();
        }

        if (_stateStack.Count > 0)
        {
            _stateStack.Peek().Resume();
        }
    }
}

public interface IState
{
    void Enter();
    void Tick();
    void Exit();
    void Pause();
    void Resume();
}
