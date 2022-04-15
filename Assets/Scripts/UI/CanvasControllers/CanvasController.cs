using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public class CanvasController : MonoBehaviour {
	#region Properties

	private GameObject caveInventory;
	private GameObject caveInventoryPrefab;

	private GameObject trapDetails;
	private GameObject trapDetailsPrefab;

	private GameObject holdingItemPlaceText;
	private GameObject holdingItemThrowText;
	private GameObject holdingItemPlacePrefab;
	private GameObject holdingItemThrowPrefab;

	private GameObject linkItemBothText;
	private GameObject pickupItemTextPrefab;
	private GameObject modifyItemTextPrefab;
	private GameObject linkItemBothTextPrefab;

	private List<GameObject> highlightTextActive;
	private CameraController cameraController;

	#endregion

	#region Events

	private void Awake() {
		References.UI.canvas = gameObject;
		References.UI.Controllers.canvasController = this;
		highlightTextActive = new List<GameObject>();
		cameraController = Camera.main.GetComponent<CameraController>();

		caveInventoryPrefab = Resources.Load<GameObject>("Prefabs/UI/CaveInventory");
		trapDetailsPrefab = Resources.Load<GameObject>("Prefabs/UI/TrapDetails");
		holdingItemPlacePrefab = Resources.Load<GameObject>("Prefabs/UI/HoldingItemPlace");
		holdingItemThrowPrefab = Resources.Load<GameObject>("Prefabs/UI/HoldingItemThrow");

		pickupItemTextPrefab = Resources.Load<GameObject>("Prefabs/UI/PickupItem");
		modifyItemTextPrefab = Resources.Load<GameObject>("Prefabs/UI/ModifyItem");
		linkItemBothTextPrefab = Resources.Load<GameObject>("Prefabs/UI/LinkItemBoth");
	}

	#endregion

	#region Methods

	public void SetCaveInventoryVisible(bool visible) {
		if (visible) {
			if (caveInventory == null) {
				caveInventory = Instantiate(caveInventoryPrefab, References.UI.canvas.transform);
			}
		} else {
			if (caveInventory != null) {
				Destroy(caveInventory);
				caveInventory = null;
			}
		}
	}

	public void SetTrapDetailsVisible(bool visible, GameObject trap) {
		if (visible) {
			if (trapDetails == null) {
				trapDetails = Instantiate(trapDetailsPrefab, References.UI.canvas.transform);
				trapDetails.GetComponent<TrapDetails>().SetTrapShowing(trap);
			}
		} else {
			if (trapDetails != null) {
				Destroy(trapDetails);
				trapDetails = null;
			}
		}
	}

	public void EnableHoldingItemText() {
		if (holdingItemPlaceText == null) {
			holdingItemPlaceText = Instantiate(holdingItemPlacePrefab);
			holdingItemPlaceText.transform.SetParent(transform);
		}
	}

	public void DisableHoldingItemText() {
		if (holdingItemPlaceText != null) Destroy(holdingItemPlaceText);
		if (holdingItemThrowText != null) Destroy(holdingItemThrowText);
	}

	public void EnableHighlightText(Dictionary<ControllingState, List<GameObject>> statesAndUiText) {
		List<GameObject> uiText = statesAndUiText[cameraController.GetControllingState()];
		
		foreach(GameObject textObject in uiText) {
			GameObject textInstance = Instantiate(textObject);
			textInstance.transform.SetParent(transform);

			highlightTextActive.Add(textInstance);
		}
	}

	public void DisableHighlightText() {
		foreach(GameObject textObject in highlightTextActive) {
			Destroy(textObject);
		}

		highlightTextActive.Clear();
	}

	public bool IsHighlightTextActive() {
		return highlightTextActive.Count > 0;
	}

	public void EnableLinkingItemText(bool enable) {
		if (enable) {
			if (linkItemBothText == null) {
				linkItemBothText = Instantiate(linkItemBothTextPrefab);
				linkItemBothText.transform.SetParent(transform);
			}
		} else {
			if (linkItemBothText != null) Destroy(linkItemBothText);
		}
	}

	#endregion
}
