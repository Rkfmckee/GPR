using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CameraController;

public class CanvasController : MonoBehaviour {
	#region Properties

	private GameObject craftingMenu;
	private GameObject craftingMenuPrefab;

	private GameObject trapModification;
	private GameObject trapModificationPrefab;

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
		trapModificationPrefab = Resources.Load<GameObject>("Prefabs/UI/TrapModification");
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

	public void SetTrapModificationVisible(bool visible, GameObject trap) {
		if (visible) {
			if (trapModification == null) {
				trapModification = Instantiate(trapModificationPrefab, References.UI.canvas.transform);
				trapModification.GetComponent<TrapModification>().SetTrap(trap);
				cameraController.SetControllingState(ControllingState.ControllingMenu);
			}
		} else {
			if (trapModification != null) {
				Destroy(trapModification);
				trapModification = null;
				cameraController.SetControllingState(ControllingState.ControllingSelf);
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
