using UnityEngine;
using UnityEngine.Playables;

public class BattleCutsceneController : MonoBehaviour
{
    [SerializeField] private CutsceneController cutsceneController;
    [Space]
    [SerializeField] private PlayableAsset startBattlePlayableAsset;

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
}
