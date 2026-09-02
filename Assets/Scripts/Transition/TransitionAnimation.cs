using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private readonly int _inTrigger = Animator.StringToHash("In");
    private readonly int _outTrigger = Animator.StringToHash("Out");
    
    public void FadeIn()
    {
        animator.SetTrigger(_inTrigger);
    }

    public void FadeOut()
    {
        animator.SetTrigger(_outTrigger);
    }
}
