using System;
using System.Threading;
using System.Threading.Tasks;
using DialogueSystem;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class CharacterPortrait
{
    public string key;
    public Sprite sprite;
}

public class DialogueBoxView : MonoBehaviour
{
    private static readonly int ShowDialogueBox = Animator.StringToHash("Show");
    private static readonly int HideDialogueBox = Animator.StringToHash("Hide");

    [Header("Dialogue Box")]
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private Animator dialogueBoxAnimator;
    
    [SerializeField] private TMP_Text dialogueText;
    
    [SerializeField] private Animator endLineIndicator;
    
    [Header("Speaker")]
    [SerializeField] private Animator speakerAnimator;
    [SerializeField] private TMP_Text speakerNameText;

    [SerializeField] private RectTransform speakerContainer;
    [SerializeField] private Image speakerImage;
    
    [Header("Input")]
    [SerializeField] private InputActionReference advanceAction;
    [SerializeField] private UnityEvent onAdvanceActionPerformed = new();

    private DialogueBoxStyle _currentStyle;
    private EventReference _currentTypingSound;
    
    private CancellationTokenSource _typingCts;

    public bool IsTyping { get; private set; }
    
    private void OnEnable() => advanceAction.action.performed += OnAdvancePerformed;
    private void OnDisable() => advanceAction.action.performed -= OnAdvancePerformed;


    public void Show()
    {
        gameObject.SetActive(true);
        
        dialogueBoxAnimator.Play(ShowDialogueBox);
    }

    public void Hide()
    {
        dialogueBoxAnimator.Play(HideDialogueBox);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
    

    public void ApplyStyle(DialogueBoxStyle style)
    {
        _currentStyle = style;
        dialogueText.font = _currentStyle.Font;
        dialogueText.color = _currentStyle.TextColor;
    }

    public async Task ShowLine(DisplayLine line)
    {
        _typingCts?.Cancel();
        _typingCts = new CancellationTokenSource();

        speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(line.SpeakerName));
        speakerNameText.text = line.SpeakerName;
        
        _currentTypingSound = _currentStyle.DefaultTypingSound;
        dialogueText.font = _currentStyle.Font;

        var speaker = dialogueController.GetSpeakerData(line.SpeakerKey);

        if (speaker)
        {
            speakerContainer.gameObject.SetActive(true);
            speakerImage.sprite = speaker.sprite;
            
            _currentTypingSound = speaker.typingSound;

            dialogueText.font = speaker.customFont != null ? speaker.customFont : _currentStyle.Font;
        }
        else
        {
            speakerContainer.gameObject.SetActive(false);
        }

        dialogueText.text = line.Text;
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.ForceMeshUpdate();
        IsTyping = true;

        try
        {
            await TypeText(dialogueText.textInfo.characterCount, _typingCts.Token); 
        }
        catch (OperationCanceledException) { }
        finally
        {
            dialogueText.maxVisibleCharacters = line.Text.Length;
            IsTyping = false;
            
            endLineIndicator.Play("Idle");
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

            int newShown = Mathf.Min(Mathf.FloorToInt(elapsed * _currentStyle.CharactersPerSecond), totalChars);

            if (newShown > shown)
            {
                char c = dialogueText.textInfo.characterInfo[newShown - 1].character;
                PlayTypingSound(c);
            
                shown = newShown;
                dialogueText.maxVisibleCharacters = shown;
            }
        }
    }

    private void PlayTypingSound(char c)
    {
        if (_currentTypingSound.IsNull) return;
        if (char.IsWhiteSpace(c) || char.IsPunctuation(c)) return;

        RuntimeManager.PlayOneShot(_currentTypingSound);
    }

    private void SkipTyping()
    {
        _typingCts?.Cancel();
    }
    
    private async void OnAdvancePerformed(InputAction.CallbackContext ctx)
    {
        if (IsTyping)
        {
            SkipTyping();
        }
        else
        {
            onAdvanceActionPerformed?.Invoke();
            await dialogueController.ContinueStory();
        }
    }
}