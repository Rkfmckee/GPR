using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
	#region Properties

	public GameObject itemInSlot;

	private Button itemButton;
	private CaveInventory inventoryController;

	#endregion

	#region Events

	private void Awake() {
		inventoryController = GetComponentInParent<CaveInventory>();
		itemButton = transform.Find("ItemButton").GetComponent<Button>();

		if (itemButton != null) {
			itemButton.onClick.AddListener(ItemButtonClicked);
		}

		GameObject itemPrice = transform.Find("ItemButton").Find("ItemPrice").gameObject;
		itemPrice.transform.Find("ItemPriceText").GetComponent<Text>().text = itemInSlot.GetComponent<CaveInventoryItem>().GetResourceCost().ToString();
	}

	#endregion

	#region Methods

	private void ItemButtonClicked() {
		inventoryController.SetSelectedItem(itemInSlot);
	}

	#endregion
}
