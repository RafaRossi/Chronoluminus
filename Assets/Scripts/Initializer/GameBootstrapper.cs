using System;
using System.Threading.Tasks;
using Commands;
using UnityEngine;

[Serializable]
public class GameInitContext : ICommandContext
{
    [field:SerializeField] public DayController DayController { get; private set; }
}

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private GameInitContext gameInitContext;
    private InitializationQueue<ICommand<GameInitContext>, GameInitContext> _initQueue = new();

    private async void Start()
    {
        _initQueue = new ();
        _initQueue.Enqueue(new InitDayController(gameInitContext.DayController));

        await _initQueue.ProcessAllAsync();
    }
}

public class InitDayController : ICommand<GameInitContext>
{
    private readonly DayController _dayController;

    public InitDayController(DayController dayController)
    {
        _dayController = dayController;
    }
    
    public Task Execute(GameInitContext context)
    {
        _dayController.StartDay();
        return Task.CompletedTask;
    }
}