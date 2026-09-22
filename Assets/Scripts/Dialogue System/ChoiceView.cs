using System.Collections.Generic;
using System.Threading.Tasks;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public abstract class ChoiceView : MonoBehaviour
    {
        [SerializeField] protected GameObject root;
        
        private Selectable _lastSelectedObject;
        
        protected TaskCompletionSource<int> tcs;

        public abstract Task<int> ShowChoices(List<Choice> choices);
        
        protected void Select(int choice)
        {
            Disable();
            tcs?.TrySetResult(choice);
        }

        public void Disable()
        {
            root.SetActive(false);
        }

        public void Enable()
        {
            root.SetActive(true);
        }
    }
}