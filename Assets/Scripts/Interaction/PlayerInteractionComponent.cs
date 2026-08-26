using Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionComponent : MonoBehaviour
{
    public static InteractionCommandsQueue InteractionQueue { get; } = new ();
    [SerializeField] private InputActionReference interactionInput;
    
    private InteractionComponent _currentInteraction;
    
    private void OnEnable() => interactionInput.action.performed += Interact;
    private void OnDisable() => interactionInput.action.performed -= Interact;

    public void SetCurrentInteraction(InteractionComponent interactionArea)
    {
        _currentInteraction = interactionArea;
    }
    
    public void ResetCurrentInteraction()
    {
        _currentInteraction = null;
    }

    private void Interact(InputAction.CallbackContext callbackContext)
    {
        if (_currentInteraction)
        {
            _currentInteraction.Interact();
            
            ResetCurrentInteraction();
        }
    }
}
