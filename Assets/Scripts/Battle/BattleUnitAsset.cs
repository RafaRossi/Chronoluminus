using UnityEngine;

[CreateAssetMenu(menuName = "Battle/Unit", fileName =  "New Unit Asset")]
public class BattleUnitAsset : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private Sprite unitIcon;
    
    [SerializeField] private UnitStats baseStats;
    
    [SerializeField] private InitiativeCalculatorStrategy initiativeCalculatorStrategy;

    public UnitData GenerateUnitData(BattleUnit unit)
    {
        var unitStats = new UnitStats(unit, baseStats.Vigor.Value, baseStats.Power.Value, baseStats.Intelligence.Value, baseStats.Speed.Value);
        return new UnitData(displayName, unitIcon, unitStats, initiativeCalculatorStrategy.GenerateInitiativeCalculator(unitStats));
    }
}