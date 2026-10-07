using UnityEngine;

public abstract class InitiativeCalculatorStrategy : ScriptableObject
{
    public abstract IInitiativeCalculator GenerateInitiativeCalculator(UnitStats unitStats);
}


public interface IInitiativeCalculator
{
    int GetInitiativeValue();
}