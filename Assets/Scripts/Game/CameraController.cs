using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public float cameraTransitionTime;

	private new Camera camera;
	private Vector3 velocity;
	private List<GameObject> allPlayers;
	private GameObject currentPlayer;

	private float cameraDifferenceToPlayerZPosition;
	private States currentState;

	#region Events

	void Start() {
		allPlayers = References.Player.players;
		camera = Camera.main;
		velocity = Vector3.one;

		FindStartingPlayer();

		cameraDifferenceToPlayerZPosition = currentPlayer.transform.position.z - camera.transform.position.z;
	}

	void Update() {
		if (currentPlayer != null) {
			Vector3 newCameraPosition = new Vector3(currentPlayer.transform.position.x, camera.transform.position.y, currentPlayer.transform.position.z - cameraDifferenceToPlayerZPosition);

			camera.transform.position = ChangePosition(newCameraPosition);
		}
	}

	#endregion

	#region Methods

	public void SetCurrentlyControlledPlayer(GameObject player) {
		currentPlayer = player;
		currentState = States.Transitioning;
	}

	private void FindStartingPlayer() {
		foreach (var currentPlayer in allPlayers) {
			if (currentPlayer.GetComponent<PlayerBehaviour>().currentlyBeingControlled) {
				this.currentPlayer = currentPlayer;
				currentState = States.Following;
				return;
			}
		}
	}

	private Vector3 ChangePosition(Vector3 newPosition) {
		// Only use smooth transition if in the Transitioning state

		float distanceUntilEndTransition = 0.1f;

		float transitionTime = 0;
		float xDifference = camera.transform.position.x - newPosition.x;
		float zDifference = camera.transform.position.z - newPosition.z;
		Vector3 differenceToNewPosition = new Vector3(xDifference, 0, zDifference);

		if (currentState == States.Transitioning) {
			if (differenceToNewPosition.magnitude > distanceUntilEndTransition) {
				transitionTime = cameraTransitionTime;
			} else {
				currentState = States.Following;
			}
		}

		return Vector3.SmoothDamp(camera.transform.position, newPosition, ref velocity, transitionTime);
	}

	#endregion

	#region Enums

	public enum States {
		Transitioning,
		Following
	}

	#endregion
}
