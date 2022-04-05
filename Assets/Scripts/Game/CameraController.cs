using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using static GeneralHelper;

public class CameraController : MonoBehaviour {
	public float cameraTransitionTime;
	public float movementSpeed;
	public float orientationChangeTime;
	[VectorLabels("Minimum", "Maximum")]
	public Vector2 yBounds;

	private Vector2 xBounds;
	private Vector2 zBounds;
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

		UpdateFloorObjectBoundaries();
	}

	void Update() {
		if (currentState == CameraState.TRANSITIONING)
		return;

		if (Input.GetButtonDown("Jump")) {
			ChangeOrientation();
		}

		if (currentOrientation == CameraOrientation.ANGLED) {
			SeeThroughWalls();
		}

		HandleMovement();
	}

	#endregion

	#region Methods

	public void UpdateFloorObjectBoundaries() {
		GameObject[] allFloorObjects = GameObject.FindGameObjectsWithTag("Floor");
		xBounds = new Vector2();
		zBounds = new Vector2();
		float minX, maxX, minZ, maxZ;

		if (allFloorObjects.Length == 0)
			return;
		
		minX = maxX = allFloorObjects[0].transform.position.x;
		minZ = maxZ = allFloorObjects[0].transform.position.z;

		foreach(var floor in allFloorObjects) {
			var position = floor.transform.position;

			if (position.x < minX)
				minX = position.x;
			
			if (position.x > maxX)
				maxX = position.x;

			if (position.z < minZ)
				minZ = position.z;
			
			if (position.z > maxZ)
				maxZ = position.z;
		}

		xBounds.x = minX;
		xBounds.y = maxX;
		zBounds.x = minZ;
		zBounds.y = maxZ;
	}

	private void HandleMovement() {
		float verticalAxis = Input.GetAxis("Vertical");
		float horizontalAxis = Input.GetAxis("Horizontal");
		Vector3 direction = new Vector3(horizontalAxis, verticalAxis, 0);

		if (currentOrientation == CameraOrientation.ANGLED) {
			direction = new Vector3(horizontalAxis, verticalAxis, verticalAxis);
		}

		direction = NormaliseVectorToKeepDeceleration(direction);
		direction += AddScrollMovement();

		Vector3 movementAmount = direction * movementSpeed * Time.deltaTime;
		transform.Translate(movementAmount);
		ClampPositionBoundaries();
	}

	private Vector3 AddScrollMovement() {
		if (currentOrientation != CameraOrientation.TOP_DOWN)
			return Vector3.zero;
		
		float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
		return Vector3.forward * scrollAxis * 50;
	}

	private void ClampPositionBoundaries() {
		// Positions stored as Vector2's
		// X = minimum, Y = maximum
		
		Vector3 clampedPosition = transform.position;
		float zOffset = 0;

		if (currentOrientation == CameraOrientation.TOP_DOWN) {
			float clampedY = Mathf.Clamp(clampedPosition.y, yBounds.x, yBounds.y);
			clampedPosition.y = clampedY;
		} else {
			zOffset = -6;
		}

		float clampedX = Mathf.Clamp(clampedPosition.x, xBounds.x, xBounds.y);
		clampedPosition.x = clampedX;
		float clampedZ = Mathf.Clamp(clampedPosition.z, zBounds.x + zOffset, zBounds.y + zOffset);
		clampedPosition.z = clampedZ;

		transform.position = clampedPosition;
	}

	private void ChangeOrientation() {
		CameraOrientation orientation = CameraOrientation.TOP_DOWN;

		if (currentOrientation == CameraOrientation.TOP_DOWN) {
			orientation = CameraOrientation.ANGLED;
		}

		StartCoroutine(TransitionOrientation(orientation));
	}

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
				ClearHiddenObject(hiddenObjects[i]);

				hiddenObjects.RemoveAt(i);
				i--;
			}
		}
	}

	private void ClearAllHiddenObjects() {
		foreach(var hiddenObject in hiddenObjects) {
			ClearHiddenObject(hiddenObject);
		}

		hiddenObjects.Clear();
	}

	private void ClearHiddenObject(GameObject hiddenObject) {
		hiddenObject.GetComponent<Renderer>().enabled = true;

		if (hiddenObject.layer == LayerMask.NameToLayer("WallIgnoreRaycast")) {
			hiddenObject.layer = LayerMask.NameToLayer("Wall");
		}
	}

	private Vector3 ChangePosition(Vector3 newPosition) {
		// Only use smooth transition if in the Transitioning state

		float distanceUntilEndTransition = 0.1f;

		float transitionTime = 0;
		float xDifference = camera.transform.position.x - newPosition.x;
		float zDifference = camera.transform.position.z - newPosition.z;
		Vector3 differenceToNewPosition = new Vector3(xDifference, 0, zDifference);

		if (currentState == CameraState.TRANSITIONING) {
			if (differenceToNewPosition.magnitude > distanceUntilEndTransition) {
				transitionTime = cameraTransitionTime;
			} else {
				currentState = CameraState.CONTROLLED_MOVEMENT;
			}
		}

		return Vector3.SmoothDamp(camera.transform.position, newPosition, ref velocity, transitionTime);
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

		if (orientation == CameraOrientation.TOP_DOWN) {
			ClearAllHiddenObjects();
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
