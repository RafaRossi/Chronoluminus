using System.Threading.Tasks;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueContext
    {
        public DialogueController Controller { get; }
        public DialogueAsset Dialogue { get; }
        
        public DialogueContext(DialogueController controller, DialogueAsset dialogue)
        {
            Controller = controller;
            Dialogue = dialogue;
        }
    }

    
    public abstract class DialogueEvent : ScriptableObject
    {
        public abstract Task Execute(DialogueContext context);
    }
}