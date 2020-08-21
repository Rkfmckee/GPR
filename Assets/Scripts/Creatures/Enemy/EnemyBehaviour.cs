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
    private Vector3 movementDirection;

    #endregion

    #region Events

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        fieldOfView = GetComponent<FieldOfView>();

        movementDirection = transform.forward;
    }

    private void Start() {
        allPlayers = References.players;
    }

    private void FixedUpdate() {
        MoveTowardsTarget();
    }

    private void OnDestroy() {
        if (References.enemies.Contains(gameObject)) {
            References.enemies.Remove(gameObject);
        }

        if (References.enemies.Count <= 0) {
            References.GameController.roundStage.SetCurrentStage(new PreparingStage());
            print("All enemies dead, Cave defended successfully");
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "WallDecoration") {
            var direction = collision.contacts[0].normal;

            direction = Quaternion.AngleAxis(Random.Range(-70.0f, 70.0f), Vector3.up) * direction;

            movementDirection = direction;
            transform.rotation = Quaternion.LookRotation(movementDirection);
        }
    }

    #endregion

    #region Methods

    private void MoveTowardsTarget() {
        targetPlayer = FindNearestPlayer();

        if (targetPlayer != null) {
            movementDirection = DirectionToNearestPlayer();
        }

        Vector3 movementAmount = movementDirection * (movementSpeed * Time.deltaTime);
        Vector3 newPosition = transform.position + movementAmount;
        rigidbody.MovePosition(newPosition);
        transform.LookAt(newPosition);
    }

    private Vector3 WalkForward() {
        var directionToTarget = transform.forward;
        return directionToTarget;
    }

    private Vector3 DirectionToNearestPlayer() {
        float xDirectionToTarget = targetPlayer.transform.position.x - transform.position.x;
        float zDirectionToTarget = targetPlayer.transform.position.z - transform.position.z;

        var directionToTarget = new Vector3(xDirectionToTarget, 0, zDirectionToTarget);
        directionToTarget = directionToTarget.normalized;

        return directionToTarget;
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
