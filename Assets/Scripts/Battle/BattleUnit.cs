using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BattleUnit : MonoBehaviour
{
    [field:SerializeField] public BattleUnitView BattleUnitView { get; private set; }
    [field:SerializeField] public BattleUnitHealthComponent UnitHealthComponent { get; private set; }
    
    public Team Team { get; private set; }

    public bool IsActive { get; private set; } = false;
    public bool IsAlive { get; private set; } = false;
    
    public UnitData UnitData { get; private set; }

    private void Awake()
    {
        Deactivate();
    }

    public void Deactivate()
    {
        IsAlive = false;
        IsActive = false;

        UpdateState();
    }
    
    public void UpdateState()
    {
        BattleUnitView.gameObject.SetActive(IsActive);
        gameObject.SetActive(IsActive);
    }

    public void InitializeUnit(BattleUnitAsset battleUnitAsset, Team team, bool initialAliveState = true)
    {
        UnitData = battleUnitAsset.GenerateUnitData(this);
        UnitHealthComponent.Initialize(UnitData);

        Team = team;

        IsAlive = initialAliveState;
        IsActive = true;
    }
    
    public int GetInitiativeValue()
    {
        return UnitData.InitiativeCalculator.GetInitiativeValue();
    }

    public void PerformAttack()
    {
        
    }

    public void StartUnitTurn()
    {
        
    }
}

[Serializable]
public class UnitData
{
    public UnitData(string unitDisplayName, UnitStats unitBaseStats, IInitiativeCalculator initiativeCalculator)
    {
        UnitDisplayName = unitDisplayName;
        
        UnitBaseStats = unitBaseStats;
        InitiativeCalculator = initiativeCalculator;
    }
    
    public string UnitDisplayName { get; private set; }
    public UnitStats UnitBaseStats { get; private set; }

    public Stat CurrentVigor => UnitBaseStats.Vigor;
    public Stat CurrentPower => UnitBaseStats.Power;
    public Stat CurrentIntelligence => UnitBaseStats.Intelligence;
    public Stat CurrentSpeed => UnitBaseStats.Speed;
    
    public IInitiativeCalculator InitiativeCalculator { get; private set; }
}

[Serializable]
public class UnitStats
{
    public BattleUnit Unit { get; private set; }
    public Stat Vigor { get; private set; }
    public Stat Power { get; private set; }
    public Stat Intelligence { get; private set; }
    public Stat Speed { get; private set; }

    public UnitStats(BattleUnit unit, int vigor, int power, int intelligence, int speed)
    {
        Unit = unit;
        
        Vigor = new Stat(vigor, OnUnitStatChanged);
        Power = new Stat(power, OnUnitStatChanged);
        Intelligence = new Stat(intelligence, OnUnitStatChanged);
        Speed = new Stat(speed, OnUnitStatChanged);
    }

    private void OnUnitStatChanged(StatChangedData data)
    {
        BattleEvents.OnStatChanged?.Invoke(new StatChangedData
        {
            Unit = Unit,
            Stat = data.Stat,
        });
    }
}


[Serializable]
public class Stat : IStat
{
    public int BaseValue { get; }
    public int Value { get; private set; }
    
    private readonly List<StatModifier> _modifiers = new();
    
    private UnityAction<StatChangedData> _onStatValueChanged;
    
    public Stat(int baseValue, UnityAction<StatChangedData> onStatValueChanged)
    {
        BaseValue = baseValue;
        Value = Calculate();
        
        _onStatValueChanged = onStatValueChanged;
    }
    
    public void AddModifier(StatModifier mod)
    {
        _modifiers.Add(mod);
        
        Recalculate();
    }

    public void RemoveModifier(StatModifier mod)
    {
        _modifiers.Remove(mod);
        
        Recalculate();
    }

    public void RemoveAllFromSource(object source)
    {
        _modifiers.RemoveAll(m => m.source == source);
        
        Recalculate();
    }

    public void TickTurn()
    {
        foreach (var m in _modifiers.Where(m => m.turnsLeft > 0)) m.turnsLeft--;
        
        _modifiers.RemoveAll(m => m.turnsLeft == 0);
        Recalculate();
    }
    
    private void Recalculate()
    {
        Value = Calculate();
        
        _onStatValueChanged?.Invoke(new StatChangedData
        {
            Stat = this
        });
    }
    
    private int Calculate()
    {
        float flat = 0, percent = 0;
        foreach (var m in _modifiers)
        {
            if (m.modifierType == ModifierType.Flat) flat += m.value;
            else percent += m.value;
        }
        return Mathf.Max(0, Mathf.RoundToInt((BaseValue + flat) * (1 + percent)));
    }
}

public enum ModifierType { Flat, Percent }

public interface IStat
{
    int BaseValue { get; }
}