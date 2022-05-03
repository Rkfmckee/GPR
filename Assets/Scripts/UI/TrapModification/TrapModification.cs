using UnityEngine;
using UnityEngine.UI;

public class TrapModification : MonoBehaviour {
	#region Properties

	private GameObject trap;
	private HealthSystem trapHealth;
	private TrapTriggerBase trapController;
	private GameObject trapHealthBarAmount;
	private GameObject trapHealthValue;
	private Text trapHealthValueText;
	private Button linkButton;
	private Image linkButtonImage;

	private GlobalObstaclesController globalObstacles;

	#endregion

	#region Events

	private void Awake() {
		Transform background = transform.Find("TrapModificationBackground");
		trapHealthBarAmount = background.Find("TrapHealthBarAmount").gameObject;
		trapHealthValue = background.Find("TrapHealthValue").gameObject;
		trapHealthValueText = trapHealthValue.GetComponent<Text>();

		var link = background.Find("LinkButton");
		linkButton = link.GetComponent<Button>();
		linkButtonImage = link.GetComponent<Image>();

		linkButton.onClick.AddListener(LinkButtonClicked);
	}

	private void Start() {
		globalObstacles = References.Game.globalObstacles;
	}

	private void Update() {
		if (trapHealth == null) {
			return;
		}
		float currentHealth = Mathf.Round(trapHealth.GetCurrentHealth());
		float maxHealth = trapHealth.maxHealth;

		trapHealthValueText.text = $"Health: {currentHealth.ToString()}";
		trapHealthBarAmount.transform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);
		
	}

	#endregion

	#region Methods

	public void SetTrap(GameObject trap) {
		this.trap = trap;
		trapHealth = trap.GetComponent<HealthSystem>();
		trapController = trap.GetComponent<TrapTriggerBase>();

		transform.Find("TrapDetails").GetComponent<TrapDetails>().SetTrap(trapController);
	}

	private void LinkButtonClicked() {
		globalObstacles.CreateTrapLinkingLine(trap.transform);
		globalObstacles.ShouldShowTrapDetails(false, null);
	}

	#endregion
}
