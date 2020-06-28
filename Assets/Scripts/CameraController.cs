using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraTransitionTime;

    private new Camera camera;
    private Vector3 velocity;
    private List<GameObject> allPlayers;
    private GameObject currentPlayer;

    private float cameraDifferenceToPlayerZPosition;

    #region Events

    void Start()
    {
        allPlayers = References.players;
        camera = Camera.main;
        velocity = Vector3.one;

        FindStartingPlayer();

        cameraDifferenceToPlayerZPosition = currentPlayer.transform.position.z - camera.transform.position.z;
    }

    void Update()
    {
        Vector3 newCameraPosition = new Vector3(currentPlayer.transform.position.x, camera.transform.position.y, currentPlayer.transform.position.z - cameraDifferenceToPlayerZPosition);

        camera.transform.position = ChangePosition(newCameraPosition);
    }

    #endregion

    #region Methods

    public void SetCurrentlyControlledPlayer(GameObject player) {
        currentPlayer = player;
    }

    private void FindStartingPlayer() {
        foreach (var currentPlayer in allPlayers) {
            if (currentPlayer.GetComponent<PlayerBehaviour>().currentlyBeingControlled) {
                this.currentPlayer = currentPlayer;
                return;
            }
        }
    }

    private Vector3 ChangePosition(Vector3 newPosition) {
        // Only use smooth transition if the distance is greater than 0.1

        float transitionTime = 0;
        float xDifference = camera.transform.position.x - newPosition.x;
        float zDifference = camera.transform.position.z - newPosition.z;
        Vector3 differenceToNewPosition = new Vector3(xDifference, 0, zDifference);

        if (differenceToNewPosition.magnitude > 0.5) {
            transitionTime = cameraTransitionTime;
        }

        return Vector3.SmoothDamp(camera.transform.position, newPosition, ref velocity, transitionTime);
    }

    #endregion
}
