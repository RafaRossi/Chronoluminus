using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "None Item Action", menuName = "Inventory/Item Action/None Item Action")]
public class NoneItemActionAsset : ItemActionAsset
{
    public class NoneItemAction : IItemAction
    {
        public Task Execute()
        {
            return Task.CompletedTask;
        }
    }

    public override IItemAction Generate()
    {
        return new NoneItemAction();
    }
}