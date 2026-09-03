using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class TransitionController : Singleton<TransitionController>
{
    [SerializeField] private TransitionAnimation simpleFade;

    public void FadeIn(UnityAction onEndFadeIn)
    {
        simpleFade.FadeIn(onEndFadeIn);
    }

    public void FadeOut(UnityAction onEndFadeOut)
    {
        simpleFade.FadeOut(onEndFadeOut);
    }
}