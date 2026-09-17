using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item/Standard Item", fileName = "Standard Item")]
public class StandardItemsAssets : ItemsAsset
{
    [field:SerializeField] public string ItemExamineDescription { get; private set; } = "";
    
    public override Item GenerateItem(ItemsAsset itemAsset)
    {
        return new StandardItem(itemAsset);
    }
    
    public class StandardItem : Item
    {
        public StandardItem(ItemsAsset itemsAsset) : base(itemsAsset.UseActionAsset.Generate(), itemsAsset.ExamineActionAsset.Generate(), itemsAsset.CombineActionAsset.Generate(), itemsAsset.CancelActionAsset.Generate()) { }
    }
}