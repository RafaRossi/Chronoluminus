using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    private ItemsAsset _currentItemAsset;
    
    public void Initialize(ItemsAsset asset)
    {
        _currentItemAsset = asset;
        itemIcon.sprite = asset.Icon;
        
        gameObject.SetActive(true);
    }

    public void ResetItem()
    {
        _currentItemAsset = null;
        itemIcon.sprite = null;
        
        gameObject.SetActive(false);
    }
}
