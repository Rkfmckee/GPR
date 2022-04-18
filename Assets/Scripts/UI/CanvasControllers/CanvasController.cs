using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public class CanvasController : MonoBehaviour {
	#region Properties

	private GameObject caveInventory;
	private GameObject caveInventoryPrefab;

	private GameObject trapDetails;
	private GameObject trapDetailsPrefab;

	private GameObject placeItemText;
	private GameObject placeItemTextPrefab;

	private GameObject linkItemText;
	private GameObject linkItemTextPrefab;

	private List<GameObject> actionTextActive;
	private CameraController cameraController;

	#endregion

	#region Events

	private void Awake() {
		References.UI.canvas = gameObject;
		References.UI.Controllers.canvasController = this;
		actionTextActive = new List<GameObject>();
		cameraController = Camera.main.GetComponent<CameraController>();

		caveInventoryPrefab = Resources.Load<GameObject>("Prefabs/UI/CaveInventory/CaveInventory");
		trapDetailsPrefab = Resources.Load<GameObject>("Prefabs/UI/TrapDetails");

		placeItemTextPrefab = Resources.Load<GameObject>("Prefabs/UI/ActionText/PlaceItem");
		linkItemTextPrefab = Resources.Load<GameObject>("Prefabs/UI/ActionText/LinkItem");
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
        if (placeItemText == null) {
            placeItemText = Instantiate(placeItemTextPrefab);
            placeItemText.transform.SetParent(transform);
        }
	}

	public void DisableHoldingItemText() {
		if (placeItemText != null) Destroy(placeItemText);
	}

	public void EnableLinkingItemText(bool enable) {
		if (enable) {
			if (linkItemText == null) {
				linkItemText = Instantiate(linkItemTextPrefab);
				linkItemText.transform.SetParent(transform);
			}
		} else {
			if (linkItemText != null) Destroy(linkItemText);
		}
	}

	public void EnableActionText(Dictionary<ControllingState, List<GameObject>> statesAndUiText) {
		var actionTextForCurrentState = statesAndUiText[cameraController.GetControllingState()];
		
		EnableActionText(actionTextForCurrentState);
	}

	public void EnableActionText(List<GameObject> actionTextObjects) {
		if (actionTextObjects.Count <= 0) {
			return;
		}

		var currentPosition = actionTextObjects[0].transform.position;
		var amountToChangePosition = new Vector3(0, 50, 0);
		
		foreach(GameObject textObject in actionTextObjects) {
			GameObject textInstance = Instantiate(textObject);
			textInstance.transform.SetParent(transform);
			textInstance.transform.position = currentPosition;

			currentPosition += amountToChangePosition;
			actionTextActive.Add(textInstance);
		}
	}

	public void DisableActionText() {
		foreach(GameObject textObject in actionTextActive) {
			Destroy(textObject);
		}

		actionTextActive.Clear();
	}

	public bool IsActionTextActive() {
		return actionTextActive.Count > 0;
	}

	#endregion
}
