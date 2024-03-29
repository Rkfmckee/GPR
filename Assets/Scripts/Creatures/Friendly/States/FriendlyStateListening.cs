using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static CameraController;
using static CursorData;

public class FriendlyStateListening : FriendlyState
{
	#region Fields

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

	public FriendlyStateListening(GameObject gameObj) : base(gameObj)
	{
		uiController = References.UI.friendlyListeningUIController;
		camera 		 = References.Camera.camera;
		References.Camera.cameraController.ControllingState = CameraControllingState.ControllingFriendly;

		tagsToListeningCommands = new Dictionary<string, ListeningCommands> {
			{"Floor", ListeningCommands.Move},
			{"Obstacle", ListeningCommands.PickUp},
			{"Trap", ListeningCommands.PickUp},
			{"Trigger", ListeningCommands.PickUp},
			{"HeldObstacle", ListeningCommands.PickUpSpawned}
		};
		commandUiExists = false;

		var layerMasks = GeneralHelper.GetLayerMasks();
		layerMask  	   = ~(layerMasks["WallHidden"] | layerMasks["Ignore Raycast"]);

		ResetIgnoreMouseClickTimer();
	}

	#endregion

	#region Events

	public override void Update()
	{
		base.Update();

		GetPointTargeted();
	}

	#endregion

	#region Methods

	private bool ShouldIgnoreMouseClick()
	{
		if (ignoreMouseClickTimer < ignoreMouseClickTime)
		{
			ignoreMouseClickTimer += Time.deltaTime;
			return true;
		}

		return false;
	}

	private void ResetIgnoreMouseClickTimer()
	{
		ignoreMouseClickTimer = 0;
		ignoreMouseClickTime  = 0.5f;
	}

	private void GetPointTargeted()
	{
		Ray cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInformation;

		if (Physics.Raycast(cameraToMouseRay, out hitInformation, Mathf.Infinity, layerMask))
		{
			var hitTag = hitInformation.transform.tag;

			if (!tagsToListeningCommands.ContainsKey(hitTag))
			{
				DisableListeningCommand();
				return;
			}

			var hitCommand = tagsToListeningCommands[hitTag];

			if (!commandUiExists)
			{
				EnableListeningCommand();
			}

			if (hitCommand != currentCommand)
			{
				currentCommand = hitCommand;
				uiController.ChangeListeningCommandText(hitCommand.GetDescription());

				References.UI.canvasController.DisableActionText();
				References.UI.canvasController.EnableActionText($"Left click to {hitCommand.GetDescription()}");

				var cursorType = CursorData.ListeningCommandToCursorType(currentCommand.Value);
				if (cursorType.HasValue)
				{
					cursor.SetCursor(cursorType.Value);
				}
			}

			PerformCommandOnClick(hitCommand, hitInformation);
		}
	}

	private void PerformCommandOnClick(ListeningCommands command, RaycastHit hitInformation)
	{
		if (ShouldIgnoreMouseClick()) return;

		if (Input.GetButtonDown("Fire1"))
		{
			switch (command)
			{
				case ListeningCommands.Move:
					MoveCommand(hitInformation.point);
					break;

				case ListeningCommands.PickUp:
					PickupCommand(hitInformation.transform.gameObject);
					break;

				case ListeningCommands.PickUpSpawned:
					var obstacle = hitInformation.transform.gameObject.GetComponent<ObstacleHeld>().obstacle;
					PickupCommand(obstacle, hitInformation.transform.gameObject);
					break;
			}
		}
	}

	private void MoveCommand(Vector3 targetPosition)
	{
		DisableListeningCommand();
		behaviour.CurrentState = new FriendlyStateGoTo(gameObject, targetPosition);
	}

	private void PickupCommand(GameObject pickupObject)
	{
		DisableListeningCommand();
		behaviour.CurrentState = new FriendlyStatePickupObject(gameObject, pickupObject);
	}

	private void PickupCommand(GameObject pickupObject, GameObject heldObject)
	{
		DisableListeningCommand();
		behaviour.CurrentState = new FriendlyStatePickupObject(gameObject, pickupObject, heldObject);
	}

	private void EnableListeningCommand()
	{
		uiController.EnableListeningCommand();
		commandUiExists = true;
	}

	private void DisableListeningCommand()
	{
		uiController.DisableListeningCommand();
		commandUiExists = false;
		currentCommand  = null;

		cursor.SetCursor(CursorType.Basic);
		References.UI.canvasController.DisableActionText();
	}

	#endregion

	#region Enums

	public enum ListeningCommands
	{
		[Description("Move")]
		Move,
		[Description("Pick up")]
		PickUp,
		[Description("Pick up")]
		PickUpSpawned
	}

	#endregion
}