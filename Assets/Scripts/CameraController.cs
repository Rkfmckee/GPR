using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private new Camera camera;
    private GameObject player;

    private float cameraDifferenceToPlayerZPosition;
    private float cameraTransitionSpeed;

    void Start()
    {
        findCurrentPlayer();

        camera = Camera.main;
        cameraDifferenceToPlayerZPosition = player.transform.position.z - camera.transform.position.z;
        cameraTransitionSpeed = 1 * Time.deltaTime;
    }

    void Update()
    {
        findCurrentPlayer();

        Vector3 newCameraPosition = new Vector3(player.transform.position.x, camera.transform.position.y, player.transform.position.z - cameraDifferenceToPlayerZPosition);
        camera.transform.position = Vector3.Lerp(camera.transform.position, newCameraPosition, cameraTransitionSpeed);
    }

    private void findCurrentPlayer() {
        var allPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (var currentPlayer in allPlayers) {
            if (currentPlayer.GetComponent<PlayerBehaviour>().currentlyBeingControlled) {
                player = currentPlayer;
                return;
            }
        }
    }
}
