using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour {
	#region Properties

	private Color titleInitialColour;
	private Color titleTransparentColor;

	private GameObject itemSelected;
	private GameObject floorTrapButton;
	private GameObject wallTrapButton;
	private GameObject triggerButton;
	private GameObject floorTrapItems;
	private GameObject wallTrapItems;
	private GameObject triggerItems;
	private GameObject takeButton;

	private CraftingStation craftingStation;

	#endregion

	#region Events

	private void Awake() {
		takeButton = transform.Find("TakeButton").gameObject;

		floorTrapButton = transform.Find("FloorTraps").gameObject;
		wallTrapButton = transform.Find("WallTraps").gameObject;
		triggerButton = transform.Find("Triggers").gameObject;

		floorTrapButton.GetComponent<Button>().onClick.AddListener(() => ChangeCategory(Categories.FloorTrap));
		wallTrapButton.GetComponent<Button>().onClick.AddListener(() => ChangeCategory(Categories.WallTrap));
		triggerButton.GetComponent<Button>().onClick.AddListener(() => ChangeCategory(Categories.Trigger));

		Transform background = transform.Find("Background");
		floorTrapItems = background.Find("FloorTrapItems").gameObject;
		wallTrapItems = background.Find("WallTrapItems").gameObject;
		triggerItems = background.Find("TriggerItems").gameObject;

		titleInitialColour = floorTrapButton.GetComponent<Image>().color;
		titleTransparentColor = titleInitialColour;
		titleTransparentColor.a = 0.5f;

		wallTrapButton.GetComponent<Image>().color = titleTransparentColor;
		triggerButton.GetComponent<Image>().color = titleTransparentColor;
	}

	#endregion

	#region Methods

		#region Get/Set
		public GameObject GetSelectedItem() {
			return itemSelected;
		}

		public void SetSelectedItem(GameObject item) {
			itemSelected = item;

			if (item != null) {
				takeButton.GetComponent<TakeButton>().SetButtonImagePressed(false);
			} else {
				takeButton.GetComponent<TakeButton>().SetButtonImagePressed(true);
			}
		}

		public CraftingStation GetCraftingStation() {
			return craftingStation;
		}

		public void CurrentCraftingStation(CraftingStation station) {
			craftingStation = station;
		}

		#endregion

	private void ChangeCategory(Categories categorySelected) {
		floorTrapItems.SetActive(false);
		floorTrapButton.GetComponent<Image>().color = titleTransparentColor;
		wallTrapItems.SetActive(false);
		wallTrapButton.GetComponent<Image>().color = titleTransparentColor;
		triggerItems.SetActive(false);
		triggerButton.GetComponent<Image>().color = titleTransparentColor;

		switch (categorySelected) {
			case Categories.FloorTrap:
				floorTrapItems.SetActive(true);
				floorTrapButton.GetComponent<Image>().color = titleInitialColour;
				break;
			case Categories.WallTrap:
				wallTrapItems.SetActive(true);
				wallTrapButton.GetComponent<Image>().color = titleInitialColour;
				break;
			case Categories.Trigger:
				triggerItems.SetActive(true);
				triggerButton.GetComponent<Image>().color = titleInitialColour;
				break;
		}

		SetSelectedItem(null);
	}

	#endregion

	#region Enums

	public enum Categories {
		FloorTrap,
		WallTrap,
		Trigger
	}

	#endregion
}
