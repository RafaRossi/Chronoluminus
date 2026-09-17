using System.Collections.Generic;
using System.Threading.Tasks;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DialogueSystem
{
    public class ChoiceVerticalCross : ChoiceView
    {
        [SerializeField] private TMP_Text promptText;
        [SerializeField] private ChoiceButton upButton, rightButton, downButton, leftButton;
        
        public override Task<int> ShowChoices(List<Choice> choices)
        {
            tcs = new TaskCompletionSource<int>();
            root.SetActive(true);
            
            var slots = new[] { leftButton, rightButton, upButton, downButton };
            
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < choices.Count)
                {
                    var choice = choices[i].index;
                    slots[i].Setup(choices[i].text, () => Select(choice));
                }
                else
                {
                    slots[i].Hide();
                }
            }
            
            EventSystem.current.SetSelectedGameObject(defaultSelectedObject.gameObject);
            
            return tcs.Task;
        }
    }
}