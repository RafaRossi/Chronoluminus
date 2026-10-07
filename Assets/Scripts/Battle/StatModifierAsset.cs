using UnityEngine;

[CreateAssetMenu(fileName = "Basic StatModifier", menuName = "Stat Modifiers/Basic Stat Modifier")]
public class StatModifierAsset : ScriptableObject
{
    [SerializeField] private ModifierType modifierType;
    [SerializeField] private float value;
    [SerializeField] private int turnsLeft;
    
    public virtual StatModifier GenerateModifier(object source)
    {
        return new StatModifier(modifierType, value, source, turnsLeft);
    }
}

public class StatModifier
{
    public readonly ModifierType modifierType;
    
    public readonly float value;
    public readonly object source;
    public int turnsLeft;
    
    public StatModifier(ModifierType type,  float value, object source, int turnsLeft)
    {
        modifierType = type;
        this.value = value;
        this.source = source;
        this.turnsLeft = turnsLeft;
    }
}