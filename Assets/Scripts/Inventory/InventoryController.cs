using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [field:SerializeField] public List<ItemsAsset> ItemsOnInventory { get; private set; } = new ();
    [SerializeField] private InputActionReference openInventoryActionReference;
    
    [SerializeField] private InventoryView inventoryView;
    
    private void OnEnable() => openInventoryActionReference.action.performed += ToggleInventory;
    private void OnDisable() => openInventoryActionReference.action.performed -= ToggleInventory;

    private void Start()
    {
        inventoryView.UpdateInventory(ItemsOnInventory);
    }

    private void ToggleInventory(InputAction.CallbackContext callbackContext)
    {
        inventoryView.ToggleInventory();
    }
}

public interface IItemAction
{
    Task Execute();
}