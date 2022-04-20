using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
	#region Properties

	public GameObject itemInSlot;

	private Button itemButton;
	private CraftingMenu inventoryController;

	#endregion

	#region Events

	private void Awake() {
		inventoryController = GetComponentInParent<CraftingMenu>();
		itemButton = transform.Find("ItemButton").GetComponent<Button>();

		if (itemButton != null) {
			itemButton.onClick.AddListener(ItemButtonClicked);
		}

		var itemIcon = transform.Find("ItemButton").Find("ItemIcon");
		itemIcon.GetComponent<Image>().sprite = itemInSlot.GetComponent<CraftingItem>().inventoryIcon;

		var itemPrice = transform.Find("ItemButton").Find("ItemPrice");
		itemPrice.Find("ItemPriceText").GetComponent<Text>().text = itemInSlot.GetComponent<CraftingItem>().resourceCost.ToString();
	}

	#endregion

	#region Methods

	private void ItemButtonClicked() {
		inventoryController.SetSelectedItem(itemInSlot);
	}

	#endregion
}
