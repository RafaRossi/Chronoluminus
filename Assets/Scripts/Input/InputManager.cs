using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public enum GameplayContext { Gameplay, Dialogue, Camera }
    
    [SerializeField] private PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();
        
        
    }

    public void EnableGameplay()
    {
        playerInput.SwitchCurrentActionMap("Gameplay");
    }

    public void EnableDialogue()
    {
        playerInput.SwitchCurrentActionMap("Dialogue");
    }

    public void EnableUI()
    {
        playerInput.SwitchCurrentActionMap("UI");
    }
}
