using System.Collections.Generic;
using System.Threading.Tasks;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DialogueSystem
{
    public class ChoiceViewVertical : ChoiceView
    {
        [SerializeField] private List<ChoiceButton> choiceButtons;
        [SerializeField] private ChoiceButton choiceButtonPrefab;
        
        [SerializeField] private RectTransform choiceButtonContainer;
        
        public override Task<int> ShowChoices(List<Choice> choices)
        {
            tcs = new TaskCompletionSource<int>();
            Enable();
            
            if (choices.Count > choiceButtons.Count)
            {
                var choiceButton = Instantiate(choiceButtonPrefab, choiceButtonContainer);
                choiceButtons.Add(choiceButton);
            }
            else
            {
                var diff = choiceButtons.Count - choices.Count;

                for (var i = 0; i < diff; i++)
                {
                    choiceButtons[^(i+1)].Hide();
                }
            }

            for (var i = 0; i < choices.Count; i++)
            {
                var index = choices[i].index;
                choiceButtons[i].Setup(choices[i].text, () => Select(index));
            }
            
            EventSystem.current.SetSelectedGameObject(choiceButtons[0].gameObject);
            return tcs.Task;
        }
    }
}

