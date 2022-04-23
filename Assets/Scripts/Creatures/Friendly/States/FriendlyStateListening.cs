using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static CameraController;
using static CursorData;

public class FriendlyStateListening : FriendlyState {
	#region Properties

	private float ignoreMouseClickTimer;
	private float ignoreMouseClickTime;
	private Dictionary<string, ListeningCommands> tagsToListeningCommands;
	private bool commandUiExists;
	private ListeningCommands? currentCommand;
	private int layerMask;

	private Camera camera;
	private FriendlyListeningUIController uiController;

	#endregion
	
	#region Constructor
	
	public FriendlyStateListening(GameObject gameObj) : base(gameObj) {
		uiController = References.UI.Controllers.friendlyListeningUIController;
		camera = References.Camera.camera;
		References.Camera.cameraController.SetControllingState(ControllingState.ControllingFriendly);

		tagsToListeningCommands = new Dictionary<string, ListeningCommands> {
			{"Floor", ListeningCommands.Move},
			{"Obstacle", ListeningCommands.PickUp},
			{"Trap", ListeningCommands.PickUp},
			{"Trigger", ListeningCommands.PickUp}
		};
		commandUiExists = false;
		
		var wallHidden = 1 << LayerMask.NameToLayer("WallHidden");
		var ignoreRaycast = 1 << LayerMask.NameToLayer("Ignore Raycast");
		layerMask = ~(wallHidden | ignoreRaycast);

		ResetIgnoreMouseClickTimer();
	}

	#endregion

	#region Events

	public override void Update() {
		base.Update();

		GetPointTargeted();
	}

	#endregion

	#region Methods

	private bool ShouldIgnoreMouseClick() {
		if (ignoreMouseClickTimer < ignoreMouseClickTime) {
			ignoreMouseClickTimer += Time.deltaTime;
			return true;
		}

		return false;
	}

	private void ResetIgnoreMouseClickTimer() {
		ignoreMouseClickTimer = 0;
		ignoreMouseClickTime = 0.5f;
	}

	private void GetPointTargeted() {
		Ray cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInformation;

		if (Physics.Raycast(cameraToMouseRay, out hitInformation, Mathf.Infinity, layerMask)) {
			var hitTag = hitInformation.transform.tag;

			if (!tagsToListeningCommands.ContainsKey(hitTag)) {
				DisableListeningCommand();
				return;
			}

			var hitCommand = tagsToListeningCommands[hitTag];

			if (!commandUiExists) {
				EnableListeningCommand();
			}

			if (hitCommand != currentCommand) {
				currentCommand = hitCommand;
				uiController.ChangeListeningCommandText(hitCommand.ToString());
				
				References.UI.Controllers.canvasController.DisableActionText();
				References.UI.Controllers.canvasController.EnableActionText($"Left click to {hitCommand.ToString()}");

				var cursorType = CursorData.ListeningCommandToCursorType(currentCommand.Value);
				if (cursorType.HasValue) {
					cursor.SetCursor(cursorType.Value);
				}
			}

			PerformCommandOnClick(hitCommand, hitInformation);
		}
	}

	private void PerformCommandOnClick(ListeningCommands command, RaycastHit hitInformation) {
		if (ShouldIgnoreMouseClick()) return;

		if (Input.GetButtonDown("Fire1")) {
			switch (command) {
				case ListeningCommands.Move:
					MoveCommand(hitInformation.point);
					break;

				case ListeningCommands.PickUp:
					PickupCommand(hitInformation.transform.gameObject);
					break;
			}
		}
	}

	private void MoveCommand(Vector3 targetPosition) {
		DisableListeningCommand();
		behaviour.SetCurrentState(new FriendlyStateGoTo(gameObject, targetPosition));
	}

	private void PickupCommand(GameObject pickupObject) {
		DisableListeningCommand();
		behaviour.SetCurrentState(new FriendlyStatePickupObject(gameObject, pickupObject));
	}

	private void EnableListeningCommand() {
		uiController.EnableListeningCommand();
		commandUiExists = true;
	}

	private void DisableListeningCommand() {
		uiController.DisableListeningCommand();
		commandUiExists = false;
		currentCommand = null;
		
		cursor.SetCursor(CursorType.Basic);
		References.UI.Controllers.canvasController.DisableActionText();
	}

	#endregion

	#region Enums

	public enum ListeningCommands {
		[Description("Move")]
		Move,
		[Description("Pick up")]
		PickUp
	}

	#endregion
}