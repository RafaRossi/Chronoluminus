using Unity.Cinemachine;
using UnityEngine;

public class BattleCameraDirector : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cameraBrain;
    [Space]
    [SerializeField] private CinemachineCamera battleCamera;
    [SerializeField] private CinemachineCamera focusCamera;
    [Space]
    [SerializeField] private CinemachineStateDrivenCamera drivenCamera;

    private void OnEnable()
    {
        BattleEvents.BattleStarted += BattleStart;
        
        BattleEvents.TurnStarted += TurnStarted;
        BattleEvents.TurnEnded += BattleView;
    }

    private void OnDisable()
    {
        BattleEvents.BattleStarted -= BattleStart;
        
        BattleEvents.TurnStarted -= TurnStarted;
        BattleEvents.TurnEnded -= BattleView;
    }

    public void FocusUnit(BattleUnit unit)
    {
        focusCamera.Follow = unit.BattleUnitView.GetTargetCameraPoint();
        drivenCamera.AnimatedTarget.Play("Focus Camera");
    }

    public void BattleView()
    {
        drivenCamera.AnimatedTarget.Play("Battle View");
    }
    
    private void TurnStarted(BattleTurn battleTurn)
    {
        FocusUnit(battleTurn.Unit);
    }
    
    private void BattleStart()
    {
        BattleView();
    }
}