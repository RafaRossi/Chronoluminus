using UnityEngine;

public enum TurnState { Done, Current, Upcoming }

public enum Team
{
    Player,
    Enemy
}

public class BattleTurn
{
    public bool IsTurnActive { get; private set; }
    
    public TurnState TurnState { get; }
    
    public BattleUnit Unit { get; }

    public BattleTurn(BattleUnit unit, bool isTurnActive = true)
    {
        Unit = unit;
        IsTurnActive = isTurnActive;
        TurnState = TurnState.Upcoming;
    }
    
    public void DisableTurn()
    {
        IsTurnActive = false;
    }
}
