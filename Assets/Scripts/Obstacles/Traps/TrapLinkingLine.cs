using System.Collections.Generic;
using UnityEngine;

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

	#endregion

	#region Events

	private void Awake() {
		lineRenderer = GetComponent<LineRenderer>();
		camera = Camera.main;

		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, Vector3.zero);

		int highlightableObjectLayerMask = 1 << LayerMask.NameToLayer("Highlightable");
		int obstacleLayerMask = 1 << LayerMask.NameToLayer("Obstacle");
		int wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
		int floorLayerMask = 1 << LayerMask.NameToLayer("Floor");
		trapLinkingLineLayerMask = highlightableObjectLayerMask | obstacleLayerMask | wallLayerMask | floorLayerMask;

		actionText = new List<string> {
			"Left click trap/trigger to Link",
			"Left click anywhere else to Cancel"
		};
	}

	private void Start() {
		globalObstacles = References.Game.globalObstacles;
		canvasController = References.UI.Controllers.canvasController;

		canvasController.EnableActionText(actionText);
	}

	private void Update() {
		Ray cameraToMouse = camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity, trapLinkingLineLayerMask)) {
			if (References.Obstacles.allTrapsAndTriggers.Contains(hit.transform.gameObject)) {
				print("here");
				StickLineToTarget(hit.transform);
			} else {
				lineRenderer.SetPosition(1, hit.point);

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
			References.UI.notifications.AddNotification("The trap or trigger you are trying to link doesn't exist");
			return;
		}

		if (firstBeingLinked == secondBeingLinked) {
			References.UI.notifications.AddNotification("You can't link a trap or trigger to itself");
			return;
		}

		if (firstTrap != null && secondTrap != null) {
			References.UI.notifications.AddNotification("You can't link two traps together");
			return;
		}

		if (firstTrigger != null && secondTrigger != null) {
			References.UI.notifications.AddNotification("You can't link two triggers together");
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

		References.UI.notifications.AddNotification($"Linked {trap.GetName()} to {trigger.GetName()}");
		References.Game.globalObstacles.RemoveTrapLinkingLine();
	}

	#endregion
}
