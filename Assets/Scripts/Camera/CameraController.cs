using UnityEngine;
using static GeneralHelper;
using static CameraOrientationController;

public class CameraController : MonoBehaviour
{
	#region Fields

	[SerializeField]
	private float cameraTransitionTime;
	[SerializeField]
	private float movementSpeed;
	[SerializeField][VectorLabels("Minimum", "Maximum")]
	private Vector2 yBounds;

	private Vector2 xBounds;
	private Vector2 zBounds;
	private Vector3 velocity;
	private float clampZOffset;
	private CameraControllingState controllingState;
	private CameraMovementState movementState;

	private CameraOrientationController orientationController;

	#endregion

	#region Properties

	public Vector2 ZBounds { get => zBounds; }

	public float ClampZOffset { get => clampZOffset; }

	public CameraControllingState ControllingState { get => controllingState; set => controllingState = value; }

	public CameraMovementState MovementState { get => movementState; set => movementState = value; }

	#endregion

	#region Events

	private void Awake()
	{
		References.Camera.camera = GetComponent<Camera>();
		References.Camera.cameraController = this;

		orientationController = GetComponent<CameraOrientationController>();

		clampZOffset = 6;
		controllingState = CameraControllingState.ControllingSelf;
		movementState = CameraMovementState.ControlledByPlayer;

		(xBounds, zBounds) = GeneralHelper.GetFloorObjectBoundaries(false);
		GeneralHelper.GetLevelSize();
	}

	void Update()
	{
		if (movementState == CameraMovementState.Transitioning
			|| controllingState == CameraControllingState.ControllingMenu)
		{
			return;
		}

		HandleMovement();
	}

	#endregion

	#region Methods

	private void HandleMovement()
	{
		float verticalAxis   = Input.GetAxis("Vertical");
		float horizontalAxis = Input.GetAxis("Horizontal");
		Vector3 direction    = new Vector3(horizontalAxis, verticalAxis, 0);

		if (orientationController.CurrentOrientation == CameraOrientation.ANGLED)
		{
			direction = new Vector3(horizontalAxis, verticalAxis, verticalAxis);
		}

		direction  = NormaliseVectorToKeepDeceleration(direction);
		direction += AddScrollMovement();

		Vector3 movementAmount = direction * movementSpeed * Time.deltaTime;
		transform.Translate(movementAmount);
		ClampPositionBoundaries();
	}

	private Vector3 AddScrollMovement()
	{
		if (orientationController.CurrentOrientation != CameraOrientation.TOP_DOWN)
			return Vector3.zero;

		float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
		return Vector3.forward * scrollAxis * 50;
	}

	private void ClampPositionBoundaries()
	{
		// Positions stored as Vector2's
		// X = minimum, Y = maximum

		var clampedPosition = transform.position;
		var zOffset         = 0f;

		if (orientationController.CurrentOrientation == CameraOrientation.TOP_DOWN)
		{
			var clampedY = Mathf.Clamp(clampedPosition.y, yBounds.x, yBounds.y);
			clampedPosition.y = clampedY;
		}
		else
		{
			zOffset = -clampZOffset;
		}

		var clampedX      = Mathf.Clamp(clampedPosition.x, xBounds.x, xBounds.y);
		clampedPosition.x = clampedX;
		var clampedZ      = Mathf.Clamp(clampedPosition.z, zBounds.x + zOffset, zBounds.y + zOffset);
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

	public enum CameraControllingState
	{
		ControllingSelf,
		ControllingFriendly,
		ControllingObstaclePlacement,
		ControllingMenu
	}

	public enum CameraMovementState
	{
		Transitioning,
		ControlledByPlayer
	}

	#endregion
}
