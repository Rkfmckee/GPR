using UnityEngine;
using UnityEngine.UI;

public class TakeButton : MonoBehaviour
{
	#region Fields

	private Button takeButton;
	private Image image;
	private CraftingMenu craftingMenu;
	private bool buttonPressed;
	private Sprite buttonSpriteUnpressed;
	private Sprite buttonSpritePressed;

	#endregion

	#region Properties

	public bool ButtonPressed 
	{
		get => buttonPressed;
		set
		{
			buttonPressed      = value;
			image.sprite       = value ? buttonSpritePressed : buttonSpriteUnpressed;
			takeButton.enabled = !value;
		}
	}

	#endregion

	#region Events

	private void Awake()
	{
		craftingMenu = GetComponentInParent<CraftingMenu>();
		takeButton   = GetComponent<Button>();
		takeButton.onClick.AddListener(TakeButtonClicked);

		image                 = GetComponent<Image>();
		buttonPressed         = true;
		buttonSpriteUnpressed = Resources.Load<Sprite>("Images/UI/CraftingMenu/TakeButton");
		buttonSpritePressed   = Resources.Load<Sprite>("Images/UI/CraftingMenu/TakeButtonPressed");

		ButtonPressed = true;
	}

	#endregion

	#region Methods

	private void TakeButtonClicked()
	{
		var itemSelected = craftingMenu.SelectedItem;
		if (itemSelected == null)
		{
			return;
		}

		var craftingItemController = itemSelected.GetComponent<CraftingItem>();

		if (craftingItemController.RemoveResourcesToSpawnItem())
		{
			craftingMenu.CraftingStation.CraftItem(itemSelected);
		}
	}

	#endregion
}
