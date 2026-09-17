using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [field:SerializeField] public List<ItemsAsset> ItemsOnInventory { get; private set; } = new ();
    [SerializeField] private InventoryView inventoryView;

    private void Start()
    {
        inventoryView.UpdateInventory(ItemsOnInventory);
    }
}

public interface IItemAction
{
    Task Execute();
}