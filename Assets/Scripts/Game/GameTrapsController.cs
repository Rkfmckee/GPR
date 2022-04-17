using System.Collections.Generic;
using UnityEngine;

public class GameTrapsController : MonoBehaviour {
	#region Properties

	[HideInInspector]
	public List<GameObject> obstaclePlacements;

	private GameObject obstaclePlacementPrefab;
	private GameObject trapLinkingLinePrefab;
	private GameObject trapLinkingLine;
	private bool inventoryOpen;
	private bool trapDetailsOpen;
	private bool linkingTextActive;

	#endregion

	#region Events

	private void Awake() {
		References.GameController.gameTraps = this;
		obstaclePlacements = new List<GameObject>();

		obstaclePlacementPrefab = Resources.Load("Prefabs/Obstacles/Placement/ObstaclePlacement") as GameObject;
		trapLinkingLinePrefab = Resources.Load("Prefabs/UI/TrapLinkingLine") as GameObject;
	}

	#endregion

	#region Methods

	public void ShouldShowCaveInventory(bool showInventory) {
		References.UI.canvas.GetComponent<CanvasController>().SetCaveInventoryVisible(showInventory);
		inventoryOpen = showInventory;
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

	public bool IsLinkingTextActive() {
		return linkingTextActive;
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

	public void EnableLinkingItemText(bool enable) {
		References.UI.canvas.GetComponent<CanvasController>().EnableLinkingItemText(enable);
		linkingTextActive = enable;
	}

	public GameObject EnableObstaclePlacement(GameObject heldObject) {
		var obstaclePlacement = Instantiate(obstaclePlacementPrefab);
		obstaclePlacement.transform.parent = gameObject.transform;
		obstaclePlacement.GetComponent<ObstaclePlacementController>().SetHeldObject(heldObject);
		obstaclePlacements.Add(obstaclePlacement);

		References.UI.canvas.GetComponent<CanvasController>().EnableHoldingItemText();
		return obstaclePlacement;
	}

	public void DisableObstaclePlacement(GameObject obstaclePlacement) {
		if (obstaclePlacement != null) Destroy(obstaclePlacement);
		obstaclePlacements.Remove(obstaclePlacement);
		
		if (obstaclePlacements.Count <= 0)
			References.UI.canvas.GetComponent<CanvasController>().DisableHoldingItemText();
	}

	#endregion
}
