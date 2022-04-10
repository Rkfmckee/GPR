using UnityEngine;

public class FriendlyBehaviour : MonoBehaviour {
	#region Properties

	public float movementSpeed;

	private FriendlyState currentState;
	private bool currentlyControlled;


	protected AnimatorController animatorController;

	#endregion

	#region Methods

		#region Get/Set

		public FriendlyState GetCurrentState() {
			return currentState;
		}

		public void SetCurrentState(FriendlyState state) {
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
