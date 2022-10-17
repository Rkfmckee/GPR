using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	#region Fields

	[SerializeField]
	private GameObject itemInSlot;

	private Button itemButton;
	private CraftingMenu inventoryController;

	private Sprite physicalMaterialsSprite;
	private Sprite magicalMaterialsSprite;

	#endregion

	#region Properties

	public GameObject ItemInSlot { get => itemInSlot; }

	#endregion

	#region Events

	private void Awake()
	{
		physicalMaterialsSprite = Resources.Load<Sprite>("Images/UI/RoundStage/ResourcesWoodPlanksOutline");
		magicalMaterialsSprite  = Resources.Load<Sprite>("Images/UI/RoundStage/ResourcesPotionOutline");

		inventoryController = GetComponentInParent<CraftingMenu>();

		var itemButtonObject = transform.Find("ItemButton");
		itemButton       	 = itemButtonObject.GetComponent<Button>();

		if (itemButton != null)
		{
			itemButton.onClick.AddListener(ItemButtonClicked);
		}

		var image        = itemButtonObject.Find("ItemPrice").GetComponent<Image>();
		var resourceType = itemInSlot.GetComponent<CraftingItem>().ResourceType;

		switch (resourceType)
		{
			case ResourceType.PhysicalMaterials:
				image.sprite = physicalMaterialsSprite;
				break;
			case ResourceType.MagicalMaterials:
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

	private void ItemButtonClicked()
	{
		inventoryController.SelectedItem = itemInSlot;
	}

	#endregion
}
