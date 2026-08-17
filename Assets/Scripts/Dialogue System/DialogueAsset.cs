using UnityEngine;

namespace DialogueSystem
{
    public class DialogueAsset : ScriptableObject
    {
        [field: SerializeField] public DialogueLine[] DialogueLines { get; private set; } = { };
        [field: SerializeField] public DialogueAsset NextDialogue { get; private set; }
        
        [field: SerializeField] public DialogueEvent StartDialogueEvent { get; private set; }
        [field: SerializeField] public DialogueEvent FinishDialogueEvent { get; private set; }
        
        [System.Serializable]
        public class DialogueLine
        {
            [TextArea] public string text;
            
            public string speakerName;
            public Sprite speakerSprite;
            
            public AudioClip dialogueSfx;
        }
    }
}