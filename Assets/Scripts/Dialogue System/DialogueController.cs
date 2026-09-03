using System.Collections.Generic;
using System.Threading.Tasks;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    public class DialogueController : MonoBehaviour
    {
        public static DialogueCommandsQueue CommandsQueue { get; } = new();
        
        [SerializeField] private TextAsset inkJson;
        [SerializeField] private DialogueBoxView boxView;
        [SerializeField] private ChoiceView choiceView;
        [SerializeField] private DialogueEffects effects;
        
        [SerializeField] private DialogueBoxStyle defaultStyle;

        [SerializeField] private SpeakerDatabase allSpeakers;

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
            CommandsQueue.Initialize(new DialogueCommandsContext(this));
        }

        public async void StartDialogueAt(string knotName)
        {
            PlayerInputLock.Lock("Dialogue");
            
            boxView.ApplyStyle(defaultStyle);
            boxView.Show();
            
            Story.ChoosePathString(knotName);
            await ContinueStory();
        }

        public void EndDialogue()
        {
            PlayerInputLock.Unlock("Dialogue");
            
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
            
            PlayerInputLock.Unlock("Dialogue");
            CommandsQueue.Enqueue(new CloseDialogueCommand());
        }

        public SpeakerData GetSpeakerData(string speakerID)
        {
            var speakers = allSpeakers.GetSpeakers();

            return string.IsNullOrEmpty(speakerID) ? null : speakers.GetValueOrDefault(speakerID);
        }
    }
}