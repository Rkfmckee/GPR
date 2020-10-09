using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    #region Properties

    protected Vector3 movementDirection;

    protected GameObject enemyObject;
    protected Transform transform;
    protected Rigidbody rigidbody;


    #endregion

    #region Methods

    protected virtual void SetupProperties(GameObject gameObject) {
        enemyObject = gameObject;
        transform = enemyObject.transform;
        rigidbody = enemyObject.GetComponent<Rigidbody>();
    }

    protected abstract Vector3? FindMovementTarget();

    protected Vector3 DirectionToMovementTarget() {
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

    protected virtual void Move(float movementSpeed) {
        movementDirection = DirectionToMovementTarget();

        Vector3 movementAmount = movementDirection * (movementSpeed * Time.deltaTime);
        Vector3 newPosition = transform.position + movementAmount;
        rigidbody.MovePosition(newPosition);
        transform.LookAt(newPosition);
    }

    #endregion
}