using System;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    public void StartOrResumeCutscene()
    {
        director.Play();
    }

    public void PauseCutscene()
    {
        director.Pause();
    }

    public void EnterCutscene()
    {
        GameManager.Instance.PushState(new Cutscene(this));
    }

    public void EndCutscene()
    {
        GameManager.Instance.PopState();
    }
    
    public void SkipCutscene()
    {
        if(director.state != PlayState.Playing) return;

        director.time = director.duration;
        director.Evaluate();
        director.Stop();
    }
}
