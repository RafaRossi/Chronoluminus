using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button itemButton;
    
    private Item _currentItem;

    private void OnEnable()
    {
        itemButton.onClick.AddListener(RequestOpenItemsPanel);
    }

    private void OnDisable()
    {
        itemButton.onClick.RemoveListener(RequestOpenItemsPanel);
    }

    public void Initialize(ItemsAsset asset)
    {
        _currentItem = asset.GenerateItem();
        itemIcon.sprite = asset.Icon;
        
        gameObject.SetActive(true);
    }

    public void ResetItem()
    {
        _currentItem = null;
        itemIcon.sprite = null;
        
        gameObject.SetActive(false);
    }

    private void RequestOpenItemsPanel()
    {
        InventoryController.Instance.ShowOptionsPanel(this, _currentItem);
    }
}
