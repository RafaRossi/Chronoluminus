using System.Threading.Tasks;
using DialogueSystem;
using UnityEngine;

public class StartDialogueFromCutscene : MonoBehaviour
{
    public async void StartDialogue(string knot)
    {
        CutsceneController.Instance.PauseCutscene();
        
        await WaitForDialogueToEndAsync(knot);

        CutsceneController.Instance.StartOrResumeCutscene();
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
        DialogueController.Instance.StartDialogueAt(knot);

        return tcs.Task;
    }
}