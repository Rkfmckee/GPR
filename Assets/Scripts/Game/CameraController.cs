using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using static GeneralHelper;

public class CameraController : MonoBehaviour {
	public float cameraTransitionTime;
	public float movementSpeed;
	public float orientationChangeTime;

	private new Camera camera;
	private Vector3 velocity;
	private CameraState currentState;
	private CameraOrientation currentOrientation;
	private Quaternion angledRotation;
	private float angledHeight;
	private Quaternion topDownRotation;
	private float topDownHeight;
	private float orientationChangeTimeCurrent;

	private List<GameObject> hiddenObjects;
	private int hiddenLayers;

	#region Events

	private void Awake() {
		hiddenObjects = new List<GameObject>();

		int wallMask = 1 << LayerMask.NameToLayer("Wall");
		int wallIgnoreMask = 1 << LayerMask.NameToLayer("WallIgnoreRaycast");
		hiddenLayers = wallMask | wallIgnoreMask;

		angledRotation = transform.rotation;
		angledHeight = transform.position.y;
		topDownRotation = Quaternion.Euler(90, 0, 0);
		topDownHeight = transform.position.y + 7;

		currentState = CameraState.CONTROLLED_MOVEMENT;
		currentOrientation = CameraOrientation.ANGLED;
	}

	void Update() {
		if (Input.GetKeyDown("space")) {
			ChangeOrientation();
		}

		if (currentState == CameraState.CONTROLLED_MOVEMENT) {
			HandleMovement();
		}

		if (currentOrientation == CameraOrientation.ANGLED) {
			SeeThroughWalls();
		}
	}

	#endregion

	#region Methods

	private void HandleMovement() {
		float verticalAxis = Input.GetAxis("Vertical");
		float horizontalAxis = Input.GetAxis("Horizontal");
		Vector3 direction = new Vector3(horizontalAxis, verticalAxis, 0);

		if (currentOrientation == CameraOrientation.ANGLED) {
			direction = new Vector3(horizontalAxis, verticalAxis, verticalAxis);
		}

		direction = NormaliseVectorToKeepDeceleration(direction);
		Vector3 movementAmount = direction * movementSpeed * Time.deltaTime;
		transform.Translate(movementAmount);
	}

	private void ChangeOrientation() {
		print("Change orientation");
		CameraOrientation orientation = CameraOrientation.TOP_DOWN;

		if (currentOrientation == CameraOrientation.TOP_DOWN) {
			orientation = CameraOrientation.ANGLED;
		}
		
		StartCoroutine(TransitionOrientation(orientation));
	}

	// private Vector3 ChangePosition(Vector3 newPosition) {
	// 	// Only use smooth transition if in the Transitioning state

	// 	float distanceUntilEndTransition = 0.1f;

	// 	float transitionTime = 0;
	// 	float xDifference = camera.transform.position.x - newPosition.x;
	// 	float zDifference = camera.transform.position.z - newPosition.z;
	// 	Vector3 differenceToNewPosition = new Vector3(xDifference, 0, zDifference);

	// 	if (currentState == CameraState.TRANSITIONING) {
	// 		if (differenceToNewPosition.magnitude > distanceUntilEndTransition) {
	// 			transitionTime = cameraTransitionTime;
	// 		} else {
	// 			currentState = CameraState.CONTROLLED_MOVEMENT;
	// 		}
	// 	}

	// 	return Vector3.SmoothDamp(camera.transform.position, newPosition, ref velocity, transitionTime);
	// }

	private void SeeThroughWalls() {
		Vector3 direction = transform.forward;

		RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity, hiddenLayers);
		Debug.DrawRay(transform.position, direction, Color.yellow);

		foreach (RaycastHit hit in hits) {
			GameObject currentHit = hit.transform.gameObject;

			if (!hiddenObjects.Contains(currentHit)) {
				hiddenObjects.Add(currentHit);
				currentHit.GetComponent<Renderer>().enabled = false;
				currentHit.layer = LayerMask.NameToLayer("WallIgnoreRaycast");
			}
		}

		ClearInappropriateMembers(hits);
	}

	private void ClearInappropriateMembers(RaycastHit[] hits) {
		for (int i = 0; i < hiddenObjects.Count; i++) {
			bool isHit = false;

			for (int j = 0; j < hits.Length; j++) {
				if (hits[j].transform.gameObject == hiddenObjects[i]) {
					isHit = true;
					break;
				}
			}

			if (!isHit) {
				GameObject wasHidden = hiddenObjects[i];
				wasHidden.GetComponent<Renderer>().enabled = true;

				if (wasHidden.layer == LayerMask.NameToLayer("WallIgnoreRaycast")) {
					wasHidden.layer = LayerMask.NameToLayer("Wall");
				}

				hiddenObjects.RemoveAt(i);
				i--;
			}
		}
	}

	#endregion

	#region Coroutine

	private IEnumerator TransitionOrientation(CameraOrientation orientation) {
		currentState = CameraState.TRANSITIONING;

		orientationChangeTimeCurrent = 0;
		Quaternion rotationFrom = angledRotation;
		Quaternion rotationTo = topDownRotation;
		Vector3 heightFrom = new Vector3(transform.position.x, angledHeight, transform.position.z);
		Vector3 heightTo = new Vector3(transform.position.x, topDownHeight, transform.position.z);

		if (orientation == CameraOrientation.ANGLED) {
			rotationFrom = topDownRotation;
			rotationTo = angledRotation;
			heightFrom = new Vector3(transform.position.x, topDownHeight, transform.position.z);
			heightTo = new Vector3(transform.position.x, angledHeight, transform.position.z);
		}

		while (orientationChangeTimeCurrent < orientationChangeTime) {
			float time = orientationChangeTimeCurrent / orientationChangeTime;

			transform.rotation = Quaternion.Lerp(rotationFrom, rotationTo, time);
			transform.position = Vector3.Lerp(heightFrom, heightTo, time);
			orientationChangeTimeCurrent += Time.deltaTime;

			yield return null;
		}

		currentOrientation = orientation;
		currentState = CameraState.CONTROLLED_MOVEMENT;
	}

	#endregion

	#region Enums

	public enum CameraState {
		TRANSITIONING,
		CONTROLLED_MOVEMENT
	}

	public enum CameraOrientation {
		TOP_DOWN,
		ANGLED
	}

	#endregion
}
