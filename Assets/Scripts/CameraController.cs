using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraTransitionSpeed;

    private new Camera camera;
    private List<GameObject> allPlayers;
    private GameObject currentPlayer;

    private float cameraDifferenceToPlayerZPosition;

    #region Events

    void Start()
    {
        allPlayers = References.players;
        camera = Camera.main;
        cameraTransitionSpeed = cameraTransitionSpeed * Time.deltaTime;

        FindStartingPlayer();

        cameraDifferenceToPlayerZPosition = currentPlayer.transform.position.z - camera.transform.position.z;
    }

    void Update()
    {
        Vector3 newCameraPosition = new Vector3(currentPlayer.transform.position.x, camera.transform.position.y, currentPlayer.transform.position.z - cameraDifferenceToPlayerZPosition);
        camera.transform.position = Vector3.Lerp(camera.transform.position, newCameraPosition, cameraTransitionSpeed);
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

    #endregion
}
