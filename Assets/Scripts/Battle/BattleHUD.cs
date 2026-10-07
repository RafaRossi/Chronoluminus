using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    
    [SerializeField] private Animator playerHUDAnimator;
    [SerializeField] private Button endTurnButton;

    private void OnEnable()
    {
        endTurnButton.onClick.AddListener(battleManager.EndTurn);

        BattleEvents.OnPlayerTurnStarted += ShowPlayerHUD;
        BattleEvents.OnPlayerTurnEnded += HidePlayerHUD;
    }

    private void OnDisable()
    {
        endTurnButton.onClick.RemoveListener(battleManager.EndTurn);
        
        BattleEvents.OnPlayerTurnStarted -= ShowPlayerHUD;
        BattleEvents.OnPlayerTurnEnded -= HidePlayerHUD;
    }

    private void ShowPlayerHUD()
    {
        playerHUDAnimator.Play("Show Player HUD");
    }
    
    private void HidePlayerHUD()
    {
        playerHUDAnimator.Play("Hide Player HUD");
    }
}
