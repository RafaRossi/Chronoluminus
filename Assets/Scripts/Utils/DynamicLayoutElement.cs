using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[DisallowMultipleComponent]
public class DynamicLayoutElement : LayoutElement
{
    [SerializeField] private LayoutElement targetElement;

    public override float minHeight
    {
        get => targetElement != null ? targetElement.preferredHeight : base.minHeight;
        set => base.minHeight = value;
    }
}
