using System.Collections.Generic;
using UnityEngine;

public class BattleRound 
{
    private readonly Queue<BattleTurn> _battleTurns = new();
    
    public BattleTurn CurrentTurn { get; private set; }
    
    public int RoundNumber { get; private set; }
    
    public BattleRound(int roundNumber, Queue<BattleTurn> battleTurns, Queue<BattleTurn> bonusTurns)
    {
        RoundNumber = roundNumber;
        
        foreach (var turn in battleTurns)
        {
            _battleTurns.Enqueue(turn);
        }

        foreach (var bonusTurn in bonusTurns)
        {
            _battleTurns.Enqueue(bonusTurn);
        }
    }

    public bool NextTurn()
    {
        while (true)
        {
            if (_battleTurns.TryDequeue(out var battleTurn))
            {
                if (battleTurn.IsTurnActive)
                {
                    CurrentTurn = battleTurn;
                    return true;
                }

                continue;
            }

            break;
        }

        return false;
    }
}
