using System;
using System.Threading.Tasks;
using Commands;
using UnityEngine;

[Serializable]
public class GameInitContext : ICommandContext
{
    [field:SerializeField] public DiscreteHourController DiscreteHourController { get; private set; }
}

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private GameInitContext gameInitContext;
    private InitializationQueue<ICommand, GameInitContext> _initQueue = new();

    private async void Start()
    {
        _initQueue = new ();
        _initQueue.Enqueue(new InitDayController(gameInitContext.DiscreteHourController));

        await _initQueue.ProcessAllAsync();
    }
}

public class InitDayController : ICommand
{
    private readonly DiscreteHourController _discreteHourController;

    public InitDayController(DiscreteHourController discreteHourController)
    {
        _discreteHourController = discreteHourController;
    }

    public Task Execute()
    {
        _discreteHourController.StartDay();
        return Task.CompletedTask;
    }
}