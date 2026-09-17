using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GameActions : MonoBehaviour, IGameAction
{
    public abstract Task Execute(CancellationToken token);
}

public interface IGameAction : ICommand
{
    Task Execute(CancellationToken token);
    Task ICommand.Execute() => Execute(CancellationToken.None);
}