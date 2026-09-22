using UnityEngine;

public abstract class ItemsAsset : ScriptableObject
{
    [field: SerializeField] public int MaxStack { get; private set; } = 1;
    
    [field:SerializeField] public string ItemName { get; private set; } = "";
    
    [field:SerializeField] public Sprite Icon { get; private set; }

    [field:SerializeField] public ItemActionAsset UseActionAsset { get; private set; }
    [field:SerializeField] public ItemActionAsset CombineActionAsset { get; private set; }
    [field:SerializeField] public ItemActionAsset ExamineActionAsset { get; private set; }
    
    [field:SerializeField] public ItemActionAsset CancelActionAsset { get; private set; }
    
    public abstract Item GenerateItem();
}

public abstract class Item
{
    protected Item(ItemsAsset itemAsset)
    {
        MaxStack = itemAsset.MaxStack;
        ItemName = itemAsset.ItemName;
        Icon = itemAsset.Icon;
        
        Use = itemAsset.UseActionAsset.Generate();
        Examine = itemAsset.ExamineActionAsset.Generate();
        Combine = itemAsset.CombineActionAsset.Generate();
        Cancel = itemAsset.CancelActionAsset.Generate();
    }

    public int MaxStack { get; }
    public string ItemName { get; }
    public Sprite Icon { get; }
    
    public IItemAction Use { get; }
    public IItemAction Examine { get; }
    public IItemAction Combine { get; }
    public IItemAction Cancel { get; }
}