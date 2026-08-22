using System.Threading.Tasks;
using UnityEngine;

public class DialogueEffects : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvas;

    public void Shake(float intensity)
    {
        // seu efeito de shake existente
    }

    public async Task Fade(string direction)
    {
        float from = direction == "in" ? 1f : 0f;
        float to = direction == "in" ? 0f : 1f;

        await Task.CompletedTask;
    }
}