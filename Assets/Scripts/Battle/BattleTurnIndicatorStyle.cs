using UnityEngine;

[CreateAssetMenu(fileName = "Turn Indicator Style",  menuName = "Battle Turns/Turn Indicator Style")]
public class BattleTurnIndicatorStyle : ScriptableObject
{
    [field:SerializeField] public Sprite PlayerTurnIndicator { get; private set; }
    [field:SerializeField] public Sprite OpponentTurnIndicator { get; private set; }
    
    [field:SerializeField] public Sprite PlayerTurnIndicatorActive { get; private set; }
    [field:SerializeField] public Sprite OpponentTurnIndicatorActive { get; private set; }
}
