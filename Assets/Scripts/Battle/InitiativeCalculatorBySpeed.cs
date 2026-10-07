using UnityEngine;

[CreateAssetMenu(fileName = "Initiative Calculator By Speed", menuName = "Battle/Initiative Calculator By Speed")]
public class InitiativeCalculatorBySpeed : InitiativeCalculatorStrategy
{
    [SerializeField] private int multiplier = 1;
    
    public override IInitiativeCalculator GenerateInitiativeCalculator(UnitStats unitStats)
    {
        return new InitiativeBySpeed(unitStats.Speed.Value, multiplier);
    }
    
    private readonly struct InitiativeBySpeed : IInitiativeCalculator
    {
        private readonly int _speedValue;
        private readonly int _multiplier;

        public InitiativeBySpeed(int initiativeValue, int initiativeMultiplier)
        {
            _speedValue = initiativeValue;
            _multiplier = initiativeMultiplier;
        }
        
        public int GetInitiativeValue()
        {
            return _speedValue * _multiplier;
        }
    }
}