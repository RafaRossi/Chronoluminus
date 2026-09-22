using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : Singleton<InventoryController>
{
    [field:SerializeField] public List<ItemsAsset> ItemsOnInventory { get; private set; } = new ();
    
    [SerializeField] private InputActionReference openInventoryActionReference;
    [SerializeField] private InputActionReference closeInventoryActionReference;
    
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private InventoryItemsOptionsPanel inventoryItemOptionsPanel;
    
    private void OnEnable()
    { 
        openInventoryActionReference.action.performed += OpenInventory;
        closeInventoryActionReference.action.performed += CloseInventory;
    }

    private void OnDisable()
    {
        openInventoryActionReference.action.performed -= OpenInventory;
        closeInventoryActionReference.action.performed -= CloseInventory;
    }

    private void Start()
    {
        inventoryView.UpdateInventory(ItemsOnInventory);
    }

    private void OpenInventory(InputAction.CallbackContext callbackContext)
    {
        inventoryView.ToggleInventory();
        inventoryItemOptionsPanel.Close();
    }
    
    private void CloseInventory(InputAction.CallbackContext callbackContext)
    {
        inventoryView.ToggleInventory();
        inventoryItemOptionsPanel.Close();
    }
    
    public void ShowOptionsPanel(InventoryItemButton inventoryItemButton, Item item)
    {
        inventoryItemOptionsPanel.Open(inventoryItemButton, item);
    }
}

public interface IItemAction
{
    Task Execute();
}