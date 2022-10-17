using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{
	#region Fields

	private Color titleInitialColour;
	private Color titleTransparentColor;

	private GameObject selectedItem;
	private GameObject floorTrapButton;
	private GameObject wallTrapButton;
	private GameObject ceilingTrapButton;
	private GameObject triggerButton;
	private GameObject miscButton;

	private GameObject floorTrapItems;
	private GameObject wallTrapItems;
	private GameObject ceilingTrapItems;
	private GameObject triggerItems;
	private GameObject miscItems;

	private GameObject takeButton;
	private GameObject obstacleDetails;
	private GameObject obstacleDetailsPrefab;

	private CraftingStation craftingStation;

	#endregion

	#region Properties

	public GameObject SelectedItem 
	{ 
		get => selectedItem; 
		set
		{
			selectedItem = value;
			takeButton.GetComponent<TakeButton>().ButtonPressed = value == null;
		}
	}

	public CraftingStation CraftingStation { get => craftingStation; set => craftingStation = value; }

	#endregion

	#region Events

	private void Awake()
	{
		obstacleDetailsPrefab = Resources.Load<GameObject>("Prefabs/UI/CraftingMenu/ObstacleDetails");

		takeButton = transform.Find("TakeButton").gameObject;

		var background = transform.Find("Background");
		var categories = background.Find("Categories");

		floorTrapButton   = categories.Find("FloorTraps").gameObject;
		wallTrapButton    = categories.Find("WallTraps").gameObject;
		ceilingTrapButton = categories.Find("CeilingTraps").gameObject;
		triggerButton     = categories.Find("Triggers").gameObject;
		miscButton        = categories.Find("Misc").gameObject;

		floorTrapButton.GetComponent<Button>().onClick.AddListener(()   => ChangeCategory(Categories.FloorTrap));
		wallTrapButton.GetComponent<Button>().onClick.AddListener(()    => ChangeCategory(Categories.WallTrap));
		ceilingTrapButton.GetComponent<Button>().onClick.AddListener(() => ChangeCategory(Categories.CeilingTrap));
		triggerButton.GetComponent<Button>().onClick.AddListener(() 	=> ChangeCategory(Categories.Trigger));
		miscButton.GetComponent<Button>().onClick.AddListener(() 		=> ChangeCategory(Categories.Misc));

		floorTrapItems   = background.Find("FloorTrapItems").gameObject;
		wallTrapItems    = background.Find("WallTrapItems").gameObject;
		ceilingTrapItems = background.Find("CeilingTrapItems").gameObject;
		triggerItems     = background.Find("TriggerItems").gameObject;
		miscItems        = background.Find("MiscItems").gameObject;

		titleInitialColour      = floorTrapButton.GetComponent<TextMeshProUGUI>().color;
		titleTransparentColor   = titleInitialColour;
		titleTransparentColor.a = 0.5f;

		wallTrapButton.GetComponent<TextMeshProUGUI>   ().color = titleTransparentColor;
		ceilingTrapButton.GetComponent<TextMeshProUGUI>().color = titleTransparentColor;
		triggerButton.GetComponent<TextMeshProUGUI>    ().color = titleTransparentColor;
		miscButton.GetComponent<TextMeshProUGUI>       ().color = titleTransparentColor;
	}

	#endregion

	#region Methods

	public void EnableObstacleDetails(ObstacleController obstacle)
	{
		DisableObstacleDetails();

		obstacleDetails                    = Instantiate(obstacleDetailsPrefab, transform);
		obstacleDetails.transform.position = Input.mousePosition + obstacleDetailsPrefab.transform.position;

		obstacleDetails.GetComponent<ObstacleDetails>().Obstacle = obstacle;
	}

	public void DisableObstacleDetails()
	{
		if (obstacleDetails == null)
			return;

		Destroy(obstacleDetails);
	}

	private void ChangeCategory(Categories categorySelected)
	{
		floorTrapItems.SetActive(false);
		wallTrapItems.SetActive(false);
		ceilingTrapItems.SetActive(false);
		triggerItems.SetActive(false);
		miscItems.SetActive(false);

		floorTrapButton.GetComponent<TextMeshProUGUI>  ().color = titleTransparentColor;
		wallTrapButton.GetComponent<TextMeshProUGUI>   ().color = titleTransparentColor;
		ceilingTrapButton.GetComponent<TextMeshProUGUI>().color = titleTransparentColor;
		triggerButton.GetComponent<TextMeshProUGUI>    ().color = titleTransparentColor;
		miscButton.GetComponent<TextMeshProUGUI>       ().color = titleTransparentColor;

		switch (categorySelected)
		{
			case Categories.FloorTrap:
				floorTrapItems.SetActive(true);
				floorTrapButton.GetComponent<TextMeshProUGUI>().color = titleInitialColour;
				break;
			case Categories.WallTrap:
				wallTrapItems.SetActive(true);
				wallTrapButton.GetComponent<TextMeshProUGUI>().color = titleInitialColour;
				break;
			case Categories.CeilingTrap:
				ceilingTrapItems.SetActive(true);
				ceilingTrapButton.GetComponent<TextMeshProUGUI>().color = titleInitialColour;
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

		SelectedItem = null;
	}

	#endregion

	#region Enums

	public enum Categories
	{
		FloorTrap,
		WallTrap,
		CeilingTrap,
		Trigger,
		Misc
	}

	#endregion
}
