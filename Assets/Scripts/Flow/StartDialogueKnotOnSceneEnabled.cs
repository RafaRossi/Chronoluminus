using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class StartDialogueKnotOnSceneEnabled : MonoBehaviour
{
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private string knotName;

    private void OnEnable()
    {
        dialogueController.CommandsQueue.Enqueue(new OpenDialogueCommand(knotName));
    }
}
