using UnityEngine;
using static CameraController;

public class FriendlyBehaviour : MonoBehaviour {
	#region Properties

	public float movementSpeed;

	private FriendlyState currentState;
	private bool currentlyControlled;

	private CameraController cameraController;

	#endregion

	#region Events

	protected virtual void Awake() {
		cameraController = Camera.main.GetComponent<CameraController>();
	}

	#endregion

	#region Methods

		#region Get/Set

		public FriendlyState GetCurrentState() {
			return currentState;
		}

		public void SetCurrentState(FriendlyState state) {
			if (state is FriendlyStateListening) {
				cameraController.SetControllingState(ControllingState.ControllingFriendly);
			} else {
				// Only change camera controlling if we're leaving Listening state
				if (currentState is FriendlyStateListening) {
					print("Change to controlling self");
					cameraController.SetControllingState(ControllingState.ControllingSelf);
				}
			}
			
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

	#endregion
}
