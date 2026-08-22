using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DialogueSystem;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class ChoiceView : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Selectable defaultSelectedObject;
        
        [SerializeField] private TMP_Text promptText;
        [SerializeField] private ChoiceButton upButton, rightButton, downButton, leftButton;
        
        private TaskCompletionSource<int> _tcs;
        
        public Task<int> ShowChoices(List<Choice> choices)
        {
            _tcs = new TaskCompletionSource<int>();
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
            
            return _tcs.Task;
        }
        
        private void Select(int choice)
        {
            root.SetActive(false);
            _tcs?.TrySetResult(choice);
        }
    }
}