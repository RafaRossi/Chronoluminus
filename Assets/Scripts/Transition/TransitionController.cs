using System.Threading.Tasks;
using UnityEngine;

public class TransitionController : Singleton<TransitionController>
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private TransitionAnimation simpleFade;
}