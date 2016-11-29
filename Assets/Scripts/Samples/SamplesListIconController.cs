using UnityEngine;
using System.Collections;

public class SamplesListIconController : SampleUIIconController
{
    [SerializeField] UnityEngine.UI.Text displayName;
    public System.Action<int> onSelectCb = delegate {};

    protected override void UpdateUI()
    {
        base.UpdateUI();
        this.displayName.text = sample.Name;        
    }

    public override void OnSelect()
    {
        onSelectCb(this.sampleId);
    }
}
