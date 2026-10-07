using System.Threading.Tasks;

public interface IBattleState : IState { }

public class BattleStateMachine : StateMachine<IBattleState>
{
    public BattleStateMachine(IBattleState initialState) : base(initialState) { }
}

public abstract class BattleState : IBattleState
{
    protected readonly BattleManager battleManager;

    protected BattleState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void Exit() { }
    public virtual void Pause() { }
    public virtual void Resume() { }
}

public class BattleInitialState : BattleState
{
    public BattleInitialState(BattleManager battleManager) : base(battleManager) { }

    public override void Enter()
    {
        battleManager.InitializeUnits();
        battleManager.SetupActiveUnits();

        BattleEvents.BattleStarted?.Invoke();
    }
}

public class BattleStartRoundState : BattleState
{
    public BattleStartRoundState(BattleManager battleManager) : base(battleManager) { }

    public override void Enter()
    {
        battleManager.StartNewRound();
        battleManager.ChangeState(new BattleStartTurnState(battleManager));
    }
}

public class BattleStartTurnState : BattleState
{
    public BattleStartTurnState(BattleManager battleManager) : base(battleManager) { }

    public override void Enter()
    {
        if (!battleManager.CurrentRound.NextTurn())
        {
            BattleEvents.RoundEnded?.Invoke(battleManager.CurrentRound);
            
            battleManager.ChangeState(new BattleStartRoundState(battleManager));
            return;
        }

        var turn = battleManager.CurrentRound.CurrentTurn;

        if (!turn.Unit.IsAlive)
        {
            battleManager.ChangeState(new BattleStartTurnState(battleManager));
            return;
        }

        turn.Unit.StartUnitTurn();
        BattleEvents.TurnStarted?.Invoke(turn);

        battleManager.ChangeState(turn.Unit.Team == Team.Player
            ? new BattlePlayerTurn(battleManager)
            : new BattleEnemyTurn(battleManager));
    }
}

public class BattlePlayerTurn : BattleState
{
    public BattlePlayerTurn(BattleManager battleManager) : base(battleManager) { }

    public override void Enter()
    {
        base.Enter();
        
        BattleEvents.OnPlayerTurnStarted?.Invoke();
    }

    // Aqui entra o que é específico do turno do jogador (habilitar input/menus de ação).
}

public class BattleEnemyTurn : BattleState
{
    public BattleEnemyTurn(BattleManager battleManager) : base(battleManager) { }

    // Aqui entra o que é específico do turno do inimigo (rodar a IA).
}

public class BattleEndTurnState : BattleState
{
    private const int AwaitTime = 2000;
    private readonly Team _team;

    public BattleEndTurnState(BattleManager battleManager, Team team) : base(battleManager)
    {
        _team = team;
    }

    public override async void Enter()
    {
        BattleEvents.TurnEnded?.Invoke();

        if (_team == Team.Player)
        {
            BattleEvents.OnPlayerTurnEnded?.Invoke();
        }
        
        await Task.Delay(AwaitTime);
        
        var result = battleManager.GetVictoryState();

        battleManager.ChangeState(result == VictoryState.None
            ? new BattleStartTurnState(battleManager)
            : new BattleEndState(battleManager, result));
    }
}

public class BattleEndState : BattleState
{
    private readonly VictoryState _result;

    public BattleEndState(BattleManager battleManager, VictoryState result) : base(battleManager)
    {
        _result = result;
    }

    public override void Enter()
    {
        BattleEvents.BattleEnded?.Invoke(_result);
    }
} 