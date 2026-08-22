using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text label;
    
    public bool IsActive => gameObject.activeSelf;

    public void Setup(string text, System.Action onClick)
    {
        label.text = text;
        gameObject.SetActive(true);
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick());
    }

    public void Hide() => gameObject.SetActive(false);
}
