using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private PlayerInput playerInput;

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
