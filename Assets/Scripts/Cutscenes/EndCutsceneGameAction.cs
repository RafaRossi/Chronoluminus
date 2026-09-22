using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EndCutsceneGameAction : GameActions
{
    [SerializeField] private CutsceneController cutsceneController;
    
    public override Task Execute(CancellationToken token)
    {
        cutsceneController.EndCutscene();
        return Task.CompletedTask;
    }
}
