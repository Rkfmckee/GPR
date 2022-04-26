using System.Collections.Generic;
using UnityEngine;
using static NotificationController;

public class TrapLinkingLine : MonoBehaviour {
	#region Properties

	private LineRenderer lineRenderer;
	private new Camera camera;
	private RaycastHit hit;
	private int trapLinkingLineLayerMask;
	private GameObject firstBeingLinked;
	private List<string> actionText;

	private GlobalObstaclesController globalObstacles;
	private CanvasController canvasController;
	private List<GameObject> traps;
	private List<GameObject> triggers;

	#endregion

	#region Events

	private void Awake() {
		lineRenderer = GetComponent<LineRenderer>();
		camera = Camera.main;

		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, Vector3.zero);
		lineRenderer.startColor = Color.yellow;

		var layerMasks = GeneralHelper.GetLayerMasks();
		trapLinkingLineLayerMask = layerMasks["Highlightable"]
								| layerMasks["Obstacle"] 
								| layerMasks["Terrain"];

		actionText = new List<string> {
			"Left click trap/trigger to Link",
			"Left click anywhere else to Cancel"
		};
	}

	private void Start() {
		globalObstacles = References.Game.globalObstacles;
		canvasController = References.UI.Controllers.canvasController;
		traps = References.Obstacles.traps;
		triggers = References.Obstacles.triggers;

		canvasController.EnableActionText(actionText);
	}

	private void Update() {
		Ray cameraToMouse = camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity, trapLinkingLineLayerMask)) {
			if (References.Obstacles.trapsAndTriggers.Contains(hit.transform.gameObject)) {
				StickLineToTarget(hit.transform);
				ChangeLineColour(hit.transform.gameObject);
			} else {
				lineRenderer.SetPosition(1, hit.point);
				ChangeLineColour(null);

				if (Input.GetButtonDown("Fire1")) {
					Destroy(gameObject);
				}
			}
		}
	}

	private void OnDestroy() {
		canvasController.DisableActionText(actionText);
	}

	#endregion

	#region Methods

	public void SetStartValue(Transform startTransform) {
		lineRenderer.SetPosition(0, startTransform.position + (Vector3.up * (startTransform.GetComponent<Collider>().bounds.size.y / 2)));
		firstBeingLinked = startTransform.gameObject;
	}

	public void StickLineToTarget(Transform target) {
		lineRenderer.SetPosition(1, target.position);

		if (Input.GetButtonDown("Fire1")) {
			LinkWithFirst(target.gameObject);
		}
	}

	private void LinkWithFirst(GameObject secondBeingLinked) {
		TrapController firstTrap = firstBeingLinked.GetComponent<TrapController>();
		TriggerController firstTrigger = firstBeingLinked.GetComponent<TriggerController>();
		TrapController secondTrap = secondBeingLinked.GetComponent<TrapController>();
		TriggerController secondTrigger = secondBeingLinked.GetComponent<TriggerController>();
		
		if (firstBeingLinked == null || secondBeingLinked == null) {
			References.UI.notifications.AddNotification("The trap or trigger you are trying to link doesn't exist", NotificationType.Error);
			return;
		}

		if (firstBeingLinked == secondBeingLinked) {
			References.UI.notifications.AddNotification("You can't link a trap or trigger to itself", NotificationType.Error);
			return;
		}

		if (firstTrap != null && secondTrap != null) {
			References.UI.notifications.AddNotification("You can't link two traps together", NotificationType.Error);
			return;
		}

		if ((firstTrigger != null && firstTrap == null) && (secondTrigger != null && secondTrap == null)) {
			References.UI.notifications.AddNotification("You can't link two triggers together", NotificationType.Error);
			return;
		}

		if (firstTrap != null) {
			// The first object is a trap
			Link(firstTrap, secondTrigger);
		} else {
			// Otherwise the second object must be the trap
			Link(secondTrap, firstTrigger);
		}
	}

	private void Link(TrapController trap, TriggerController trigger) {
		trap.SetLinkedTrigger(trigger);
		trigger.SetLinkedTrap(trap);

		References.UI.notifications.AddNotification($"Linked {trap.GetName()} to {trigger.GetName()}", NotificationType.Success);
		References.Game.globalObstacles.RemoveTrapLinkingLine();
	}

	private void ChangeLineColour(GameObject target) {
		if (traps.Contains(firstBeingLinked)) {
			if (triggers.Contains(target)) {
				lineRenderer.endColor = Color.green;
				return;
			}
		}

		if (triggers.Contains(firstBeingLinked)) {
			if (traps.Contains(target)) {
				lineRenderer.endColor = Color.green;
				return;
			}
		}

		lineRenderer.endColor = Color.red;
	}

	#endregion
}
