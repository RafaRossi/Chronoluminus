using System.Collections;
using DialogueSystem;
using UnityEngine;

public class StartDialogueKnotOnSceneStartWithDelay : MonoBehaviour
{
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private string knotName;
    
    [SerializeField] private float delayTime = 0.1f;

    private IEnumerator Start()
    {
        dialogueController.CommandsQueue.Enqueue(new OpenDialogueCommand(knotName));
        yield return new WaitForSeconds(delayTime);
    }
}
