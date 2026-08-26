using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractionComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent onInteract = new();
        private readonly List<IInteractionCommand> _interactionCommands = new();

        public void Interact()
        {
            foreach (var interactionCommand in _interactionCommands)
            {
                PlayerInteractionComponent.InteractionQueue.Enqueue(interactionCommand);
            }
            
            onInteract?.Invoke();
        }
    
        public void RegisterInteractionEvent(IInteractionCommand command)
        {
            _interactionCommands.Add(command);
        }
    
        public void RemoveInteractionEvent(IInteractionCommand command)
        {
            _interactionCommands.Remove(command);
        }

        public void OnEnterInteractionRange(PlayerInteractionComponent playerInteractionComponent)
        {
            playerInteractionComponent.SetCurrentInteraction(this);
        }

        public void OnExitInteractionRange(PlayerInteractionComponent playerInteractionComponent)
        { 
            playerInteractionComponent.ResetCurrentInteraction();
        }
    }
}
