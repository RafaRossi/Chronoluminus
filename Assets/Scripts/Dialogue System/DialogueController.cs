using System.Collections.Generic;
using System.Threading.Tasks;
using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DialogueSystem
{
    public class DialogueController : Singleton<DialogueController>
    {
        public static DialogueCommandsQueue CommandsQueue { get; } = new();
        
        [SerializeField] private InkFile inkFile;
        [SerializeField] private DialogueBoxView boxView;
        [SerializeField] private ChoiceView choiceView;
        [SerializeField] private DialogueEffects effects;
        
        [SerializeField] private DialogueBoxStyle defaultStyle;

        [SerializeField] private SpeakerDatabase allSpeakers;

        [SerializeField] private UnityEvent onStartTyping = new();
        [SerializeField] private UnityEvent onEndTyping = new();
        
        [Header("Input")]
        [SerializeField] private InputActionReference advanceAction;
        [SerializeField] private UnityEvent onAdvanceActionPerformed = new();
        
        private void OnEnable() => advanceAction.action.performed += OnAdvancePerformed;
        private void OnDisable() => advanceAction.action.performed -= OnAdvancePerformed;

        public UnityAction OnDialogueEnded { get; set; }

        private Story _story;

        private Story Story
        {
            get
            {
                if (_story == null)
                {
                    _story = new Story(inkFile.storyJson);
                }
                
                return _story;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            CommandsQueue.Initialize(new DialogueCommandsContext(this));

            boxView.gameObject.SetActive(false);
            choiceView.Disable();
        }

        public async void StartDialogueAt(string knotName)
        {
            InputManager.Instance.EnableDialogue();
            
            boxView.ApplyStyle(defaultStyle);
            boxView.Show();
            
            Story.ChoosePathString(knotName);
            await ContinueStory();
        }

        public void EndDialogue()
        {
            boxView.Close();
            OnDialogueEnded?.Invoke();
            
            InputManager.Instance.EnableGameplay();
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
            else
            {
                boxView.Hide();
            
                CommandsQueue.Enqueue(new CloseDialogueCommand());
            }
        }

        public SpeakerData GetSpeakerData(string speakerID)
        {
            var speakers = allSpeakers.GetSpeakers();

            return string.IsNullOrEmpty(speakerID) ? null : speakers.GetValueOrDefault(speakerID);
        }
        
        private async void OnAdvancePerformed(InputAction.CallbackContext ctx)
        {
            if (boxView.IsTyping)
            {
                boxView.SkipTyping();
            }
            else
            {
                onAdvanceActionPerformed?.Invoke();
                await ContinueStory();
            }
        }
    }
}