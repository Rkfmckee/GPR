using UnityEngine;
using UnityEngine.UI;
using static ResourceController;

public class InventorySlot : MonoBehaviour {
	#region Properties

	public GameObject itemInSlot;

	private Button itemButton;
	private CraftingMenu inventoryController;
	
	private Sprite physicalMaterialsSprite;
	private Sprite magicalMaterialsSprite;

	#endregion

	#region Events

	private void Awake() {
		physicalMaterialsSprite = Resources.Load<Sprite>("Images/UI/RoundStage/ResourcesWoodPlanksOutline");
		magicalMaterialsSprite = Resources.Load<Sprite>("Images/UI/RoundStage/ResourcesPotionOutline");

		inventoryController = GetComponentInParent<CraftingMenu>();
		var itemButtonObject = transform.Find("ItemButton");
		itemButton = itemButtonObject.GetComponent<Button>();

		if (itemButton != null) {
			itemButton.onClick.AddListener(ItemButtonClicked);
		}

		var image = itemButtonObject.Find("ItemPrice").GetComponent<Image>();
		var resourceType = itemInSlot.GetComponent<CraftingItem>().GetResourceType();

		switch(resourceType) {
			case ResourceType.PhysicalMaterial:
				image.sprite = physicalMaterialsSprite;
				break;
			case ResourceType.MagicalMaterial:
				image.sprite = magicalMaterialsSprite;
				break;
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
