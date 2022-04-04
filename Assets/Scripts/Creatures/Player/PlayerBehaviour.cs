using System;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {
	#region Properties

	public bool currentlyBeingControlled;
	public float movementSpeed;

	private CharacterController characterController;
	private AnimatorController animatorController;

	#endregion

	#region Events

	private void Awake() {
		References.Player.players.Add(gameObject);
		if (currentlyBeingControlled) { References.Player.currentPlayer = gameObject; }

		characterController = GetComponent<CharacterController>();
		animatorController = GetComponent<AnimatorController>();
	}

	private void Update() {
		if (currentlyBeingControlled) {
			HandleMovement();
		}
	}

	#endregion

	#region Methods

	public void SetCurrentlyBeingControlled(bool isControlled) {
		currentlyBeingControlled = isControlled;

		if (currentlyBeingControlled) {
			References.Player.currentPlayer = gameObject;

			var cameraController = Camera.main.GetComponent<CameraController>();

			if (cameraController != null) {
				cameraController.SetCurrentlyControlledPlayer(gameObject);
			}
		}
	}

	private void HandleMovement() {
		float zDirection = Input.GetAxis("Vertical");
		float xDirection = Input.GetAxis("Horizontal");
		Vector3 direction = new Vector3(xDirection, 0, zDirection);

		direction = NormaliseVectorToKeepDeceleration(direction);
		Vector3 movementAmount = direction * movementSpeed;
		characterController.SimpleMove(movementAmount);

		if (movementAmount.magnitude > 0) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementAmount), 0.15F);
		}

		float animationVerticalMovement = Mathf.Abs(zDirection) + Mathf.Abs(xDirection);
		animatorController.UpdateAnimatorValues(animationVerticalMovement, 0);
	}

	private Vector3 NormaliseVectorToKeepDeceleration(Vector3 vector) {
		// Normalizing a decimal vector rounds it to 1, which causes weird deceleration
		// So don't do that if it's between 1 and -1

		if (Math.Abs(vector.magnitude) > 1) {
			vector = vector.normalized;
		}

		return vector;
	}

	#endregion
}
