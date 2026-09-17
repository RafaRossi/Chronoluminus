using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class CallEventOnCutsceneFinished : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private List<GameActions> gameActions = new();

    private CancellationTokenSource _cts;

    private Queue<Task> _eventsQueue = new();

    private void OnEnable()
    {
        playableDirector.stopped += PlayableDirectorOnStopped;
    }
    
    private void OnDisable()
    {
        playableDirector.stopped -= PlayableDirectorOnStopped;
        _cts?.Cancel();
    }

    private void PlayableDirectorOnStopped(PlayableDirector director)
    {
        _cts = new CancellationTokenSource();
        _ = RunGameActionsAsync(_cts.Token);
    }
    
    private async Task RunGameActionsAsync(CancellationToken token)
    {
        foreach (var action in gameActions)
        {
            if (action == null) continue;
            try
            {
                await action.Execute(token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}
