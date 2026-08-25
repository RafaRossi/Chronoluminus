using System;
using System.Threading.Tasks;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    public class DialogueController : MonoBehaviour
    {
        [SerializeField] private DialogueCommandsQueue commandsQueue;
        public DialogueCommandsQueue CommandsQueue => commandsQueue;
        
        [SerializeField] private TextAsset inkJson;
        [SerializeField] private DialogueBoxView boxView;
        [SerializeField] private ChoiceView choiceView;
        [SerializeField] private DialogueEffects effects;
        
        [SerializeField] private DialogueBoxStyle defaultStyle;

        [SerializeField] private UnityEvent onStartTyping = new();
        [SerializeField] private UnityEvent onEndTyping = new();

        private Story _story;

        private Story Story
        {
            get
            {
                if (_story == null)
                {
                    _story = new Story(inkJson.text);
                }
                
                return _story;
            }
        }

        private void Awake()
        {
            commandsQueue.Initialize(new DialogueCommandsContext(this));
        }

        public async void StartDialogueAt(string knotName)
        {
            CameraInputLock.Lock("Dialogue");
            
            boxView.ApplyStyle(defaultStyle);
            boxView.Show();
            
            Story.ChoosePathString(knotName);
            await ContinueStory();
        }

        public void EndDialogue()
        {
            CameraInputLock.Unlock("Dialogue");
            
            boxView.Hide();
        }

        public async Task ContinueStory()
        {
            while (Story.canContinue)
            {
                string text = Story.Continue().Trim();
                
                var tags = Story.currentTags;

                foreach (var tag in tags)
                {
                    if (tag.StartsWith("fade:"))
                        await effects.Fade(tag.Substring(5));
                }

                if (!string.IsNullOrEmpty(text))
                {
                    var line = DialogueLineParser.Parse(text, tags);
                    await boxView.ShowLine(line);
                    return;
                }
            }

            if (Story.currentChoices.Count > 0)
            {
                int index = await choiceView.ShowChoices(Story.currentChoices);
                Story.ChooseChoiceIndex(index);
                await ContinueStory();
            }
            
            CameraInputLock.Unlock("Dialogue");
            CommandsQueue.Enqueue(new CloseDialogueCommand());
        }
    }
}