using UnityEngine;
using UnityEngine.UI;

public class TakeButton : MonoBehaviour {
	#region Properties

	private Button takeButton;
	private Image image;
	private CraftingMenu craftingMenu;
	private bool buttonPressed; 
	private Sprite buttonSpriteUnpressed;
	private Sprite buttonSpritePressed;

	#endregion

	#region Events

	private void Awake() {
		craftingMenu = GetComponentInParent<CraftingMenu>();
		takeButton = GetComponent<Button>();
		takeButton.onClick.AddListener(TakeButtonClicked);

		image = GetComponent<Image>();
		buttonPressed = true;
		buttonSpriteUnpressed = Resources.Load<Sprite>("Images/UI/CraftingMenu/TakeButton");
		buttonSpritePressed = Resources.Load<Sprite>("Images/UI/CraftingMenu/TakeButtonPressed");

		SetButtonImagePressed(true);
	}

	#endregion

	#region Methods

		#region Get/Set

		public bool IsButtonPressed() {
			return buttonPressed;
		}

		public void SetButtonImagePressed(bool pressed) {
			buttonPressed = pressed;
			
			if (pressed) {
				image.sprite = buttonSpritePressed;
				takeButton.enabled = false;
			} else {
				image.sprite = buttonSpriteUnpressed;
				takeButton.enabled = true;
			}
		}

		#endregion

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
