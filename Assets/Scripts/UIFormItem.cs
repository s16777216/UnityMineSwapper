using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFormItem : MonoBehaviour
{
    [SerializeField]
    private string label = "";
    [SerializeField]
    private string value = "";

    [SerializeField]
    private TMP_Text labelComponent;
    [SerializeField]
    private TMP_Text valueComponent;

    private void Awake()
    {
        Util.TryGetChildComponentByName(this, "Label", out labelComponent);
        Util.TryGetChildComponentByName(this, "Value", out valueComponent);

        SetLabel(label);
        SetValue(value);
    }

    public void SetLabel(string label)
    {
        labelComponent.text = label;
        this.label = label;
    }

    public void SetValue(string value)
    {
        valueComponent.text = value;
        this.value = value;
    }
}
