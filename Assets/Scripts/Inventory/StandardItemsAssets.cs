using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item/Standard Item", fileName = "Standard Item")]
public class StandardItemsAssets : ItemsAsset
{
    [field:SerializeField] public string ItemExamineDescription { get; private set; } = "";
    
    public override Item GenerateItem()
    {
        return new StandardItem(this);
    }
    
    public class StandardItem : Item
    {
        public string ItemExamineDescription { get; }

        public StandardItem(StandardItemsAssets itemsAsset) : base(itemsAsset)
        {
            ItemExamineDescription = itemsAsset.ItemExamineDescription;
        }
    }
}