using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DialogueSystem;
using UnityEngine;

public class DialogueCommandsQueue : CommandsQueue<IDialogueCommand, DialogueCommandsContext>
{
    
}

public interface IDialogueCommand : ICommand<DialogueCommandsContext> { }

public class DialogueCommandsContext : ICommandContext
{
    public readonly DialogueController controller;

    public DialogueCommandsContext(DialogueController controller)
    {
        this.controller = controller;
    }
}

public class OpenDialogueCommand : IDialogueCommand
{
    private readonly string _dialogueStartKnot;

    public OpenDialogueCommand(string dialogueStartKnot)
    {
        _dialogueStartKnot = dialogueStartKnot;
    }
    
    public Task Execute(DialogueCommandsContext context)
    {
        context.controller.StartDialogueAt(_dialogueStartKnot);
        return Task.CompletedTask;
    }
}

public class CloseDialogueCommand : IDialogueCommand
{
    public Task Execute(DialogueCommandsContext context)
    {
        context.controller.EndDialogue();
        
        return Task.CompletedTask;
    }
}
