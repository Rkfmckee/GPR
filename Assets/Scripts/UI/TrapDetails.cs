using UnityEngine;
using UnityEngine.UI;

public class TrapDetails : MonoBehaviour {
	#region Properties

	private GameObject trapShowing;
	private HealthSystem trapHealth;
	private GameObject trapHealthBarAmount;
	private GameObject trapHealthValue;
	private Text trapHealthValueText;
	private Button linkButton;
	private Image linkButtonImage;

	private GlobalObstaclesController globalObstacles;

	#endregion

	#region Events

	private void Awake() {
		Transform background = transform.Find("TrapDetailsBackground");
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

	public void SetTrapShowing(GameObject trap) {
		trapShowing = trap;
		trapHealth = trap.GetComponent<HealthSystem>();
	}

	private void LinkButtonClicked() {
		globalObstacles.CreateTrapLinkingLine(trapShowing.transform);
		globalObstacles.ShouldShowTrapDetails(false, null);
	}

	#endregion
}
