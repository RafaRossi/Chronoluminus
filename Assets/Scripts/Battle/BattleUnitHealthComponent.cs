using System;
using UnityEngine;

public class BattleUnitHealthComponent : MonoBehaviour
{
    public UnitHealth UnitHealth { get; private set; }

    private void OnEnable()
    {
        BattleEvents.OnStatChanged += OnStatChanged;
    }

    private void OnDisable()
    {
        BattleEvents.OnStatChanged -= OnStatChanged;
    }

    public void Initialize(UnitData unitData)
    {
        UnitHealth = new UnitHealth(unitData.CurrentVigor);
    }

    public void ReceiveDamage(int damage)
    {
        
    }

    private void OnStatChanged(StatChangedData data)
    {
        if (data.Stat == UnitHealth.Vigor)
        {
            UnitHealth.UpdateCurrentHealth();
        }
    }
}

public class UnitHealth
{
    public int CurrentValue { get; private set; }
    public int MaxValue { get; private set; }
    
    public Stat Vigor { get; private set; }

    public UnitHealth(Stat vigor)
    {
        var health = (int) MathF.Max(1, vigor.Value * 4);

        CurrentValue = health;
        MaxValue = health;

        Vigor = vigor;
    }

    public void UpdateCurrentHealth()
    {
        int previousMaxHealth = MaxValue;
        
        var maxHealth = (int) MathF.Max(1, Vigor.Value * 4);

        MaxValue = maxHealth;

        CurrentValue += (maxHealth - previousMaxHealth);
    }
}