using UnityEngine;

public class FriendlyBehaviour : MonoBehaviour
{
	#region Fields

	[SerializeField]
	private float movementSpeed;

	private FriendlyState currentState;
	private bool currentlyControlled;

	private new Rigidbody rigidbody;
	private CameraController cameraController;

	#endregion

	#region Properties

	public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

	public FriendlyState CurrentState
	{
		get => currentState;
		set
		{
			var shouldFreeze = value is FriendlyStateIdle ||
							   value is FriendlyStateListening;
			ShouldFreezeRigidbody(shouldFreeze);

			currentState = value;
		}
	}

	public bool CurrentlyControlled
	{
		get => currentlyControlled;
		set
		{
			currentlyControlled = value;
			CurrentState        = new FriendlyStateListening(gameObject);
		}
	}

	#endregion

	#region Events

	protected virtual void Awake()
	{
		rigidbody        = GetComponent<Rigidbody>();
		cameraController = Camera.main.GetComponent<CameraController>();
	}

	#endregion

	#region Methods

	public void ShouldFreezeRigidbody(bool shouldFreeze)
	{
		rigidbody.isKinematic    = shouldFreeze;
		rigidbody.freezeRotation = shouldFreeze;
	}

	#endregion
}
