using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUIController : MonoBehaviour
{
    #region Properties

    private int physicalMaterialsPercentage;
    private int valuablesPercentage;
    private float barWidth;
    private float barHeight;
    private float physicalMaterialsPercentageModifier;
    private float valuablesPercentageModifier;

    private ResourceController resourceController;

    private GameObject physicalMaterialsGroup;
    private GameObject physicalMaterialsBar;
    private RectTransform physicalMaterialsBarRect;
    private Text physicalMaterialsValue;

    private GameObject valuablesGroup;
    private GameObject valuablesBar;
    private RectTransform valuablesBarRect;
    private Text valuablesValue;




    #endregion

    #region Events

    private void Start() {
        resourceController = References.GameController.resources;

        physicalMaterialsGroup = References.UI.canvas.transform.Find("Resources").Find("PhysicalMaterials").gameObject;
        physicalMaterialsBar = physicalMaterialsGroup.transform.Find("PhysicalMaterialsBarAmount").gameObject;
        physicalMaterialsBarRect = physicalMaterialsBar.GetComponent<RectTransform>();
        physicalMaterialsValue = physicalMaterialsGroup.transform.Find("PhysicalMaterialsValue").GetComponent<Text>();

        valuablesGroup = References.UI.canvas.transform.Find("Resources").Find("Valuables").gameObject;
        valuablesBar = valuablesGroup.transform.Find("ValuablesBarAmount").gameObject;
        valuablesBarRect = valuablesBar.GetComponent<RectTransform>();
        valuablesValue = valuablesGroup.transform.Find("ValuablesValue").GetComponent<Text>();

        barWidth = physicalMaterialsBar.GetComponent<RectTransform>().sizeDelta.x;
        barHeight = physicalMaterialsBar.GetComponent<RectTransform>().sizeDelta.y;

        physicalMaterialsPercentageModifier = (1f / resourceController.GetPhysicalMaterialMaximum()) * barWidth;
        valuablesPercentageModifier = (1f / resourceController.GetValuableMaximum()) * barWidth;
    }

    private void Update() {
        int physicalMaterialsAmount = resourceController.GetPhysicalMaterialAmount();
        int valuablesAmount = resourceController.GetValuableAmount();

        physicalMaterialsBarRect.sizeDelta = new Vector2(physicalMaterialsAmount * physicalMaterialsPercentageModifier, barHeight);
        valuablesBarRect.sizeDelta = new Vector2(valuablesAmount * valuablesPercentageModifier, barHeight);

        physicalMaterialsValue.text = physicalMaterialsAmount.ToString();
        valuablesValue.text = valuablesAmount.ToString();
    }

    #endregion
}
