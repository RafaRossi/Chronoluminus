using UnityEngine;
using UnityEngine.UI;

public class InventoryItemOption : MonoBehaviour
{
    [SerializeField] private Button button;
    private IItemAction _onOptionSelected;

    private void OnEnable()
    {
        button.onClick.AddListener(OptionSelected);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OptionSelected);
    }

    public void Initialize(IItemAction onOptionSelected)
    {
        if (onOptionSelected is NoneItemActionAsset.NoneItemAction)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);
        _onOptionSelected = onOptionSelected;
    }

    private void OptionSelected()
    {
        _onOptionSelected?.Execute();
    }
}
