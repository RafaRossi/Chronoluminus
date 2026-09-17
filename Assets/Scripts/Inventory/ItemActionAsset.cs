using UnityEngine;

public abstract class ItemActionAsset : ScriptableObject
{
    public abstract IItemAction Generate();
}