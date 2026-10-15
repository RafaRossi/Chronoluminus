using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderIndicatorController : MonoBehaviour
{
    [SerializeField] private TurnOrderIndicator turnOrderIndicatorPrefab;
    [SerializeField] private RectTransform turnOrderIndicatorContainer;
    
    [SerializeField] private List<TurnOrderIndicator> turnOrderIndicators = new ();
    [SerializeField] private BattleTurnIndicatorStyle indicatorStyle;
    
    private BattleRound _currentRound;
    
    private readonly Dictionary<BattleTurn, TurnOrderIndicator> _currentTurnOrderIndicators = new();

    private void OnEnable()
    {
        BattleEvents.RoundStarted += RoundStarted;
        
        BattleEvents.OnTurnsUpdate += OnTurnsUpdate;
        BattleEvents.OnTurnDisabled += RemoveTurnIndicator;

        BattleEvents.TurnStarted += SetCurrentTurnIndicator;
        BattleEvents.TurnEnded += DisableTurnIndicator;
    }

    private void OnDisable()
    {
        BattleEvents.RoundStarted -= RoundStarted;
        
        BattleEvents.OnTurnsUpdate -= OnTurnsUpdate;
        BattleEvents.OnTurnDisabled -= RemoveTurnIndicator;
        
        BattleEvents.TurnStarted -= SetCurrentTurnIndicator;
        BattleEvents.TurnEnded -= DisableTurnIndicator;
    }

    private void RoundStarted(BattleRound round)
    {
        _currentRound = round; 
        Refresh();
    }
    
    private void OnTurnsUpdate()
    {
        Refresh();
    }

    private void InstantiateTurnOrderIndicator(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            turnOrderIndicators.Add(Instantiate(turnOrderIndicatorPrefab, turnOrderIndicatorContainer));
        }
    }

    private void Refresh()
    {
        if (_currentRound == null) return;

        var turns = _currentRound.Turns;
        var currentIndex = _currentRound.CurrentIndex;

        if (turns.Count > turnOrderIndicators.Count)
            InstantiateTurnOrderIndicator(turns.Count - turnOrderIndicators.Count);

        _currentTurnOrderIndicators.Clear();

        var slot = 0;
        for (var i = 0; i < turns.Count; i++)
        {
            var turn = turns[i];
            if (!turn.IsTurnActive) continue;

            var state = i < currentIndex ? TurnState.Done
                : i == currentIndex ? TurnState.Current
                : TurnState.Upcoming;

            var indicator = turnOrderIndicators[slot++];
            
            indicator.Setup(turn, state, indicatorStyle);
            indicator.Activate();
            
            _currentTurnOrderIndicators[turn] = indicator;
        }
        for (; slot < turnOrderIndicators.Count; slot++)
            turnOrderIndicators[slot].Deactivate();
    }

    private void RemoveTurnIndicator(BattleTurn battleTurn)
    {
        if (!_currentTurnOrderIndicators.Remove(battleTurn, out var indicator)) return;
        indicator.Hide();
    }

    private void SetCurrentTurnIndicator(BattleTurn battleTurn)
    {
        _currentTurnOrderIndicators[battleTurn].SetCurrent(battleTurn == _currentRound.CurrentTurn);
    }

    private void DisableTurnIndicator()
    {
        _currentTurnOrderIndicators[_currentRound.CurrentTurn].SetCurrent(false);
    }
}
