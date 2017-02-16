using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsListIconController : MonoBehaviour {

    [SerializeField] Image icon;
    [SerializeField] Image nonCollectedIcon;
    [SerializeField] Text displayName;
    [SerializeField] Text cost;
	[SerializeField] Toggle toggle;

    protected int toolId;
    protected ToolsDBScriptableObject.Tool tool;
    protected bool collectedStatus;

	private bool handlerAdded = false;

	public System.Action<int> onSelectCb = delegate {};

    public void Set(int toolId, ToggleGroup toggleGroup)
    {
		this.toolId = toolId;
		tool = GameController.Instance.ToolsDB[toolId];
		collectedStatus = PlayerData.Instance.ToolsUnlocked.Contains(toolId);

		UpdateUI();

		if(!handlerAdded)
		{
			PlayerData.Instance.OnToolsUnlockUpdated += OnToolsCollectionUpdatedHandler;
			toggle.onValueChanged.AddListener(OnSelect);
			toggle.group = toggleGroup;
			handlerAdded = true;
		}
    }

    protected virtual void UpdateUI()
    {
		icon.sprite = tool.icon;
		icon.enabled = collectedStatus;
		nonCollectedIcon.sprite = tool.icon;
		nonCollectedIcon.enabled = !collectedStatus;
		displayName.text = tool.name;
		cost.text = tool.unlockCost.ToString();
    }

    void OnToolsCollectionUpdatedHandler()
    {
		collectedStatus = PlayerData.Instance.ToolsUnlocked.Contains(toolId);
		icon.enabled = collectedStatus;
		nonCollectedIcon.enabled = !collectedStatus;
    }

	public virtual void OnSelect(bool selectedState)
    {
        if (selectedState)
        {
            onSelectCb(this.toolId);
        }        
    }

    void OnDestroy()
    {
		if(handlerAdded && PlayerData.Instance != null)
		{
			PlayerData.Instance.OnToolsUnlockUpdated -= OnToolsCollectionUpdatedHandler;
			toggle.onValueChanged.RemoveListener(OnSelect);
			handlerAdded = !handlerAdded;
		}
    }
}
