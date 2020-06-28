using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region Properties

    public float movementSpeed;

    private new Rigidbody rigidbody;
    private FieldOfView fieldOfView;

    private List<GameObject> allPlayers;
    private GameObject targetPlayer;

    #endregion

    #region Events

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
        fieldOfView = GetComponent<FieldOfView>();

        allPlayers = References.players;
    }

    private void FixedUpdate() {
        MoveTowardsNearestPlayer();
    }

    #endregion

    #region Methods

    private void MoveTowardsNearestPlayer() {
        targetPlayer = FindNearestPlayer();
        Vector3 directionToTarget;

        if (targetPlayer != null) {
            float xDirectionToTarget = targetPlayer.transform.position.x - transform.position.x;
            float zDirectionToTarget = targetPlayer.transform.position.z - transform.position.z;

            directionToTarget = new Vector3(xDirectionToTarget, 0, zDirectionToTarget);
            directionToTarget = directionToTarget.normalized;
        } else {
            directionToTarget = transform.forward;
        }

        Vector3 movementAmount = directionToTarget * (movementSpeed * Time.deltaTime);
        Vector3 newPosition = transform.position + movementAmount;
        rigidbody.MovePosition(newPosition);
        transform.LookAt(newPosition);

        print(movementAmount);
    }

    private GameObject FindNearestPlayer() {
        List<GameObject> playersInEyesight = fieldOfView.visibleTargets;

        if (playersInEyesight.Count <= 0) { return null; }

        GameObject nearestPlayer = playersInEyesight[0];
        float distanceToNearestPlayer = Vector3.Distance(transform.position, nearestPlayer.transform.position);

        for (int i = 1; i < playersInEyesight.Count; i++) {
            GameObject currentPlayer = playersInEyesight[i];
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
