using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ICommandContext { }

public abstract class CommandsQueue<T, TU> : CommandsQueue where T : ICommand<TU> where TU : ICommandContext
{
    private readonly Queue<T> _queue = new();
    private bool _isProcessing = false;
    
    private TU _context;

    public virtual void Initialize(TU context)
    {
        _context = context;
    }
    
    private async Task Process()
    {
        _isProcessing = true;

        while (_queue.Count > 0)
        {
            var action = _queue.Dequeue();
            await action.Execute(_context);
        }

        _isProcessing = false;
    }
    
    public void Enqueue(T action)
    {
        _queue.Enqueue(action);

        if (!_isProcessing)
        {
            _ = Process();
        }
    }

    public void EnqueueFront(T action)
    {
        var temp = new Queue<T>();
        temp.Enqueue(action);

        foreach (var a in _queue)
        {
            temp.Enqueue(a);
        }
        _queue.Clear();

        foreach (var a in temp)
        {
            _queue.Enqueue(a);
        }

        if (!_isProcessing)
        {
            _ = Process();
        }
    }
}

public abstract class CommandsQueue : MonoBehaviour
{

}