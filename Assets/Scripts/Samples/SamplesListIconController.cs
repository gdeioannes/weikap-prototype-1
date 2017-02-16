using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SamplesListIconController : SampleUIIconController
{
    [SerializeField] Text displayName;
    [SerializeField] Toggle toggle;

    public System.Action<int> onSelectCb = delegate {};

    protected override void UpdateUI()
    {
        base.UpdateUI();
        this.displayName.text = sample.Name;        
    }

    public void Set(int sampleId, ToggleGroup toggleGroup)
    {
        if (!handlerAdded)
        {
            toggle.onValueChanged.AddListener(OnSelect);
            toggle.group = toggleGroup;
        }
        base.Set(sampleId);
    }

    public override void OnSelect()
    {
        onSelectCb(this.sampleId);
    }

    public virtual void OnSelect(bool selectedState)
    {
        if (selectedState)
        {
            OnSelect();
        }
    }
}
