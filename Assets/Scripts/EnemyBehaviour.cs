using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region Properties

    public float movementSpeed;

    private new Rigidbody rigidbody;
    private List<GameObject> players;
    private GameObject targetPlayer;

    #endregion

    #region Events

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();

        players = References.players;
    }

    private void Update() {
        targetPlayer = FindNearestPlayer();

        float xDirectionToTarget = transform.position.x - targetPlayer.transform.position.x;
        float zDirectionToTarget = transform.position.z - targetPlayer.transform.position.z;

        Vector3 directionToTarget = new Vector3(xDirectionToTarget, 0, zDirectionToTarget);
        directionToTarget = directionToTarget.normalized;
        Vector3 movementAmount = directionToTarget * (movementSpeed * Time.deltaTime);

        Vector3 newPosition = transform.position - movementAmount;
        rigidbody.MovePosition(newPosition);
        transform.LookAt(newPosition);
    }

    #endregion

    #region Methods

    private GameObject FindNearestPlayer() {
        GameObject nearestPlayer = players[0];
        float distanceToNearestPlayer = Vector3.Distance(transform.position, nearestPlayer.transform.position);

        for (int i = 1; i < players.Count; i++) {
            GameObject currentPlayer = players[i];
            float distanceToCurrentPlayer = Vector3.Distance(transform.position, currentPlayer.transform.position);

            if (distanceToCurrentPlayer < distanceToNearestPlayer) {
                nearestPlayer = currentPlayer;
                distanceToNearestPlayer = distanceToCurrentPlayer;
            }

        }

        return nearestPlayer;
    }

    #endregion
}
