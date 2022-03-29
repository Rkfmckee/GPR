using UnityEngine;

public class TrapLinkingLineController : MonoBehaviour {
	#region Properties

	private LineRenderer lineRenderer;
	private new Camera camera;
	private RaycastHit hit;
	private int trapLinkingLineLayerMask;
	private GameObject firstObjectBeingLinked;
	private GameObject secondObjectBeingLinked;
	private GameTrapsController gameTraps;

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

		gameTraps = References.GameController.gameTraps;
		if (!gameTraps.IsLinkingTextActive()) {
			gameTraps.EnableLinkingItemText(true);

			if (gameTraps.IsHighlightTextActive()) {
				gameTraps.EnableHighlightItemText(false, false);
			}
		}
	}

	private void Update() {
		Ray cameraToMouse = camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity, trapLinkingLineLayerMask)) {
			if (hit.transform.tag == "Trap" || hit.transform.tag == "Trigger") {
				lineRenderer.SetPosition(1, hit.transform.position);

				if (secondObjectBeingLinked == null) {
					secondObjectBeingLinked = hit.transform.gameObject;
				}

				if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
					LinkObjects();
				}
			} else {
				lineRenderer.SetPosition(1, hit.point);

				if (secondObjectBeingLinked != null) {
					secondObjectBeingLinked = null;
				}

				if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
					Destroy(gameObject);
				}
			}
		}
	}

	private void OnDestroy() {
		if (gameTraps.IsLinkingTextActive()) {
			gameTraps.EnableLinkingItemText(false);
		}
	}

	#endregion

	#region Methods

	public void SetStartValue(Transform startTransform) {
		lineRenderer.SetPosition(0, startTransform.position + (Vector3.up * (startTransform.GetComponent<Collider>().bounds.size.y / 2)));
		firstObjectBeingLinked = startTransform.gameObject;
	}

	private void LinkObjects() {
		if (firstObjectBeingLinked == null || secondObjectBeingLinked == null) {
			print("One of the objects you are trying to link doesn't exist");
			return;
		}

		if (firstObjectBeingLinked == secondObjectBeingLinked) {
			print("You can't link an object to itself");
			return;
		}

		if (firstObjectBeingLinked.tag == "Trap") {
			if (secondObjectBeingLinked.tag == "Trigger") {
				TrapTriggerController controller = FindTriggerController(secondObjectBeingLinked);
				controller.trapToTrigger = FindTrapController(firstObjectBeingLinked);

				References.UI.notifications.AddNotification($"Linked {firstObjectBeingLinked.name} to {secondObjectBeingLinked.name}");

				References.GameController.gameTraps.RemoveTrapLinkingLine();
				return;
			} else {
				print("Traps can only be linked to Triggers");
				return;
			}
		} else if (firstObjectBeingLinked.tag == "Trigger") {
			if (secondObjectBeingLinked.tag == "Trap") {
				TrapTriggerController controller = FindTriggerController(firstObjectBeingLinked);
				controller.trapToTrigger = FindTrapController(secondObjectBeingLinked);

				print($"Successfully linked {firstObjectBeingLinked.name} to {secondObjectBeingLinked.name}");

				References.GameController.gameTraps.RemoveTrapLinkingLine();
				return;
			} else {
				print("Triggers can only be linked to Traps");
				return;
			}
		}


	}

	private TrapController FindTrapController(GameObject target) {
		TrapController controller = target.GetComponent<TrapController>();

		//print($"TrapController: {controller}");
		return controller;
	}

	private TrapTriggerController FindTriggerController(GameObject target) {
		TrapTriggerController controller = target.GetComponent<TrapTriggerController>();

		//print($"TriggerController: {controller}");
		return controller;
	}

	#endregion
}
