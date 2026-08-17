using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueBoxView : MonoBehaviour
    {
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private TMP_Text speakerNameText;
        
        [SerializeField] private DialogueBoxStyle defaultStyle;
        
        private DialogueBoxStyle _currentStyle;
        private CancellationTokenSource _typingCts;
        
        public bool IsTyping { get; private set; }

        public async Task ShowLine(DialogueAsset.DialogueLine line)
        {
            _typingCts?.Cancel();
            
            _typingCts = new CancellationTokenSource();

            speakerNameText.text = line.speakerName; 
            dialogueText.text = line.text;
            dialogueText.maxVisibleCharacters = 0;
            
            IsTyping = true;

            try
            {
                await TypeText(line.text.Length, _typingCts.Token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                dialogueText.maxVisibleCharacters = line.text.Length;
                IsTyping = false;
            }
        }

        private async Task TypeText(int totalChars, CancellationToken token)
        {
            float elapsed = 0f;
            int shown = 0;

            while (shown < totalChars)
            {
                token.ThrowIfCancellationRequested();
                await Task.Yield();
                elapsed += Time.deltaTime;
                shown = Mathf.Min(Mathf.FloorToInt(elapsed * _currentStyle.CharactersPerSecond), totalChars);
                dialogueText.maxVisibleCharacters = shown;
            }
        }
        
        public void SkipTyping() => _typingCts?.Cancel();
    }
}