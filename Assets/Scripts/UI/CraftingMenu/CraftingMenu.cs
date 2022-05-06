using TMPro;
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
	private GameObject miscButton;
	private GameObject floorTrapItems;
	private GameObject wallTrapItems;
	private GameObject triggerItems;
	private GameObject miscItems;
	private GameObject takeButton;
	private GameObject obstacleDetails;
	private GameObject obstacleDetailsPrefab;

	private CraftingStation craftingStation;

	#endregion

	#region Events

	private void Awake() {
		obstacleDetailsPrefab = Resources.Load<GameObject>("Prefabs/UI/ObstacleDetails/ObstacleDetails");

		takeButton = transform.Find("TakeButton").gameObject;

		var background = transform.Find("Background");
		var categories = background.Find("Categories");

		floorTrapButton = categories.Find("FloorTraps").gameObject;
		wallTrapButton = categories.Find("WallTraps").gameObject;
		triggerButton = categories.Find("Triggers").gameObject;
		miscButton = categories.Find("Misc").gameObject;

		floorTrapButton.GetComponent<Button>().onClick.AddListener(() => ChangeCategory(Categories.FloorTrap));
		wallTrapButton.GetComponent<Button>().onClick.AddListener(() => ChangeCategory(Categories.WallTrap));
		triggerButton.GetComponent<Button>().onClick.AddListener(() => ChangeCategory(Categories.Trigger));
		miscButton.GetComponent<Button>().onClick.AddListener(() => ChangeCategory(Categories.Misc));
		
		floorTrapItems = background.Find("FloorTrapItems").gameObject;
		wallTrapItems = background.Find("WallTrapItems").gameObject;
		triggerItems = background.Find("TriggerItems").gameObject;
		miscItems = background.Find("MiscItems").gameObject;

		titleInitialColour = floorTrapButton.GetComponent<TextMeshProUGUI>().color;
		titleTransparentColor = titleInitialColour;
		titleTransparentColor.a = 0.5f;

		wallTrapButton.GetComponent<TextMeshProUGUI>().color = titleTransparentColor;
		triggerButton.GetComponent<TextMeshProUGUI>().color = titleTransparentColor;
		miscButton.GetComponent<TextMeshProUGUI>().color = titleTransparentColor;
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

	public void EnableObstacleDetails(ObstacleController obstacle) {
		DisableObstacleDetails();

		obstacleDetails = Instantiate(obstacleDetailsPrefab, transform);
		obstacleDetails.transform.position = Input.mousePosition + obstacleDetailsPrefab.transform.position;
		obstacleDetails.GetComponent<ObstacleDetails>().SetObstacle(obstacle);
	}

	public void DisableObstacleDetails() {
		if (obstacleDetails == null) 
			return;

		Destroy(obstacleDetails);
	}

	private void ChangeCategory(Categories categorySelected) {
		floorTrapItems.SetActive(false);
		floorTrapButton.GetComponent<TextMeshProUGUI>().color = titleTransparentColor;
		wallTrapItems.SetActive(false);
		wallTrapButton.GetComponent<TextMeshProUGUI>().color = titleTransparentColor;
		triggerItems.SetActive(false);
		triggerButton.GetComponent<TextMeshProUGUI>().color = titleTransparentColor;
		miscItems.SetActive(false);
		miscButton.GetComponent<TextMeshProUGUI>().color = titleTransparentColor;

		switch (categorySelected) {
			case Categories.FloorTrap:
				floorTrapItems.SetActive(true);
				floorTrapButton.GetComponent<TextMeshProUGUI>().color = titleInitialColour;
				break;
			case Categories.WallTrap:
				wallTrapItems.SetActive(true);
				wallTrapButton.GetComponent<TextMeshProUGUI>().color = titleInitialColour;
				break;
			case Categories.Trigger:
				triggerItems.SetActive(true);
				triggerButton.GetComponent<TextMeshProUGUI>().color = titleInitialColour;
				break;
			case Categories.Misc:
				miscItems.SetActive(true);
				miscButton.GetComponent<TextMeshProUGUI>().color = titleInitialColour;
				break;
		}

		SetSelectedItem(null);
	}

	#endregion

	#region Enums

	public enum Categories {
		FloorTrap,
		WallTrap,
		Trigger,
		Misc
	}

	#endregion
}
