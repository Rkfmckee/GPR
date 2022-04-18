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

		var itemIcon = transform.Find("ItemButton").Find("ItemIcon");
		itemIcon.GetComponent<Image>().sprite = itemInSlot.GetComponent<CaveInventoryItem>().inventoryIcon;

		var itemPrice = transform.Find("ItemButton").Find("ItemPrice");
		itemPrice.Find("ItemPriceText").GetComponent<Text>().text = itemInSlot.GetComponent<CaveInventoryItem>().resourceCost.ToString();
	}

	#endregion

	#region Methods

	private void ItemButtonClicked() {
		inventoryController.SetSelectedItem(itemInSlot);
	}

	#endregion
}
