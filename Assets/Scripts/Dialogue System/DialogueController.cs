using System.Threading.Tasks;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DialogueSystem
{
    public class DialogueController : MonoBehaviour
    {
        [SerializeField] private TextAsset inkJson;
        [SerializeField] private DialogueBoxView boxView;
        [SerializeField] private ChoiceView choiceView;
        [SerializeField] private DialogueEffects effects;
        [SerializeField] private InputActionReference advanceAction;
        
        [SerializeField] private DialogueBoxStyle defaultStyle;

        [SerializeField] private UnityEvent onStartTyping = new();
        [SerializeField] private UnityEvent onEndTyping = new();
        
        [SerializeField] private UnityEvent onAdvanceActionPerformed = new();

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
            _story = new Story(inkJson.text);
            //_story.BindExternalFunction("Shake", (float intensity) => effects.Shake(intensity));
        }

        private void OnEnable() => advanceAction.action.performed += OnAdvancePerformed;
        private void OnDisable() => advanceAction.action.performed -= OnAdvancePerformed;

        public async void StartDialogueAt(string knotName)
        {
            boxView.ApplyStyle(defaultStyle);
            boxView.Show();
            
            Story.ChoosePathString(knotName);
            await ContinueStory();
        }

        private async Task ContinueStory()
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
        }

        private async void OnAdvancePerformed(InputAction.CallbackContext ctx)
        {
            if (boxView.IsTyping)
            {
                boxView.SkipTyping();
            }
            else if (Story.canContinue)
            {
                onAdvanceActionPerformed?.Invoke();
                await ContinueStory();
            }
        }
    }
}