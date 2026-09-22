using System;
using UnityEngine;

public class InventoryItemsOptionsPanel : MonoBehaviour
{
    [SerializeField] private RectTransform root;
    [SerializeField] private CanvasGroup canvasGroup;
    
    [SerializeField] private InventoryItemOption useOptionButton;
    [SerializeField] private InventoryItemOption examineOptionButton;
    [SerializeField] private InventoryItemOption combineOptionButton;

    private void Awake()
    {
        Close();
    }

    public void Close()
    {
        canvasGroup.interactable = false;
        gameObject.SetActive(false);
    }

    public void Open(InventoryItemButton parentButton, Item item)
    {
        canvasGroup.interactable = true;
        
        useOptionButton.Initialize(item.Use);
        examineOptionButton.Initialize(item.Examine);
        combineOptionButton.Initialize(item.Combine);
        
        root.SetParent(parentButton.transform, true);
        root.anchoredPosition = new Vector2(0, 200);
        
        gameObject.SetActive(true);
    }
}
