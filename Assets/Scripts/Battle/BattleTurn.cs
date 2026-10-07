using UnityEngine;

public enum Team
{
    Player,
    Enemy
}

public class BattleTurn
{
    public bool IsTurnActive { get; private set; }
    
    public BattleUnit Unit { get; }

    public BattleTurn(BattleUnit unit, bool isTurnActive = true)
    {
        Unit = unit;
        IsTurnActive = isTurnActive;
    }
    
    public void DisableTurn()
    {
        IsTurnActive = false;
    }
}
