using UnityEngine;
using UnityEngine.Playables;

public class BattleCutsceneController : MonoBehaviour
{
    [SerializeField] private CutsceneController cutsceneController;
    [Space]
    [SerializeField] private PlayableAsset startBattlePlayableAsset;
    [Space]
    [SerializeField] private PlayableAsset playerTurnPlayableAsset;
    [SerializeField] private PlayableAsset opponentTurnPlayableAsset;

    private void OnEnable()
    {
        BattleEvents.BattleStarted += StartBattleCutscene;
    }
    
    private void OnDisable()
    {
        BattleEvents.BattleStarted -= StartBattleCutscene;
    }

    private void StartBattleCutscene()
    {
        cutsceneController.SetNextCutsceneAsset(startBattlePlayableAsset);
        cutsceneController.StartOrResumeCutscene();
    }

    private void StartPlayerTurnCutscene()
    {
        cutsceneController.SetNextCutsceneAsset(playerTurnPlayableAsset);
        cutsceneController.StartOrResumeCutscene();
    }

    private void StartOpponentTurnCutscene()
    {
        cutsceneController.SetNextCutsceneAsset(opponentTurnPlayableAsset);
        cutsceneController.StartOrResumeCutscene();
    }
}
