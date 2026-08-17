using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem
{
    public class DialogueController : MonoBehaviour
    {
        [SerializeField] private InputActionReference advanceAction;
        
        [SerializeField] private DialogueBoxView boxView;
        
        private Dialogue _currentDialogue;
        private DialogueContext _context;
        
        public async void StartDialogue(DialogueAsset dialogueAsset)
        {
            _context = new DialogueContext(this, dialogueAsset);
            _currentDialogue = new Dialogue(dialogueAsset, _context);

            await _currentDialogue.StartDialogueEvent();
            NextDialogueLine();
        }

        public async void NextDialogueLine()
        {
            if (_currentDialogue.HasNextLine)
            {
                var line = _currentDialogue.GetNextDialogueLine();
                await boxView.ShowLine(line);
            }
            else
            {
                await _currentDialogue.FinishDialogueEvent();
                
                if(_currentDialogue.DialogueAsset.NextDialogue != null)
                    StartDialogue(_currentDialogue.DialogueAsset.NextDialogue);
            }
        }

        private void OnEnable() => advanceAction.action.performed += OnAdvancePressed;
        private void OnDisable() => advanceAction.action.performed -= OnAdvancePressed;

        private void OnAdvancePressed(InputAction.CallbackContext ctx)
        {
            if (_currentDialogue == null) return;

            if (boxView.IsTyping)
                boxView.SkipTyping();
            else
                NextDialogueLine();
        }
        
        private class Dialogue
        {
            public DialogueAsset DialogueAsset { get; private set; }
            
            private DialogueContext _context;
            
            private readonly Queue<DialogueAsset.DialogueLine> _dialogueLines = new();

            private readonly DialogueEvent _startDialogueEvent;
            private readonly DialogueEvent _finishDialogueEvent;
            
            public Dialogue(DialogueAsset dialogueAsset, DialogueContext context)
            {
                DialogueAsset = dialogueAsset;

                foreach (var dialogueLine in dialogueAsset.DialogueLines)
                {
                    _dialogueLines.Enqueue(dialogueLine);
                }
                
                _startDialogueEvent = dialogueAsset.StartDialogueEvent;
                _finishDialogueEvent = dialogueAsset.FinishDialogueEvent;
            }

            public DialogueAsset.DialogueLine GetNextDialogueLine() => _dialogueLines.Dequeue();

            public async Task StartDialogueEvent() => await (_startDialogueEvent.Execute(_context) ?? Task.CompletedTask);
            public async Task FinishDialogueEvent() => await (_finishDialogueEvent.Execute(_context) ?? Task.CompletedTask);

            public bool HasNextLine => _dialogueLines.Count > 0;
        }
    }
}