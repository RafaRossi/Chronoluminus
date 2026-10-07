using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private BattleCameraDirector battleCameraDirector;

    [SerializeField] private List<BattleUnit> playerUnits = new();
    [SerializeField] private List<BattleUnit> enemyUnits = new();

    [SerializeField, ReadOnly] private string currentStateName;

    private readonly List<BattleUnit> _battleUnits = new();
    private readonly List<BattleUnitAsset> _playerUnitsAssets = new();
    private readonly List<BattleUnitAsset> _enemyUnitsAssets = new();

    private const int InitiativeThreshold = 100;

    public BattleStateMachine BattleStateMachine { get; private set; }
    public BattleRound CurrentRound { get; private set; }

    private void Start()
    {
        BattleStateMachine = new BattleStateMachine(new BattleInitialState(this));
    }

    public void ChangeState(IBattleState newState)
    {
        currentStateName = newState.GetType().Name;
        Debug.Log($"[Battle] → {currentStateName}");
        BattleStateMachine.ChangeState(newState);
    }
    
    public void StartBattle() => ChangeState(new BattleStartRoundState(this));
    
    [ContextMenu("End Turn")]
    public void EndTurn() => ChangeState(new BattleEndTurnState(this, CurrentRound.CurrentTurn.Unit.Team));

    public void RegisterUnits(List<BattleUnitAsset> playerAssets, List<BattleUnitAsset> enemyAssets)
    {
        _playerUnitsAssets.Clear();
        _enemyUnitsAssets.Clear();

        _playerUnitsAssets.AddRange(playerAssets);
        _enemyUnitsAssets.AddRange(enemyAssets);
    }

    public void InitializeUnits()
    {
        _battleUnits.Clear();

        for (var i = 0; i < Mathf.Min(_playerUnitsAssets.Count, playerUnits.Count); i++)
        {
            playerUnits[i].InitializeUnit(_playerUnitsAssets[i], Team.Player);
            _battleUnits.Add(playerUnits[i]);
        }

        for (var i = 0; i < Mathf.Min(_enemyUnitsAssets.Count, enemyUnits.Count); i++)
        {
            enemyUnits[i].InitializeUnit(_enemyUnitsAssets[i], Team.Enemy);
            _battleUnits.Add(enemyUnits[i]);
        }
    }

    public void SetupActiveUnits()
    {
        foreach (var battleUnit in _battleUnits)
        {
            battleUnit.UpdateState();
        }
    }

    public void StartNewRound()
    {
        var (turns, bonusTurns) = GenerateTurns();
        var roundNumber = CurrentRound != null ? CurrentRound.RoundNumber + 1 : 1;

        CurrentRound = new BattleRound(roundNumber, turns, bonusTurns);
        BattleEvents.RoundStarted?.Invoke(CurrentRound);
    }

    public VictoryState GetVictoryState()
    {
        var playerAlive = playerUnits.Any(u => u.IsAlive);
        var enemyAlive = enemyUnits.Any(u => u.IsAlive);

        if (playerAlive && enemyAlive) return VictoryState.None;
        if (!playerAlive && !enemyAlive) return VictoryState.Tie;
        return playerAlive ? VictoryState.Player : VictoryState.Enemy;
    }

    private (Queue<BattleTurn> turns, Queue<BattleTurn> bonusTurns) GenerateTurns()
    {
        var turnsData = new List<(int initiative, BattleTurn turn)>();
        var bonusTurnsData = new List<(int initiative, BattleTurn turn)>();

        foreach (var unit in _battleUnits)
        {
            if (!unit.IsAlive || !unit.IsActive) continue;

            var initiative = unit.GetInitiativeValue();
            turnsData.Add((initiative, new BattleTurn(unit)));

            while (initiative >= InitiativeThreshold)
            {
                initiative -= InitiativeThreshold;
                bonusTurnsData.Add((initiative, new BattleTurn(unit)));
            }
        }
        
        var turns = new Queue<BattleTurn>(turnsData.OrderByDescending(t => t.initiative).Select(t => t.turn));
        var bonusTurns = new Queue<BattleTurn>(bonusTurnsData.OrderByDescending(t => t.initiative).Select(t => t.turn));

        return (turns, bonusTurns);
    }
}

public static class BattleEvents
{
    public static UnityAction BattleStarted;
    public static UnityAction<VictoryState> BattleEnded;

    public static UnityAction<UnitDefeatedData> UnitDefeated;

    public static UnityAction<BattleRound> RoundStarted;
    public static UnityAction<BattleRound> RoundEnded;

    public static UnityAction<StatChangedData> OnStatChanged;

    public static UnityAction<BattleTurn> TurnStarted;
    public static UnityAction TurnEnded;

    public static UnityAction OnPlayerTurnStarted;
    public static UnityAction OnPlayerTurnEnded;
}

public struct UnitDefeatedData
{
    public BattleUnit DefeatedUnit;
    public BattleUnit AttackerUnit;
}

public struct StatChangedData
{
    public BattleUnit Unit;
    public Stat Stat;
}

public enum VictoryState
{
    Player,
    Enemy,
    Tie,
    None
}