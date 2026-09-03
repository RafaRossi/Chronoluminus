using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransitionAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private readonly int _inTrigger = Animator.StringToHash("In");
    private readonly int _outTrigger = Animator.StringToHash("Out");

    private UnityAction _onEndFadeIn = delegate { };
    private UnityAction _onEndFadeOut = delegate { };
    
    public void FadeIn(UnityAction onEndFadeIn)
    {
        _onEndFadeIn = onEndFadeIn;
        
        animator.SetTrigger(_inTrigger);
    }

    public void FadeOut(UnityAction onEndFadeOut)
    {
        _onEndFadeOut = onEndFadeOut;
        
        animator.SetTrigger(_outTrigger);
    }

    public void EndFadeIn()
    {
        _onEndFadeIn?.Invoke();
    }

    public void EndFadeOut()
    {
        _onEndFadeOut?.Invoke();
    }
}
