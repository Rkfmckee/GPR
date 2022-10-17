using UnityEngine;
using UnityEngine.UI;

public class TrapModification : MonoBehaviour
{
	#region Fields

	private GameObject trap;
	private HealthSystem trapHealth;
	private ObstacleController trapController;
	private GameObject trapHealthBarAmount;
	private GameObject trapHealthValue;
	private Text trapHealthValueText;
	private Button linkButton;
	private Image linkButtonImage;

	private GlobalObstaclesController globalObstacles;

	#endregion

	#region Properties

	public GameObject Trap
	{
		set
		{
			trap           = value;
			trapHealth     = trap.GetComponent<HealthSystem>();
			trapController = trap.GetComponent<ObstacleController>();

			transform.Find("ObstacleDetails").GetComponent<ObstacleDetails>().Obstacle = trapController;
		}
	}

	#endregion

	#region Events

	private void Awake()
	{
		var background      = transform.Find("TrapModificationBackground");
		trapHealthBarAmount = background.Find("TrapHealthBarAmount").gameObject;
		trapHealthValue     = background.Find("TrapHealthValue").gameObject;
		trapHealthValueText = trapHealthValue.GetComponent<Text>();

		var link        = background.Find("LinkButton");
		linkButton      = link.GetComponent<Button>();
		linkButtonImage = link.GetComponent<Image>();

		linkButton.onClick.AddListener(LinkButtonClicked);
	}

	private void Start()
	{
		globalObstacles = References.Game.globalObstacles;
	}

	private void Update()
	{
		if (trapHealth == null)
		{
			return;
		}
		float currentHealth = Mathf.Round(trapHealth.GetCurrentHealth());
		float maxHealth     = trapHealth.MaxHealth;

		trapHealthValueText.text                 = $"Health: {currentHealth.ToString()}";
		trapHealthBarAmount.transform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);

	}

	#endregion

	#region Methods

	private void LinkButtonClicked()
	{
		globalObstacles.CreateTrapLinkingLine(trap.transform);
		globalObstacles.ShouldShowTrapDetails(false, null);
	}

	#endregion
}
