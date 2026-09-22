using System;
using System.Threading.Tasks;
using Commands;
using UnityEngine;

namespace Interaction
{
    public class OpenDialogueOnInteract : MonoBehaviour
    {
        [SerializeField] private string knotName;
        [SerializeField] private InteractionComponent interactionComponent;

        private void Start()
        {
            interactionComponent.RegisterInteractionEvent(new OpenDialogueOnInteractCommand(knotName));
        }

        private class OpenDialogueOnInteractCommand : IInteractionCommand
        {
            private readonly string _knotName;

            public OpenDialogueOnInteractCommand(string knotName)
            {
                _knotName = knotName;
            }
            
            public Task Execute(ICommandContext context)
            {
                GameManager.Instance.PushState(new Dialogue(_knotName));
                
                return Task.CompletedTask;
            }
        }
    }
}
