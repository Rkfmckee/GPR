using UnityEngine;
using static GeneralHelper;
using static CameraOrientationController;

public class CameraController : MonoBehaviour {
	#region Properties

	public float cameraTransitionTime;
	public float movementSpeed;
	[VectorLabels("Minimum", "Maximum")]
	public Vector2 yBounds;

	private Vector2 xBounds;
	private Vector2 zBounds;
	private Vector3 velocity;
	private ControllingState controllingState;
	private CameraMovementState movementState;
	private float clampZOffset;

	private CameraOrientationController orientationController;

	#endregion

	#region Events

	private void Awake() {
		orientationController = GetComponent<CameraOrientationController>();

		clampZOffset = 6;
		controllingState = ControllingState.ControllingSelf;
		movementState = CameraMovementState.ControlledByPlayer;

		UpdateFloorObjectBoundaries();
	}

	void Update() {
		if (movementState == CameraMovementState.Transitioning)
		return;

		HandleMovement();
	}

	#endregion

	#region Methods

		#region Get/Set

		public ControllingState GetControllingState() {
			return controllingState;
		}

		public void SetControllingState(ControllingState state) {
			controllingState = state;
		}

		public CameraMovementState GetMovementState() {
			return movementState;
		}

		public void SetMovementState(CameraMovementState state) {
			movementState = state;
		}

		public Vector2 GetZBounds() {
			return zBounds;
		}

		public float GetClampZOffset() {
			return clampZOffset;
		}

		#endregion

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

		if (orientationController.GetCurrentOrientation() == CameraOrientation.ANGLED) {
			direction = new Vector3(horizontalAxis, verticalAxis, verticalAxis);
		}

		direction = NormaliseVectorToKeepDeceleration(direction);
		direction += AddScrollMovement();

		Vector3 movementAmount = direction * movementSpeed * Time.deltaTime;
		transform.Translate(movementAmount);
		ClampPositionBoundaries();
	}

	private Vector3 AddScrollMovement() {
		if (orientationController.GetCurrentOrientation() != CameraOrientation.TOP_DOWN)
			return Vector3.zero;
		
		float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
		return Vector3.forward * scrollAxis * 50;
	}

	private void ClampPositionBoundaries() {
		// Positions stored as Vector2's
		// X = minimum, Y = maximum
		
		Vector3 clampedPosition = transform.position;
		float zOffset = 0;

		if (orientationController.GetCurrentOrientation() == CameraOrientation.TOP_DOWN) {
			float clampedY = Mathf.Clamp(clampedPosition.y, yBounds.x, yBounds.y);
			clampedPosition.y = clampedY;
		} else {
			zOffset = -clampZOffset;
		}

		float clampedX = Mathf.Clamp(clampedPosition.x, xBounds.x, xBounds.y);
		clampedPosition.x = clampedX;
		float clampedZ = Mathf.Clamp(clampedPosition.z, zBounds.x + zOffset, zBounds.y + zOffset);
		clampedPosition.z = clampedZ;

		transform.position = clampedPosition;
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

	#endregion

	#region Enums

		public enum ControllingState {
		ControllingSelf,
		ControllingFriendly,
		ControllingMenu
	}
	
	public enum CameraMovementState {
		Transitioning,
		ControlledByPlayer
	}

	#endregion
}
