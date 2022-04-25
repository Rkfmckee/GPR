using System.Collections.Generic;
using UnityEngine;

public class GlobalObstaclesController : MonoBehaviour {
	#region Properties

	private List<GameObject> obstaclePlacements;
	private GameObject obstaclePlacementPrefab;
	private GameObject trapLinkingLinePrefab;
	private GameObject trapLinkingLine;
	private bool inventoryOpen;
	private bool trapDetailsOpen;

	private CanvasController canvasController;

	#endregion

	#region Events

	private void Awake() {
		References.Game.globalObstacles = this;
		obstaclePlacements = new List<GameObject>();

		obstaclePlacementPrefab = Resources.Load("Prefabs/Obstacles/Placement/ObstaclePlacement") as GameObject;
		trapLinkingLinePrefab = Resources.Load("Prefabs/UI/TrapLinkingLine") as GameObject;
	}

	private void Start() {
		canvasController = References.UI.canvas.GetComponent<CanvasController>();
	}

	#endregion

	#region Methods

	public void ShouldShowCraftingMenu(bool showMenu, CraftingStation craftingStation = null) {
		References.UI.canvas.GetComponent<CanvasController>().SetCraftingMenuVisible(showMenu, craftingStation);
		inventoryOpen = showMenu;
	}

	public void ShouldShowTrapDetails(bool showDetails, GameObject trap) {
		References.UI.canvas.GetComponent<CanvasController>().SetTrapDetailsVisible(showDetails, trap);
		trapDetailsOpen = showDetails;
	}

	public bool IsInventoryOpen() {
		return inventoryOpen;
	}

	public bool IsTrapDetailsOpen() {
		return trapDetailsOpen;
	}

	public bool IsTrapLinkingLineActive() {
		return trapLinkingLine != null;
	}

	public void RemoveTrapLinkingLine() {
		if (trapLinkingLine != null) {
			Destroy(trapLinkingLine);
		}
	}

	public void CreateTrapLinkingLine(Transform startTransform) {
		if (trapLinkingLine == null) {
			trapLinkingLine = Instantiate(trapLinkingLinePrefab);
			trapLinkingLine.GetComponent<TrapLinkingLine>().SetStartValue(startTransform);
		}
	}

	public GameObject EnableObstaclePlacement(GameObject heldObject) {
		var obstaclePlacement = Instantiate(obstaclePlacementPrefab);
		var obstaclePlacementController = obstaclePlacement.GetComponent<ObstaclePlacementController>();

		obstaclePlacement.transform.parent = gameObject.transform;
		obstaclePlacementController.SetHeldObject(heldObject);
		obstaclePlacements.Add(obstaclePlacement);

		canvasController.EnableActionText(obstaclePlacementController.GetActionText());
		return obstaclePlacement;
	}

	public void DisableObstaclePlacement(GameObject obstaclePlacement) {
		if (obstaclePlacement != null) Destroy(obstaclePlacement);
		obstaclePlacements.Remove(obstaclePlacement);
	}

	#endregion
}
