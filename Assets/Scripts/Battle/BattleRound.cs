using System.Collections.Generic;

public class BattleRound
{
    private readonly List<BattleTurn> _turns = new();
    private int _currentIndex = 0;

    public int RoundNumber { get; }
    public int CurrentIndex => _currentIndex;
    public IReadOnlyList<BattleTurn> Turns => _turns;

    public BattleTurn CurrentTurn =>
        _currentIndex >= 0 && _currentIndex < _turns.Count ? _turns[_currentIndex] : null;

    public BattleRound(int roundNumber, IEnumerable<BattleTurn> battleTurns, IEnumerable<BattleTurn> bonusTurns)
    {
        RoundNumber = roundNumber;
        _turns.AddRange(battleTurns);
        _turns.AddRange(bonusTurns);
    }
    
    public bool NextTurn()
    {
        while (_currentIndex < _turns.Count)
        {
            _currentIndex++;
            if (_currentIndex < _turns.Count && _turns[_currentIndex].Unit.IsAlive) return true;
        }
        return false;
    }

    public void AddAdditionalTurn(BattleTurn turn)
    {
        _turns.Add(turn);
        BattleEvents.OnTurnsUpdate?.Invoke();
    }

    public void InsertAdditionalTurn(BattleTurn turn, int index)
    {
        _turns.Insert(index, turn);
        BattleEvents.OnTurnsUpdate?.Invoke();
    }

    public void DisableTurn(BattleTurn turn)
    {
        turn.DisableTurn();
        BattleEvents.OnTurnsUpdate?.Invoke();
    }
}