using UnityEngine;

namespace Interaction
{
    public class InteractionArea : MonoBehaviour
    {
        [SerializeField] private InteractionComponent interactionComponent;
        [SerializeField] private InteractionIndicator interactionIndicator;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerInteractionComponent component)) return;
        
            interactionComponent.OnEnterInteractionRange(component);
            Activate();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out PlayerInteractionComponent component)) return;
        
            interactionComponent.OnExitInteractionRange(component);
            Deactivate();
        }

        public void Activate()
        {
            interactionIndicator.Activate();
        }

        public void Deactivate()
        {
            interactionIndicator.Deactivate();
        }
    }
}