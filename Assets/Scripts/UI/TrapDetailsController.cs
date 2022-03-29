using UnityEngine;
using UnityEngine.UI;

public class TrapDetailsController : MonoBehaviour {
	#region Properties

	private GameObject trapShowing;
	private HealthSystem trapHealth;
	private GameObject trapHealthBarAmount;
	private GameObject trapHealthValue;
	private Text trapHealthValueText;
	private Button linkButton;
	private Button repairButton;
	private Text repairButtonText;
	private Image repairButtonImage;
	private Image linkButtonImage;

	private Sprite repairSpriteUnpressed;
	private Sprite repairSpritePressed;
	private Sprite linkSpriteUnpressed;
	private Sprite linkSpritePressed;

	private GameTrapsController gameTraps;

	#endregion

	#region Events

	private void Awake() {
		Transform background = transform.Find("TrapDetailsBackground");
		trapHealthBarAmount = background.Find("TrapHealthBarAmount").gameObject;
		trapHealthValue = background.Find("TrapHealthValue").gameObject;
		trapHealthValueText = trapHealthValue.GetComponent<Text>();

		var repair = background.Find("RepairButton");
		repairButton = repair.GetComponent<Button>();
		repairButtonImage = repair.GetComponent<Image>();
		repairButtonText = repair.Find("RepairValue").GetComponent<Text>();

		var link = background.Find("LinkButton");
		linkButton = link.GetComponent<Button>();
		linkButtonImage = link.GetComponent<Image>();

		repairButton.onClick.AddListener(RepairButtonClicked);
		linkButton.onClick.AddListener(LinkButtonClicked);

		repairSpriteUnpressed = Resources.Load<Sprite>("Images/UI/TrapDetails/RepairButton");
		repairSpritePressed = Resources.Load<Sprite>("Images/UI/TrapDetails/RepairButtonPressed");
		linkSpriteUnpressed = Resources.Load<Sprite>("Images/UI/TrapDetails/LinkingLineButton");
		linkSpritePressed = Resources.Load<Sprite>("Images/UI/TrapDetails/LinkingLineButtonPressed");
	}

	private void Start() {
		gameTraps = References.GameController.gameTraps;
	}

	private void Update() {
		if (trapHealth != null) {
			float currentHealth = Mathf.Round(trapHealth.GetCurrentHealth());
			float maxHealth = trapHealth.maxHealth;

			trapHealthValueText.text = currentHealth.ToString();
			trapHealthBarAmount.transform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);
			repairButtonText.text = Mathf.Round(maxHealth - currentHealth).ToString();

			if (currentHealth < maxHealth) {
				SetRepairButtonClicked(false);
			} else {
				SetRepairButtonClicked(true);
			}
		}
	}

	#endregion

	#region Methods

	public void SetTrapShowing(GameObject trap) {
		trapShowing = trap;
		trapHealth = trap.GetComponent<HealthSystem>();
	}

	private void RepairButtonClicked() {
		float amountToHeal = Mathf.Round(trapHealth.maxHealth - trapHealth.GetCurrentHealth());
		if (References.GameController.resources.RemoveResourcesIfHaveEnough(ResourceController.ResourceType.PhysicalMaterial, (int)amountToHeal)) {
			trapHealth.Heal(amountToHeal);
		}
	}

	private void LinkButtonClicked() {
		linkButtonImage.sprite = linkSpritePressed;
		gameTraps.CreateTrapLinkingLine(trapShowing.transform);
		gameTraps.ShouldShowTrapDetails(false, null);
	}

	private void SetRepairButtonClicked(bool clicked) {
		if (clicked) {
			if (repairButtonImage.sprite == repairSpriteUnpressed) {
				repairButtonImage.sprite = repairSpritePressed;
				repairButtonText.enabled = false;
				repairButton.enabled = false;
			}
		} else {
			if (repairButtonImage.sprite == repairSpritePressed) {
				repairButtonImage.sprite = repairSpriteUnpressed;
				repairButtonText.enabled = true;
				repairButton.enabled = true;
			}
		}
	}

	#endregion
}
