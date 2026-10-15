using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    
    [SerializeField] private Animator playerHUDAnimator;
    [SerializeField] private Button endTurnButton;

    [SerializeField] private TMP_Text unitName;
    [SerializeField] private Image unitIcon;

    private void OnEnable()
    {
        endTurnButton.onClick.AddListener(battleManager.EndTurn);

        BattleEvents.OnPlayerTurnStarted += ShowPlayerHUD;
        BattleEvents.OnPlayerTurnEnded += HidePlayerHUD;
        
        BattleEvents.TurnStarted += UpdatePlayerInfo;
    }

    private void OnDisable()
    {
        endTurnButton.onClick.RemoveListener(battleManager.EndTurn);
        
        BattleEvents.OnPlayerTurnStarted -= ShowPlayerHUD;
        BattleEvents.OnPlayerTurnEnded -= HidePlayerHUD;
        
        BattleEvents.TurnStarted -= UpdatePlayerInfo;
    }

    private void ShowPlayerHUD()
    {
        playerHUDAnimator.Play("Show Player HUD");
    }
    
    private void HidePlayerHUD()
    {
        playerHUDAnimator.Play("Hide Player HUD");
    }
    
    private void UpdatePlayerInfo(BattleTurn turn)
    {
        unitName.text = turn.Unit.UnitData.UnitDisplayName;
        unitIcon.sprite = turn.Unit.UnitData.UnitIcon;
    }
}
