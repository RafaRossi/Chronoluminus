using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : Singleton<CutsceneController>
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
    
    public void SkipCutscene()
    {
        if(director.state != PlayState.Playing) return;

        director.time = director.duration;
        director.Evaluate();
        director.Stop();
    }
}
