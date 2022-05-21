using UnityEngine;

public class FriendlyBehaviour : MonoBehaviour {
	#region Properties

	public float movementSpeed;

	private FriendlyState currentState;
	private bool currentlyControlled;

	private new Rigidbody rigidbody;
	private CameraController cameraController;

	#endregion

	#region Events

	protected virtual void Awake() {
		rigidbody = GetComponent<Rigidbody>();
		cameraController = Camera.main.GetComponent<CameraController>();
	}

	#endregion

	#region Methods

		#region Get/Set

		public FriendlyState GetCurrentState() {
			return currentState;
		}

		public void SetCurrentState(FriendlyState state) {
			var shouldFreeze = state is FriendlyStateIdle ||
								state is FriendlyStateListening;
			ShouldFreezeRigidbody(shouldFreeze);

			currentState = state;
		}

		public bool IsCurrentlyControlled() {
			return currentlyControlled;
		}

		public void SetCurrentlyControlled(bool controlled) {
			currentlyControlled = controlled;
			SetCurrentState(new FriendlyStateListening(gameObject));
		}

		#endregion

	public void ShouldFreezeRigidbody(bool shouldFreeze) {
		rigidbody.isKinematic = shouldFreeze;
		rigidbody.freezeRotation = shouldFreeze;
	}

	#endregion
}
