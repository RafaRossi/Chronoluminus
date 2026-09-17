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
    
    public abstract Item GenerateItem(ItemsAsset itemAsset);
}

public abstract class Item
{
    protected Item(IItemAction useAction, IItemAction examine, IItemAction combineAction, IItemAction cancelAction)
    {
        Use = useAction;
        Examine = examine;
        Combine = combineAction;
        Cancel = cancelAction;
    }
    
    public IItemAction Use { get; }
    public IItemAction Examine { get; }
    public IItemAction Combine { get; }
    public IItemAction Cancel { get; }
}