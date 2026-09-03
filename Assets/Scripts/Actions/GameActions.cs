using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GameActions : ScriptableObject
{
    public abstract IGameAction Generate();
}

public interface IGameAction : ICommand<GameInitContext> { }
