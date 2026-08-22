using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterPortrait
{
    public string key;
    public Sprite sprite;
}

public class DialogueBoxView : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private Image speakerImage;
    [SerializeField] private List<CharacterPortrait> portraits;
    
    [SerializeField] private Animator endLineIndicator;

    private DialogueBoxStyle _currentStyle;
    private CancellationTokenSource _typingCts;

    public bool IsTyping { get; private set; }

    public void Show()
    {
        gameObject.SetActive(true);
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

        var portrait = portraits.Find(p => p.key == line.SpriteKey);
        speakerImage.gameObject.SetActive(portrait != null);
        speakerImage.sprite = portrait?.sprite;

        dialogueText.text = line.Text;
        dialogueText.maxVisibleCharacters = 0;
        IsTyping = true;

        try
        {
            await TypeText(line.Text.Length, _typingCts.Token);
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
            shown = Mathf.Min(Mathf.FloorToInt(elapsed * _currentStyle.CharactersPerSecond), totalChars);
            dialogueText.maxVisibleCharacters = shown;
        }
    }

    public void SkipTyping()
    {
        _typingCts?.Cancel();
    }
}