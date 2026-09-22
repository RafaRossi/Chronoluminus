using System.Threading.Tasks;
using DialogueSystem;
using UnityEngine;

public class StartDialogueFromCutscene : MonoBehaviour
{
    [SerializeField] private CutsceneController cutsceneController;
    public async void StartDialogue(string knot)
    {
        //cutsceneController.PauseCutscene();
        
        await WaitForDialogueToEndAsync(knot);

        //cutsceneController.StartOrResumeCutscene();
        //GameManager.Instance.PushState(new Cutscene(cutsceneController));
    }

    private Task WaitForDialogueToEndAsync(string knot)
    {
        var tcs = new TaskCompletionSource<bool>();

        void OnDialogueEnded()
        {
            DialogueController.Instance.OnDialogueEnded -= OnDialogueEnded;
            tcs.TrySetResult(true);
        }

        DialogueController.Instance.OnDialogueEnded += OnDialogueEnded;
        GameManager.Instance.PushState(new Dialogue(knot));

        return tcs.Task;
    }
}