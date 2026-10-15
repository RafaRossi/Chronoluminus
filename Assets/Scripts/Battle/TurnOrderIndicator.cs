using UnityEngine;
using UnityEngine.UI;

public class TurnOrderIndicator : MonoBehaviour
{
    private static readonly int IsCurrentTurn = Animator.StringToHash("isCurrentTurn");
    [SerializeField] private Image backgroundIndicator;
    [SerializeField] private Animator animator;

    private Sprite _backgroundIndicatorSprite;
    private Sprite _backgroundIndicatorSpriteActive;

    private bool _isCurrentTurn;
    
    public void Setup(BattleTurn turn, TurnState state, BattleTurnIndicatorStyle style)
    {
        _backgroundIndicatorSprite =
            turn.Unit.Team == Team.Player ? style.PlayerTurnIndicator : style.OpponentTurnIndicator;
        
        _backgroundIndicatorSpriteActive = 
            turn.Unit.Team == Team.Player ? style.PlayerTurnIndicatorActive : style.OpponentTurnIndicatorActive;

        backgroundIndicator.sprite = _backgroundIndicatorSprite;
    }

    public void SetCurrent(bool isCurrent)
    {
        if (_isCurrentTurn != isCurrent)
        {
            animator.SetBool(IsCurrentTurn, isCurrent);
        }
        
        _isCurrentTurn = isCurrent;
        
        backgroundIndicator.sprite = isCurrent ? _backgroundIndicatorSpriteActive : _backgroundIndicatorSprite;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        
    }

    public void Hide()
    {
        
    }
}
