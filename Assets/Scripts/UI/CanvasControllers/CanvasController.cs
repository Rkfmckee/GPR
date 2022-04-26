using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CameraController;

public class CanvasController : MonoBehaviour {
	#region Properties

	private GameObject craftingMenu;
	private GameObject craftingMenuPrefab;

	private GameObject trapDetails;
	private GameObject trapDetailsPrefab;

	private Transform actionTextParent;
	private GameObject actionTextPrefab;
	private List<GameObject> actionTextActive;
	private CameraController cameraController;

	#endregion

	#region Events

	private void Awake() {
		References.UI.canvas = gameObject;
		References.UI.Controllers.canvasController = this;
		actionTextActive = new List<GameObject>();

		actionTextParent = transform.Find("Action Text");
		actionTextPrefab = Resources.Load<GameObject>("Prefabs/UI/ActionText");

		craftingMenuPrefab = Resources.Load<GameObject>("Prefabs/UI/CraftingMenu/CraftingMenu");
		trapDetailsPrefab = Resources.Load<GameObject>("Prefabs/UI/TrapDetails");
	}

	private void Start() {
		cameraController = References.Camera.cameraController;
	}

	#endregion

	#region Methods

	public void SetCraftingMenuVisible(bool visible, CraftingStation craftingStation = null) {
		if (visible) {
			if (craftingMenu == null) {
				craftingMenu = Instantiate(craftingMenuPrefab, References.UI.canvas.transform);
				craftingMenu.GetComponent<CraftingMenu>().CurrentCraftingStation(craftingStation);
				cameraController.SetControllingState(ControllingState.ControllingMenu);
			}
		} else {
			if (craftingMenu != null) {
				Destroy(craftingMenu);
				craftingMenu = null;
				cameraController.SetControllingState(ControllingState.ControllingSelf);
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

	public void EnableActionText(Dictionary<ControllingState, List<string>> statesAndUiText) {
		if (!statesAndUiText.ContainsKey(cameraController.GetControllingState())) {
			return;
		}
		
		var actionTextForCurrentState = statesAndUiText[cameraController.GetControllingState()];
		EnableActionText(actionTextForCurrentState);
	}

	public void EnableActionText(string actionText) {
		var actionTextList = new List<string> { actionText };
		EnableActionText(actionTextList);
	}

	public void EnableActionText(List<string> actionText) {
		if (actionText.Count <= 0) {
			return;
		}

		var startPosition = Vector3.zero;
		var amountToChangePosition = new Vector3(0, 30, 0);
		
		for(int i = actionText.Count - 1; i >= 0; i--) {
			GameObject textObject = Instantiate(actionTextPrefab, actionTextParent);
			textObject.GetComponent<TextMeshProUGUI>().text = actionText[i];
			textObject.transform.localPosition = startPosition;

			startPosition += amountToChangePosition;
			actionTextActive.Add(textObject);
		}
	}

	public void DisableActionText(Dictionary<ControllingState, List<string>> statesAndUiText) {
		if (!statesAndUiText.ContainsKey(cameraController.GetControllingState())) {
			return;
		}
		
		var actionTextForCurrentState = statesAndUiText[cameraController.GetControllingState()];
		DisableActionText(actionTextForCurrentState);
	}

	public void DisableActionText(List<string> actionText) {
		var objectsToRemove = new List<GameObject>();
		
		foreach(GameObject textObject in actionTextActive) {
			if (actionText.Contains(textObject.GetComponent<TextMeshProUGUI>().text)) {
				Destroy(textObject);
				objectsToRemove.Add(textObject);
			}
		}

		actionTextActive.RemoveAll(a => objectsToRemove.Contains(a));
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
