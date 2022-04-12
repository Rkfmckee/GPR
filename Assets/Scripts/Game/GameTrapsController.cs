using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTrapsController : MonoBehaviour {
	#region Properties

	[HideInInspector]
	public GameObject objectPlacement;
	public ObjectPlacementController objectPlacementController;

	private GameObject objectPlacementPrefab;
	private GameObject trapLinkingLinePrefab;
	private GameObject trapLinkingLine;
	private bool inventoryOpen;
	private bool trapDetailsOpen;
	private bool linkingTextActive;

	#endregion

	#region Events

	private void Awake() {
		References.GameController.gameTraps = this;
		objectPlacementPrefab = Resources.Load("Prefabs/Objects/Placement/ObjectPlacement") as GameObject;
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

	public void EnableObjectPlacementIfPossible(GameObject heldObject) {
		StartCoroutine(CheckIfObjectPlacementCanBeEnabled(heldObject));
	}

	public void DisableObjectPlacement() {
		if (objectPlacement != null) Destroy(objectPlacement);

		References.UI.canvas.GetComponent<CanvasController>().DisableHoldingItemText();
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

	private void EnableObjectPlacement(GameObject heldObject) {
		objectPlacement = Instantiate(objectPlacementPrefab);
		objectPlacement.transform.parent = gameObject.transform;
		objectPlacementController = objectPlacement.GetComponent<ObjectPlacementController>();
		objectPlacementController.SetHeldObject(heldObject);

		References.UI.canvas.GetComponent<CanvasController>().EnableHoldingItemText();
	}

	#endregion

	#region Coroutines

	private IEnumerator CheckIfObjectPlacementCanBeEnabled(GameObject heldObject) {
		GameTrapsController gameTrapController = References.GameController.gameTraps;

		while (gameTrapController.IsInventoryOpen()) {
			yield return null;
		}

		EnableObjectPlacement(heldObject);
	}

	#endregion
}
