using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour {
	#region Properties

	private int physicalMaterialsPercentage;
	private int valuablesPercentage;
	private float barWidth;
	private float barHeight;
	private float physicalBarWidthOnePercent;
	private float magicalBarWidthOnePercent;
	private float valuableBarWidthOnePercent;

	private ResourceController resourceController;

	private GameObject physicalMaterialsGroup;
	private GameObject physicalMaterialsBar;
	private RectTransform physicalMaterialsBarRect;
	private Text physicalMaterialsValue;

	private GameObject magicalMaterialsGroup;
	private GameObject magicalMaterialsBar;
	private RectTransform magicalMaterialsBarRect;
	private Text magicalMaterialsValue;

	private GameObject valuablesGroup;
	private GameObject valuablesBar;
	private RectTransform valuablesBarRect;
	private Text valuablesValue;

	#endregion

	#region Events

	private void Awake() {
		References.UI.resources = this;
	}

	private void Start() {
		resourceController = References.Game.resources;

		physicalMaterialsGroup = References.UI.canvas.transform.Find("Resources").Find("PhysicalMaterials").gameObject;
		physicalMaterialsBar = physicalMaterialsGroup.transform.Find("PhysicalMaterialsBarAmount").gameObject;
		physicalMaterialsBarRect = physicalMaterialsBar.GetComponent<RectTransform>();
		physicalMaterialsValue = physicalMaterialsGroup.transform.Find("PhysicalMaterialsValue").GetComponent<Text>();

		magicalMaterialsGroup = References.UI.canvas.transform.Find("Resources").Find("MagicalMaterials").gameObject;
		magicalMaterialsBar = magicalMaterialsGroup.transform.Find("MagicalMaterialsBarAmount").gameObject;
		magicalMaterialsBarRect = magicalMaterialsBar.GetComponent<RectTransform>();
		magicalMaterialsValue = magicalMaterialsGroup.transform.Find("MagicalMaterialsValue").GetComponent<Text>();

		valuablesGroup = References.UI.canvas.transform.Find("Resources").Find("Valuables").gameObject;
		valuablesBar = valuablesGroup.transform.Find("ValuablesBarAmount").gameObject;
		valuablesBarRect = valuablesBar.GetComponent<RectTransform>();
		valuablesValue = valuablesGroup.transform.Find("ValuablesValue").GetComponent<Text>();

		barWidth = physicalMaterialsBar.GetComponent<RectTransform>().sizeDelta.x;
		barHeight = physicalMaterialsBar.GetComponent<RectTransform>().sizeDelta.y;

		physicalBarWidthOnePercent = (1f / resourceController.PhysicalMaterialMaximum) * barWidth;
		magicalBarWidthOnePercent = (1f / resourceController.MagicalMaterialMaximum) * barWidth;
		valuableBarWidthOnePercent = (1f / resourceController.ValuableMaximum) * barWidth;

		UpdateResourcesUI();
	}

	#endregion

	#region Methods

	public void UpdateResourcesUI() {
		var physicalMaterialsAmount = resourceController.PhysicalMaterialQuantity;
		var magicalMaterialsAmount = resourceController.MagicalMaterialQuantity;
		var valuablesAmount = resourceController.ValuableQuantity;

		physicalMaterialsBarRect.sizeDelta = new Vector2(physicalMaterialsAmount * physicalBarWidthOnePercent, barHeight);
		magicalMaterialsBarRect.sizeDelta = new Vector2(magicalMaterialsAmount * magicalBarWidthOnePercent, barHeight);
		valuablesBarRect.sizeDelta = new Vector2(valuablesAmount * valuableBarWidthOnePercent, barHeight);

		physicalMaterialsValue.text = physicalMaterialsAmount.ToString();
		magicalMaterialsValue.text = magicalMaterialsAmount.ToString();
		valuablesValue.text = valuablesAmount.ToString();
	}

	#endregion
}
