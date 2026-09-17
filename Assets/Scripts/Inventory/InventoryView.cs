using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    private static readonly int Shrink = Animator.StringToHash("Shrink");
    private static readonly int Expand = Animator.StringToHash("Expand");
    [SerializeField] private Animator animator;
    
    [SerializeField] private List<InventoryItemButton> inventoryButtons;
    [SerializeField] private InventoryItemButton inventoryButtonPrefab;
        
    [SerializeField] private RectTransform inventoryButtonContainer;

    private bool _isVisible = false;

    public void UpdateInventory(List<ItemsAsset> items)
    {
        if (items.Count > inventoryButtons.Count)
        {
            var inventoryButton = Instantiate(inventoryButtonPrefab, inventoryButtonContainer);
            inventoryButtons.Add(inventoryButton);
        }
        else
        {
            var diff = inventoryButtons.Count - items.Count;

            for (var i = 0; i < diff; i++)
            {
                inventoryButtons[^(i+1)].ResetItem();
            }
        }

        for (var i = 0; i < items.Count; i++)
        {
            inventoryButtons[i].Initialize(items[i]);
        }
    }
        
    public void ToggleInventory()
    {
        if (!_isVisible)
        {
            ShowInventory();
        }
        else
        {
            ShrinkInventory();
        }
    }

    public void ShrinkInventory()
    {
        animator.SetTrigger(Shrink);
        _isVisible = false;
    }

    public void ShowInventory()
    {
        animator.SetTrigger(Expand);
        _isVisible = true;
    }
}
