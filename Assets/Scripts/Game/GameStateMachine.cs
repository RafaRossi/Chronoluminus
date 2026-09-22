using DialogueSystem;
using UnityEngine.Playables;

public class GameStateMachine : StateMachine<IGameState>
{
    
}

public interface IGameState : IState { }

public class InGame : IGameState
{
    public void Enter()
    {
        InputManager.Instance.EnableGameplay();
    }

    public void Tick()
    {
    }

    public void Exit()
    {
    }

    public void Pause()
    {
    }

    public void Resume()
    {
        InputManager.Instance.EnableGameplay();
    }
}

public class Inventory : IGameState
{
    public void Enter()
    {
        InputManager.Instance.EnableInventory();
    }

    public void Tick()
    {
    }

    public void Exit()
    {
    }

    public void Pause()
    {
    }

    public void Resume()
    {
        InputManager.Instance.EnableInventory();
    }
}

public class Dialogue : IGameState
{
    private readonly string _knot;
    public Dialogue(string knot)
    {
        _knot = knot;
    }
    
    public void Enter()
    {
        InputManager.Instance.EnableDialogue();
        DialogueController.CommandsQueue.Enqueue(new OpenDialogueCommand(_knot));
    }

    public void Tick()
    {
    }

    public void Exit()
    {
        DialogueController.CommandsQueue.Enqueue(new CloseDialogueCommand());
    }

    public void Pause()
    {
        
    }

    public void Resume()
    {
        
    }
}

public class Cutscene : IGameState
{
    private readonly CutsceneController _cutsceneController;
    
    public Cutscene(CutsceneController cutsceneController)
    {
        _cutsceneController = cutsceneController;
    }
    
    public void Enter()
    {
        InputManager.Instance.EnableCutscene();
        _cutsceneController.StartOrResumeCutscene();
    }

    public void Tick()
    {
    }

    public void Exit()
    {
    }

    public void Pause()
    {
        _cutsceneController.PauseCutscene();
    }

    public void Resume()
    {
        InputManager.Instance.EnableCutscene();
        _cutsceneController.StartOrResumeCutscene();
    }
}