using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsPurchasePopUpController : MonoBehaviour {

	[SerializeField] GameObject purchaseAvailableContainer;
	[SerializeField] GameObject purchaseNotAvailableContainer;

	[SerializeField] Text purchaseLabel;
	[SerializeField] Text unableToPurchaseLabel;

	string unableToPurchaseSampleText, purchaseMsgText;
	int toolId;
    ToolsDBScriptableObject.Tool toolData;

    void Awake()
	{
		unableToPurchaseSampleText = unableToPurchaseLabel.text;
		purchaseMsgText = purchaseLabel.text;
	}

	public void Show(int id, ToolsDBScriptableObject.Tool toolData)
	{
		this.gameObject.SetActive(true);

        this.toolId = id;
        this.toolData = toolData;
		bool isAbleToPurchase = PlayerData.Instance.CoinsAvailable >= toolData.unlockCost;
		purchaseAvailableContainer.SetActive(isAbleToPurchase);
		purchaseNotAvailableContainer.SetActive(!isAbleToPurchase);

		if(!isAbleToPurchase)
		{
			unableToPurchaseLabel.text = string.Format(unableToPurchaseSampleText, toolData.name, toolData.unlockCost - PlayerData.Instance.CoinsAvailable);
		}
		else
		{
			purchaseLabel.text = string.Format(purchaseMsgText, toolData.name, toolData.unlockCost);
		}
	}

	public void DoPurchase()
	{
		PlayerData.Instance.BuyTool(this.toolId, toolData);
	}
}
