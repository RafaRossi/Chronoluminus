using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static UnityAction<string> OnDialogueStarted = delegate { };
}
