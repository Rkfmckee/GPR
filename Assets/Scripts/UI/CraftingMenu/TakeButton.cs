using System;
using UnityEngine;
using UnityEngine.UI;
using static TrapController;

public class TakeButton : MonoBehaviour {
	#region Properties

	private Button takeButton;
	private Image image;
	private CraftingMenu craftingMenu;
	private Sprite buttonUnpressed;
	private Sprite buttonPressed;

	#endregion

	#region Events

	private void Awake() {
		craftingMenu = GetComponentInParent<CraftingMenu>();
		takeButton = GetComponent<Button>();
		takeButton.onClick.AddListener(TakeButtonClicked);

		image = GetComponent<Image>();
		buttonUnpressed = Resources.Load<Sprite>("Images/UI/CraftingMenu/TakeButton");
		buttonPressed = Resources.Load<Sprite>("Images/UI/CraftingMenu/TakeButtonPressed");

		SetButtonImagePressed(true);
	}

	#endregion

	#region Methods

	public void SetButtonImagePressed(bool pressed) {
		if (pressed) {
			image.sprite = buttonPressed;
			takeButton.enabled = false;
		} else {
			image.sprite = buttonUnpressed;
			takeButton.enabled = true;
		}
	}

	private void TakeButtonClicked() {
		GameObject itemSelected = craftingMenu.GetSelectedItem();
		if (itemSelected == null) {
			return;
		}

		var craftingItemController = itemSelected.GetComponent<CraftingItem>();

		if (craftingItemController.RemoveResourcesToSpawnItem()) {
			craftingMenu.GetCraftingStation().CraftItem(itemSelected);
		}
	}

	#endregion
}
