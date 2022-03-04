using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase : EnemyState {
    public EnemyStateChase(GameObject enemyObj) : base(enemyObj) {
    }

    public override void Update() {
    }

    public override void FixedUpdate() {
        MoveTowardsTarget();
    }

    public override void OnCollisionEnter(Collision collision) {
    }

    private void MoveTowardsTarget() {
        movementDirection = DirectionToMovementTarget();

        Vector3 movementAmount = movementDirection * (movementSpeed * Time.deltaTime);
        Vector3 newPosition = transform.position + movementAmount;
        rigidbody.MovePosition(newPosition);
        transform.LookAt(newPosition);
    }

    private Vector3? FindMovementTarget() {
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

        return nearestPlayer.transform.position;
    }

    private Vector3 DirectionToMovementTarget() {
        Vector3? movementTarget = FindMovementTarget();

        if (movementTarget.HasValue) {
            float xDirectionToTarget = movementTarget.Value.x - transform.position.x;
            float zDirectionToTarget = movementTarget.Value.z - transform.position.z;

            var directionToTarget = new Vector3(xDirectionToTarget, 0, zDirectionToTarget);
            directionToTarget = directionToTarget.normalized;

            return directionToTarget;
        } else {
            return Vector3.zero;
        }
    }
}
